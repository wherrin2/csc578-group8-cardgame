using System.Collections.Generic;
using System.Windows;

namespace CSC578_Project
{
    class BoundaryObject : GameObject
    {
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
