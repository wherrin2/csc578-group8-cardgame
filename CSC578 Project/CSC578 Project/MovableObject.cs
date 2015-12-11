using System.Drawing;

namespace CSC578_Project
{
    class MovableObject : DrawableObject
    {
        public bool Visible { get; set; } = true;
        public bool IsSelected { get; set; }
        public bool IsLocked { get; set; }
        public int Rank { get; set; }
        public int Value { get; set; }

        public bool IsSelectable(int requesterId)
        {
            return (!IsLocked && OwnerId == requesterId);
        }
    }
}
