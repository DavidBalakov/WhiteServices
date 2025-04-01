using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Entities
{
    public class Repair
    {
        public Repair()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public RepairType RepairType { get; set; }
        [ForeignKey("Product")]
        public string ProductSerialNumber { get; set; }
        public Product Product { get; set; }
        public List<Order> Orders { get; set; }
    }
}
