using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class IntelReport
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int TargetId { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    
            public override string ToString()
        {
            return $"Reporter: {ReporterId}, Target: {TargetId}, Time: {Timestamp}, Text: {Text}";
        }
    }

}
