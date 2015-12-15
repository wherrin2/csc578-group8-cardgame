using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static List<LogicObject> logicObjects; 
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

                if (logicObjects == null)
                    logicObjects = new List<LogicObject>();
                else
                    logicObjects.Clear();
                

                foreach (var extension in package.FileExtensions)
                {
                    string fileKey = extension.Substring(1);
                    string file = package.Path + package.Name + extension;
                    if (!extension.Contains(".rules"))
                    {
                        var objects = new Dictionary<string, GameObject>();
                        foreach (var gameObject in OpenGameObjects(file, fileKey))
                        {
                            objects.Add(gameObject.Name, gameObject);
                        }
                        gameObjects?.Add(fileKey, objects);
                    }
                    else
                    {
                        logicObjects = OpenLogicObjects(file, fileKey);
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


        public static List<LogicObject> GetLogicObjects()
        {
            return logicObjects;
        } 

        public static List<string> GetGameObjectFileKeys()
        {
            if (gameObjects != null)
            {
                return gameObjects.Keys.ToList();
            }
            return null;
        } 
        public static List<GameObject> GetGameObjects(string key)
        {
            if (gameObjects != null && gameObjects.ContainsKey(key))
            {
                return gameObjects[key].Values.ToList();
            }
            return null;
        }

        public static GameObject GetGameObjectAnyByKey(string fileKey, string partialKey)
        {
            if (gameObjects != null && gameObjects.ContainsKey(fileKey))
            {
                var found = gameObjects[fileKey].Keys.First(k => k.Contains(partialKey));
                if (found != null)
                    return gameObjects[fileKey][found];
            }
            return null;
        }

        public static bool GameObjectExists(string fileKey, string key)
        {
            if (gameObjects.ContainsKey(fileKey))
                return gameObjects[fileKey].ContainsKey(key);
            return false;
        }

        public static List<GameObject> GetGameObjectAllByKey(string fileKey, string partialKey)
        {
            var foundGameObjects = new List<GameObject>();
            if (gameObjects != null && gameObjects.ContainsKey(fileKey))
            {
                var foundKeys = gameObjects[fileKey].Keys.Where(k => k.Contains(partialKey)).ToList();
                foundGameObjects.AddRange(foundKeys.Select(key => gameObjects[fileKey][key]));
            }
            return foundGameObjects;
        }

        public static GameObject GetGameObjectByKey(string fileKey, string key)
        {
            if (gameObjects != null && gameObjects.ContainsKey(fileKey))
                if (gameObjects[fileKey].ContainsKey(key))
                    return gameObjects[fileKey][key];
            return null;
        }


        /// <summary>
        /// Players, Board, and Cards file can all be broken down into GameObjects
        /// </summary>
        /// <param name="fileName">Path to specific file (should be Player, Board, or Cards file)</param>
        /// <param name="fileKey">File Key is the File Extension</param>
        /// <returns>True if valid file. Does not validate links to other files at this step. False if error occured.</returns>
        private static List<GameObject> OpenGameObjects(string fileName, string fileKey)
        {
            return JsonParser.Deserialize(ReadFile(fileName), fileKey);
        }

        /// <summary>
        /// Rules file broken down into logic objects
        /// </summary>
        /// <param name="fileName">Path to specific file</param>
        /// <param name="fileKey">File Key is the File Extension</param>
        /// <returns>List of Logic Objects</returns>
        private static List<LogicObject> OpenLogicObjects(string fileName, string fileKey)
        {
            return JsonParser.DeserializeLogic(ReadFile(fileName), fileKey);
        }

        /// <summary>
        /// Reads file into a string
        /// </summary>
        /// <param name="fileName">File to read into string to try and deserialize</param>
        /// <returns>string ready to deserialize</returns>
        private static string ReadFile(string fileName)
        {
            var json = "";
            try
            {
                using (StreamReader file = File.OpenText(fileName))
                {
                    json = file.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return json;
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
