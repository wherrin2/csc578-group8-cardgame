using System.Drawing;

namespace CSC578_Project
{
    class MovableObject : DrawableObject
    {
        public bool Visible { get; set; } = true;
        public bool IsSelected { get; set; }

        public bool Move(Point point, int requesterID)
        {
            return false;
        }

        public bool IsSelectable(int requesterID)
        {
            return OwnerID == requesterID;
        }
    }
}
