using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Animation;


namespace CSC578_Project
{
    public sealed class PlayingSurfaceManager
    {
        private static List<GameObject> gameObjects = new List<GameObject>();
        private static PlayingSurface surface;

        static PlayingSurfaceManager()
        {
            CreatePlayingSurface();
        }

        public static void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            surface.AddGameObject(gameObject);       
        }

        public static bool RemoveGameObject(GameObject gameObject)
        {
            surface.RemoveGameObject(gameObject);
            return gameObjects.Remove(gameObject);
        }

        public static void NewGame()
        {
            CreatePlayingSurface();
        }

        public static void ShowPlayingSurface()
        {
            surface?.Show();
        }

        public static void HidePlayingSurface()
        {
            surface?.Hide();
        }

        public static void MoveGameObjectsAndSetValues(int ownerId, int newOwnerId, Position movePosition, ExpressionSet[] expressions)
        {
            try
            {
                var gameObject = gameObjects.First(x => x.OwnerId == ownerId && x.GetType() == typeof(MovableObject));
                if (gameObject != null)
                {
                    foreach (var expression in expressions)
                    {
                        if(LogicExpressionHandler.EvaluateCurrentGameObject(expression, gameObject))
                            surface.RedrawObject(gameObject);
                    }
                    gameObject.OwnerId = newOwnerId;
                    gameObject.Position = movePosition;
                    surface.MoveGameObject(gameObject, movePosition, true);
                }
            }
            catch (InvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private static void CreatePlayingSurface()
        {
            gameObjects.Clear();
            surface?.Dispose();
            surface = new PlayingSurface();
            surface.GameObjectHasMoved += CheckMovement;
            surface.GameHasStarted += GameHasStarted;
            surface.GameHasBeenClosed += GameHasEnded;
        }
        private static void CheckMovement(object sender, GameObjectEventArgs e)
        {
            var gameObject = (GameObject) sender;
            var boundaryObject = (BoundaryObject) e.CollidingObject;
            //validate and move

            var nearestGridPoint = boundaryObject.GetNearestGridPoint(e.Position);
            gameObject.Position = nearestGridPoint;
            surface.MoveGameObject(gameObject, nearestGridPoint, true);

        }

        private static void GameHasStarted(object sender, EventArgs e)
        {
            GameEngine.Instance.ExecuteLogicObjects();
        }

        private static void GameHasEnded(object sender, EventArgs e)
        {
            //tell form to reappear
        }
    }
}
