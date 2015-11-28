using System.Drawing;

namespace CSC578_Project
{
    class DrawableObject : GameObject
    {
        public Image FrontImage { get; set; }
        public Image BackImage { get; set; }
        public bool IsFrontImage { get; set; } = true;
        public bool IsBackgroundImage { get; set; }
        public int LayerId { get; set; }

        public void BringToFront()
        {
           
        }
        public void SendToBack()
        {

        }
    }
}
