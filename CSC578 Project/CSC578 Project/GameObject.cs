using System.Drawing;

namespace CSC578_Project
{
    public class GameObject
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Point Position { get; set; }
        public int Id { get; set; }
        public int OwnerId { get; set; } = -1;

    }
}
