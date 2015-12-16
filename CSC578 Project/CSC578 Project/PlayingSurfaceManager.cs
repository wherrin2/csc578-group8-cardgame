using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Animation;


namespace CSC578_Project
{
    public sealed class PlayingSurfaceManager
    {
        private static List<GameObject> gameObjects = new List<GameObject>();
        private static PlayingSurface surface;
        private static bool allowSwaps = false;
        private static int layerID = 0;


        static PlayingSurfaceManager()
        {
            CreatePlayingSurface();
            layerID = 0;
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

        public static void RemoveAllGameObjectsAtPosition(GameObject gameObject)
        {
            var deleteObjects =
                gameObjects.FindAll(x => x.Position.X == gameObject.Position.X && x.Position.Y == gameObject.Position.Y);

            foreach (var deleteObject in deleteObjects)
            {
                RemoveGameObject(deleteObject);
            }
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

        public static bool MoveGameObjects(GameObject gameObject, Position movePosition, ExpressionSet[] expressions)
        {
            try
            {
                if (gameObject != null)
                {
                    foreach (var expression in expressions)
                    {
                        if (LogicExpressionHandler.EvaluateCurrentGameObject(expression, gameObject))
                            surface.RedrawObject(gameObject);
                    }
                    gameObject.Position = movePosition;
                    ((DrawableObject) gameObject).LayerId = layerID++;
                    surface.MoveGameObject(gameObject, movePosition, true);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return false;

        }
        public static bool MoveGameObjectsAndSetValues(GameObject gameObject, int newOwnerId, Position movePosition,
            ExpressionSet[] expressions)
        {
            if (MoveGameObjects(gameObject, movePosition, expressions))
            {
                gameObject.OwnerId = newOwnerId;
                return true;
            }
            return false;

        }

        public static bool MoveGameObjectsAndSetValues(int ownerId, int newOwnerId, Position movePosition, ExpressionSet[] expressions)
        {
            try
            {
                var gameObject = gameObjects.First(x => x.OwnerId == ownerId && x.GetType() == typeof(MovableObject));
                return MoveGameObjectsAndSetValues(gameObject,newOwnerId, movePosition, expressions);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return false;
        }

        public static void AllowSwapsCreateButton()
        {
            allowSwaps = true;
            surface.ShowAllowSwapsButton();
        }

        public static void DisableSwaps()
        {
            allowSwaps = false;
        }

        public static void AddLogicActionToPlayingSurface(LogicAction logicAction)
        {
            surface.AddLogicActionToBoundaries(logicAction);
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
            try
            {
                var gameObject = (GameObject) sender;
                var boundaryObject = (BoundaryObject) e.CollidingObject;
                //validate and move

                var nearestGridPoint = boundaryObject.GetNearestGridPoint(e.Position);

                if (allowSwaps)
                {
                    SwapPositions(gameObject, boundaryObject, nearestGridPoint);
                }
                else
                {
                    gameObject.Position = nearestGridPoint;
                    ((DrawableObject) gameObject).LayerId = layerID++;
                    surface.MoveGameObject(gameObject, nearestGridPoint, true);
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine(x.StackTrace);
            }

        }

        private static void SwapPositions(GameObject gameObject, BoundaryObject boundaryObject, Position nextPosition)
        {
            var collidingObjects =
                gameObjects.FindAll(x => x.Position.X == nextPosition.X && x.Position.Y == nextPosition.Y && x.Name != gameObject.Name && x.OwnerId == gameObject.OwnerId);

            foreach (var collidingObject in collidingObjects)
            {
                if (collidingObject.GetType() == typeof (MovableObject))
                {
                    var movable = (MovableObject) collidingObject;
                    if (!movable.IsLocked)
                    {
                        collidingObject.Position = gameObject.Position;
                        surface.MoveGameObject(collidingObject, gameObject.Position, false);
                        gameObject.Position = nextPosition;
                        surface.MoveGameObject(gameObject, nextPosition, false);
                        return;
                    }
                }
            }

            surface.MoveGameObject(gameObject, gameObject.Position, true);
        }

        private static void GameHasStarted(object sender, EventArgs e)
        {
            GameEngine.Instance.ExecuteStartupLogicObjects();
        }

        private static void GameHasEnded(object sender, EventArgs e)
        {
            //tell form to reappear
        }
    }
}
