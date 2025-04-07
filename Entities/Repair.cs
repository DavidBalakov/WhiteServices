using System.ComponentModel.DataAnnotations;

namespace Diploma.Entities
{
    public class Repair
    {
        [Key]
        public int Id { get; set; }
        public RepairType RepairType { get; set; }
        public string AdditionalNotes { get; set; }
    }
}