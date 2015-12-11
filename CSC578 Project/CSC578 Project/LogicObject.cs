using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC578_Project
{
    public class LogicObject
    {
        public string Name { get; set; }
        public ExpressionSet[] ExpressionSet { get; set; }
        public string InternalAction { get; set; }
        public int Priority { get; set; }
        public bool RunOnce { get; set; }
        public bool IsDelayed { get; set; }
        public bool Loop { get; set; }
        public LogicObject WaitFor { get; set; }

        public void ProcessInternalAction()
        {
           
        }
    }
}
