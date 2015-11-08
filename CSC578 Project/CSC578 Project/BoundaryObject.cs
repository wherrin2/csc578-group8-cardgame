using System.Collections.Generic;
using System.Drawing;

namespace CSC578_Project
{
    class BoundaryObject : GameObject
    {
        public bool ShowBoundaryOutline { get; set; }
        private List<Point> gridPoints = new List<Point>();
        private List<int> allowedOwnerIDs = new List<int>();

        public void AddGridPoint(Point point)
        {
            gridPoints.Add(point);
        }
        public void AddOwnerID(int id)
        {
            allowedOwnerIDs.Add(id);
        }

        /// <summary>
        /// Checks if point is inside the boundary zone. Checks if the ID is allowed in the zone.
        /// </summary>
        /// <param name="point">Point object was moved to</param>
        /// <param name="requesterID">ID of the player requesting the movement</param>
        /// <returns>True if Point is allowed and inside boundary. Otherwise false.</returns>
        public bool CheckBoundary(Point point, int requesterID)
        {
            if (allowedOwnerIDs.Contains(requesterID))
            { 
                if (point.X >= Position.X && point.X <= Position.X + Width)
                {
                    if (point.Y >= Position.Y && point.Y <= Position.Y + Height)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public Point GetNearestGridPoint(Point point)
        {
            //implement logic
            return point;
        }
    }
}
