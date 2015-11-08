using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC578_Project
{
    public class PlayingSurfaceManager
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        private PlayingSurface surface = new PlayingSurface();
        public PlayingSurfaceManager()
        {
            surface.GameObjectHasMoved += CheckMovement;
            //may move this to the game engine itself
            surface.Show();
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            surface.AddGameObject(gameObject);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
            surface.RemoveGameObject(gameObject);
        }

        private void CheckMovement(object sender, Point e)
        {
            var gameObject = (GameObject) sender;
            // validate movement 
            //if correct assign position
           // gameObject.Position = e;
            //if not valid move back
            //surface.MoveGameObject(gameObject, gameObject.Position, false);
        }
    }
}
