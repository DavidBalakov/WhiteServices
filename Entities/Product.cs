using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Entities
{
    public class Product
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        public string SerialNumber { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [ForeignKey("RegisteredUser")]
        public string UserId { get; set; }
        public RegisteredUser User { get; set; }
        public DateOnly DateOfPurchase { get; set; }
        public DateOnly WarrantyExpirationDate { get; set; }
    }
}