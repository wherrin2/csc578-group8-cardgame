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
        public Position Position { get; set; }
        public object CollidingObject { get; set; }
    }
}
