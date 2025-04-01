using System.ComponentModel.DataAnnotations;

namespace Diploma.Entities
{
    public enum OrderStatus
    {
        Незапочната,
        [Display(Name = "В процес на изпълнение")]
        Незавършена,
        Завършена,
    }
}