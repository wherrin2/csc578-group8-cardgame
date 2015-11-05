
namespace CSC578_Project
{
    public sealed class GameEngine
    {
        private static readonly GameEngine instance = new GameEngine();
        static GameEngine() { }
        private GameEngine() { }

        public static GameEngine Instance
        {
            get
            {
                return instance;
            }

        }
    }
}
