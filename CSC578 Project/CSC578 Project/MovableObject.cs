using System.Drawing;

namespace CSC578_Project
{
    class MovableObject : DrawableObject
    {
        public bool Visible { get; set; } = true;
        public bool IsSelected { get; set; }

        public bool IsSelectable(int requesterId)
        {
            return OwnerId == requesterId;
        }
    }
}
