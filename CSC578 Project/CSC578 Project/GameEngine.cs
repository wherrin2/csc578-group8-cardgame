
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

        public bool ValidateGamePackage(GamePackage package)
        {
            //pass package contents to listening modules
            return true;
        }

        public void StartGameInstance(GamePackage package)
        {
            PlayingSurfaceManager.NewGame();
            //pass package to other modules
            PlayingSurfaceManager.ShowPlayingSurface();
        }
    }
}
