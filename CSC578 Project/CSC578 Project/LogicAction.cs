using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC578_Project
{
    public class LogicAction
    {
        public string Event { get; set; }
        public bool Loop { get; set; }
        public bool AllowContinue { get; set; }
        public int Priority { get; set; }
        public LogicObject Actions { get; set; }
        public string[] BoundaryRules { get; set; }
    }
}
