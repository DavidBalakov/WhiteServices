using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Entities
{
    public class Order
    {
        public Order()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        [ForeignKey("RegisteredUser")]
        public string ClientId { get; set; }
        public RegisteredUser? Client { get; set; }
        public DateOnly RegistrationDate { get; set; }

        [ForeignKey("Repair")]
        public string RepairId { get; set; }
        public Repair? Repair { get; set; }
        public OrderStatus OrderStatus { get; set; }

        [ForeignKey("Product")]
        public string ProductId { get; set; }
        public Product? Product { get; set; }

        public string? AdditionalNotes { get; set; }
    }
}
