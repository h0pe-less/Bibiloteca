using System.ComponentModel.DataAnnotations;

namespace localLib.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Adresa de email este obligatorie")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este validă")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie")]
        [DataType(DataType.Password)]
        [Display(Name = "Parolă")]
        public string Password { get; set; }
    }
}