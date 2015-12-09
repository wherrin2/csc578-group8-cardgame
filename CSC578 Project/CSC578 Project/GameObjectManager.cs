using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;


namespace CSC578_Project
{
    /// <summary>
    /// Opens specific game package and reads text files into memory.
    /// </summary>
    public static class GameObjectManager
    {
        //string is the key of the class (or text file name) or subclass
        private static Dictionary<string, List<GameObject>> gameObjects;
        private static Random randomNumber = new Random();
        //should add dictionary just for the rules logic if needed

        /// <summary>
        /// Opens Game Package from text files and begins populating objects
        /// </summary>
        /// <param name="package">Package of the game file to be opened</param>
        /// <returns>True if no errors were encountered. False if an error occured.</returns>
        public static bool OpenGamePackage(GamePackage package)
        {
            try
            {
                if (gameObjects == null)
                    gameObjects = new Dictionary<string, List<GameObject>>();
                else
                    gameObjects.Clear();

                foreach (var extension in package.FileExtensions)
                {
                    if (!extension.Contains(".rules"))
                    {
                        string fileKey = extension.Substring(1);
                        gameObjects?.Add(fileKey, OpenGameObject(package.Path + package.Name + extension, fileKey));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        public static List<GameObject> GetGameObjects(string key)
        {
            if (gameObjects != null && gameObjects.ContainsKey(key))
                return gameObjects[key];
            return null;
        } 

        /// <summary>
        /// Players, Board, and Cards file can all be broken down into GameObjects
        /// </summary>
        /// <param name="fileName">Path to specific file (should be Player, Board, or Cards file)</param>
        /// <returns>True if valid file. Does not validate links to other files at this step. False if error occured.</returns>
        private static List<GameObject> OpenGameObject(string fileName, string fileKey)
        {
            using (StreamReader file = File.OpenText(fileName))
            {
                string json = file.ReadToEnd();
                return JsonParser.Deserialize(json, fileKey);
                //should check objects and entries for array. Determine game objects based on if keys exist.
                //is_movable, front_image, allowed_owner_ids would establish the various objects if the keys existed and not false or null 
            }
        }

        /// <summary>
        /// Rules Logic
        /// </summary>
        /// <param name="fileName">Path to Rules file for this game package.</param>
        /// <returns>True if valid file. Does not validate links to other files at this step. False if error occured.</returns>
        private static bool OpenRuleLogic(string fileName)
        {
            return true;
        }

        public static void Shuffle(List<GameObject> gameObjects)
        {
            var count = gameObjects.Count;
            while (count > 1)
            {
                count--;
                var next = randomNumber.Next(count + 1);
                var temp = gameObjects[next];
                gameObjects[next] = gameObjects[count];
                gameObjects[count] = temp;

            }
        }
    }

}
