using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Diploma.Models.ViewModels.Products
{
    public class ProductViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Полето \"Сериен номер\" е задължително!")]
        [DisplayName("Сериен номер")]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "Полето \"Марка\" е задължително!")]
        [DisplayName("Марка")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Полето \"Модел\" е задължително!")]
        [DisplayName("Модел")]
        public string Modelz { get; set; }
    }
}