using System.ComponentModel.DataAnnotations;

namespace Diploma.Models.ViewModels
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "Потребителското име е задължително")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Паролата е задължителна")]
        public string Password { get; set; }
    }
}