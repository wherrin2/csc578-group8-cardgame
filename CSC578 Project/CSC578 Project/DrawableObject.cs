using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC578_Project
{
    class DrawableObject : GameObject
    {
        public Image FrontImage { get; set; }
        public Image BackImage { get; set; }
        public int LayerID { get; set; }

        public void BringToFront()
        {

        }
        public void SendToBack()
        {

        }
    }
}
