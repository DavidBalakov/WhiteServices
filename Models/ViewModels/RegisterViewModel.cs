using System.ComponentModel.DataAnnotations;

namespace Diploma.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string? UserId { get; set; }
        [Required(ErrorMessage = "Потребителското име е задължително")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Имейла е задължителен")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Паролата е задължителна")]
        public string Password { get; set; }
        // [Compare("Password", ErrorMessage = "Паролите не съвпадат")]
        // [Required(ErrorMessage = "Потвърдете паролата")]
        // public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Личното име е задължително")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Фамилното име е задължително")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Телефонният номер е задължителен")]
        public string PhoneNumber { get; set; }
        public string? Role { get; set; }

    }
}