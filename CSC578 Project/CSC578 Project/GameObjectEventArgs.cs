using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC578_Project
{
    public class GameObjectEventArgs : EventArgs
    {
        public Point Position { get; set; }
    }
}
