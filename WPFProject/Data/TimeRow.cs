using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class TimeRow
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public String TaskName { get; set; }
        public bool IsAddedManually { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan DurationAddedManually { get; set; }

        public string DesignerCode { get; set; }
        public string MainDesignerCode { get; set; }
        public string ClientCode { get; set; }
        public string TraderCode { get; set; }
    }
}
