using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSC578_Project
{
    class GameObject
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Point Position { get; set; }
        public int ID { get; set; }
        public int OwnerID { get; set; } = -1;

    }
}
