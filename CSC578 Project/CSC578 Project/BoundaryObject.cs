using System;
using System.Collections.Generic;
using System.Drawing;

namespace CSC578_Project
{
    public class BoundaryObject : GameObject
    {
        public bool ShowBoundaryOutline { get; set; }
        public Position[] GridPoints { get; set; }
        private List<int> allowedOwnerIds = new List<int>();

        public void AddOwnerId(int id)
        {
            allowedOwnerIds.Add(id);
        }

        /// <summary>
        /// Checks if point is inside the boundary zone. Checks if the ID is allowed in the zone.
        /// </summary>
        /// <param name="position">The position object was moved to</param>
        /// <param name="requesterId">ID of the player requesting the movement</param>
        /// <returns>True if Point is allowed and inside boundary. Otherwise false.</returns>
        public bool CheckBoundary(Position position, int requesterId)
        {
            if (allowedOwnerIds.Contains(requesterId))
            { 
                if (position?.X >= Position.X && position?.X <= Position.X + Width)
                {
                    if (position.Y >= Position.Y && position.Y <= Position.Y + Height)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public Position GetNearestGridPoint(Position position)
        {
            var nearestGridPoint = new Position() { X = int.MaxValue, Y = int.MaxValue };
            if (position != null && GridPoints != null)
            {
                var nearestValue = int.MaxValue;
                foreach (var gridPoint in GridPoints)
                {
                    var nextValue = Math.Abs(gridPoint.X - position.X) + Math.Abs(gridPoint.Y - position.Y);
                    if (nextValue < nearestValue)
                    {
                        nearestValue = nextValue;
                        nearestGridPoint = gridPoint;
                    }

                }
            }
            return nearestGridPoint;
        }
    }
}
