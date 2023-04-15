using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIdentity.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is mandatory")]
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
        [Required(ErrorMessage = "Name is mandatory ")]
        public string Name { get; set; }
        public string Url { get; set; }
        [Display(Name = "Country Code")]
        public int CountryCode { get; set; }
        public string Telephone { get; set; }
        [Required(ErrorMessage = "Country is mandatory ")]
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Birthdate is mandatory")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "State is mandatory")]
        public bool State { get; set; }
    }
}
