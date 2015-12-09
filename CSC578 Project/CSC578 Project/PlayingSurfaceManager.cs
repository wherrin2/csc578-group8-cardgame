using System.Collections.Generic;


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
        private static void CreatePlayingSurface()
        {
            gameObjects.Clear();
            surface?.Dispose();
            surface = new PlayingSurface();
            surface.GameObjectHasMoved += CheckMovement;
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
    }
}
