using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Model
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeName { get; set; }

        public virtual ICollection<ShiftItem> ShiftItems { get; set; } = null!;
    }
}
