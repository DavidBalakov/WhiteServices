using System.ComponentModel.DataAnnotations;
using Diploma.Entities;

namespace Diploma.Models.ViewModels
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Задължително")]
        public string ProductId { get; set; }
        [Required(ErrorMessage = "Задължително")]
        public string? ProductModel { get; set; }
        public string? ProductBrand { get; set; }
        public string RegistrationDate { get; set; }
        public RepairType RepairType { get; set; }
        public string? AdditionalNotes { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public string? Id { get; set; }
    }
}