using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftLoggerConsole.Models
{
    public class ShiftModelDto
    {
        public int day { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string duration { get; set; }
    }
}
