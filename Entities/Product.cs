using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Entities
{
    public class Product
    {
        [Key]
        public string SerialNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateOnly DateOfPurchase { get; set; }
        public DateOnly WarrantyExpirationDate { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public RegisteredUser User { get; set; }
    }
}
