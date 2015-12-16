
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace CSC578_Project
{
    public sealed class GameEngine
    {
        private static GamePackage currentGamePackage;
        private static readonly GameEngine instance = new GameEngine();
        static GameEngine() { }
        private GameEngine() { }
        private static List<LogicObject> logicObjects = new List<LogicObject>(); 

        public static GameEngine Instance
        {
            get
            {
                return instance;
            }

        }

        private bool ValidateGamePackage()
        {
            return GameObjectManager.OpenGamePackage(currentGamePackage);
        }

        public bool StartGameInstance(GamePackage package)
        {
            currentGamePackage = package;
            PlayingSurfaceManager.NewGame();
            
            if (ValidateGamePackage())
            {
                logicObjects?.Clear();
                var fileKeys = GameObjectManager.GetGameObjectFileKeys();
                foreach (var fileKey in fileKeys)
                {
                    var gameObjects = GameObjectManager.GetGameObjects(fileKey);
                    //hack to shuffle cards
                    if (gameObjects?.Count > 2 && (gameObjects[0].GetType() == typeof(MovableObject) && gameObjects[1].GetType() == typeof(MovableObject)))
                        GameObjectManager.Shuffle(gameObjects);

                    foreach (var gameObject in gameObjects)
                    {
                        PlayingSurfaceManager.AddGameObject(gameObject);
                    }
                }
                PlayingSurfaceManager.ShowPlayingSurface();
                return true;
            }
            return false;

        }

        public bool ExecuteLogicActions()
        {
             var logicActions = GameObjectManager.GetLogicActions();

            foreach (var action in logicActions)
            {
               PlayingSurfaceManager.AddLogicActionToPlayingSurface(action);
            }
            return true;
        }

        public bool ExecuteStartupLogicObjects()
        {
            var logicObjects = GameObjectManager.GetLogicObjects();
            foreach (var logicObject in logicObjects)
            {
                if (logicObject.StartUp)
                {
                    var logicHandler = new LogicObjectHandler(logicObject);
                    logicHandler.ExecuteLogicObject();
                }
            }
            logicObjects.Clear();
            ExecuteLogicActions();
            return true;
        }

        public void ReloadGame()
        {
            if (currentGamePackage != null)
                StartGameInstance(currentGamePackage);
        }


    }
}
