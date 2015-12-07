using System.Drawing;

namespace CSC578_Project
{
    class DrawableObject : GameObject
    {
        public string FrontImage { get; set; }
        public string BackImage { get; set; }
        public bool IsFrontImage { get; set; } = true;
        public bool IsBackgroundImage { get; set; }
        public bool IsRotated { get; set; }
        public int LayerId { get; set; }
        

        public void BringToFront()
        {
           
        }
        public void SendToBack()
        {

        }
    }
}
