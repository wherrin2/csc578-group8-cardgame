using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;


namespace CSC578_Project
{
    /// <summary>
    /// Opens specific game package and reads text files into memory.
    /// </summary>
    public static class GameObjectManager
    {
        //string is the key of the class (or text file name) or subclass
        //private static Dictionary<string, List<GameObject>> gameObjects;
        private static Dictionary<string, Dictionary<string, GameObject>> gameObjects; 
        private static readonly Random randomNumber = new Random();
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
                    gameObjects = new Dictionary<string, Dictionary<string, GameObject>>();
                else
                    gameObjects.Clear();

                foreach (var extension in package.FileExtensions)
                {
                    if (!extension.Contains(".rules"))
                    {
                        string fileKey = extension.Substring(1);
                        var objects = new Dictionary<string, GameObject>();
                        foreach (var gameObject in OpenGameObject(package.Path + package.Name + extension, fileKey))
                        {
                            objects.Add(gameObject.Name, gameObject);
                        }
                        gameObjects?.Add(fileKey, objects);
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
            {
                return gameObjects[key].Values.ToList();
            }
            return null;
        }

        public static GameObject GetGameObjectAny(string fileKey, string partialKey)
        {
            if (gameObjects != null && gameObjects.ContainsKey(fileKey))
            {
                var found = gameObjects[fileKey].Keys.First(k => k.Contains(partialKey));
                if (found != null)
                    return gameObjects[fileKey][found];
            }
            return null;
        }

        public static List<GameObject> GetGameObjectAll(string fileKey, string partialKey)
        {
            var foundGameObjects = new List<GameObject>();
            if (gameObjects != null && gameObjects.ContainsKey(fileKey))
            {
                var foundKeys = gameObjects[fileKey].Keys.Where(k => k.Contains(partialKey)).ToList();
                foundGameObjects.AddRange(foundKeys.Select(key => gameObjects[fileKey][key]));
            }
            return foundGameObjects;
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

        public static void Shuffle(List<GameObject> objects)
        {
            var count = objects.Count;
            while (count > 1)
            {
                count--;
                var next = randomNumber.Next(count + 1);
                var temp = objects[next];
                objects[next] = objects[count];
                objects[count] = temp;

            }
        }
    }

}
