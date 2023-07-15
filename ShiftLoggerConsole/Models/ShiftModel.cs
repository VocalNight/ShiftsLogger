using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftLoggerConsole.Models
{
    public class ShiftModel
    {
        public long Id { get; set; }
        public int day { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string duration { get; set; }
        public int EmployeeId { get; set; }
    }
}
