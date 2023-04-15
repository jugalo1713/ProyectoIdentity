using System.ComponentModel.DataAnnotations;

namespace ProyectoIdentity.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password is mandatory")]
        [StringLength(50, ErrorMessage = "Must have between 5 and 50 characters", MinimumLength = 5)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password confirmation is mandatory")]
        [Compare("Password", ErrorMessage = "Password and password confirmation are different")]
        [Display(Name = "Password Confirmation")]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }
        public string Code { get; set; }
    }
}
