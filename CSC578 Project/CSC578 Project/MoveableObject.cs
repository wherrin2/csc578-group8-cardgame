using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSC578_Project
{
    class MoveableObject : DrawableObject
    {
        public bool Visible { get; set; } = true;
        public bool IsFrontImage { get; set; }

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
