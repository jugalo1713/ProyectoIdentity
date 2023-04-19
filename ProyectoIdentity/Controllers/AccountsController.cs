using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdentity.Models;
using ProyectoIdentity.Models.ViewModels;
using System.Reflection.Metadata.Ecma335;

namespace ProyectoIdentity.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManagaer;
        private readonly SignInManager<IdentityUser> _signInManagaer;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailsender;
        public AccountsController(IMapper mapper, UserManager<IdentityUser> userManagaer, SignInManager<IdentityUser> signInManagaer, IEmailSender emailsender)
        {
            this._mapper = mapper;
            this._userManagaer  = userManagaer;
            this._signInManagaer = signInManagaer;
            this._emailsender = emailsender;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register(string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel, string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = _mapper.Map<AppUser>(viewModel);
                var result = await _userManagaer.CreateAsync(user, viewModel.Password);
                if(result.Succeeded)
                {
                    var code = await _userManagaer.GenerateEmailConfirmationTokenAsync(user);
                    var returnUrl = Url.Action("ConfirmedEmail", "Accounts", new { userId = user.Id, code = code}, protocol: HttpContext.Request.Scheme);
                    await _emailsender.SendEmailAsync(viewModel.Email, "Confirm your account", "To Confirm your email click here:: <a href=\"" + returnUrl + "\">Link</a> ");

                    //await _signInManagaer.SignInAsync(user, false);
                    return RedirectToAction("ConfirmEmail","Accounts");
                }
                ValidateErrors(result);
            }
            return View(viewModel);
        }

        private void ValidateErrors(IdentityResult result)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Access(string? returnurl = null) 
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Access(AccessViewModel viewModel, string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManagaer.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnurl);
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Account is blocked");
                    return View("Blocked");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Access Credentials");
                    return View(viewModel);
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManagaer.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller",""));
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManagaer.FindByEmailAsync(viewModel.Email);
                if (user == null)
                    RedirectToAction("ForgotPasswordConfirmation");

                var code = await _userManagaer.GeneratePasswordResetTokenAsync(user);
                var returnUrl = Url.Action("ResetPassword", "Accounts", new { userId = user.Id, code = code, email = user.Email}, protocol: HttpContext.Request.Scheme);
                await _emailsender.SendEmailAsync(viewModel.Email, "Recuperar contraseña de Julian", "Por favor recupere su contraseña dando click acá: <a href=\""+ returnUrl +"\">enlace</a> ");
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            return View(viewModel);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation() => View();

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword (string? code = null, string? email = null)
        {
            return code == null ? View("Error") : View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManagaer.FindByEmailAsync(viewModel.Email);
                if (user == null)
                    RedirectToAction("RecoverPasswordConfirmation");

                var result = await _userManagaer.ResetPasswordAsync(user, viewModel.Code, viewModel.Password);
                if (result.Succeeded)
                    return RedirectToAction("RecoverPasswordConfirmation");

                ValidateErrors(result);
            }
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RecoverPasswordConfirmation() => View();
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmEmail() => View();

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmedEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                return View("Error");

            var user = await _userManagaer.FindByIdAsync(userId);

            if (user == null) 
                return View("Error");

            var result = await _userManagaer.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmedEmail" : "Error");
        }
    }
}
