
using System.Collections.Generic;

namespace CSC578_Project
{
    public sealed class GameEngine
    {
        private static GamePackage currentGamePackage;
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
            currentGamePackage = package;
            PlayingSurfaceManager.NewGame();
            if (GameObjectManager.OpenGamePackage(package))
            {
                //remove
                var gameObjects = GameObjectManager.GetGameObjects("cards");
                foreach (var obj in gameObjects)
                {
                    PlayingSurfaceManager.AddGameObject(obj);
                }
                gameObjects = GameObjectManager.GetGameObjects("board");
                foreach (var obj in gameObjects)
                {
                    PlayingSurfaceManager.AddGameObject(obj);
                    
                }
            }
            PlayingSurfaceManager.ShowPlayingSurface();
        }

        public void ReloadGame()
        {
            if (currentGamePackage != null)
                StartGameInstance(currentGamePackage);
        }
    }
}
