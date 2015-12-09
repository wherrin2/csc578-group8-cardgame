using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC578_Project
{
    public class LogicStep
    {
        public int Priority { get; set; }
        public string Name { get; set; }
        public bool RunOnce { get; set; }
        public bool IsDelayed { get; set; }
        public bool Loop { get; set; }
        public LogicStep WaitFor { get; set; }

    }
}
