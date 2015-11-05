using System.Collections.Generic;
using System.Drawing;

namespace CSC578_Project
{
    class BoundaryObject : GameObject
    {
        public bool ShowBoundaryOutline { get; set; }
        private List<Point> gridPoints = new List<Point>();
        private List<int> allowedOwnerIDs = new List<int>();

        public bool CheckBoundary(Point point, int ownerID)
        {
            //implement logic
            return false;
        }
        public Point GetNearestGridPoint(Point point)
        {
            //implement logic
            return point;
        }
    }
}
