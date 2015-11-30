using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSC578_Project
{
    /// <summary>
    /// Opens specific game package and reads text files into memory.
    /// </summary>
    public static class GameObjectManager
    {
        //string is the key of the class (or text file name) or subclass
        private static Dictionary<string, List<GameObject>> gameObjects;
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
                gameObjects?.Clear();
                foreach (var extension in package.FileExtensions)
                {
                    if (!extension.Contains(".rules"))
                     OpenGameObject(package.Path + package.Name + extension);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Players, Board, and Cards file can all be broken down into GameObjects
        /// </summary>
        /// <param name="fileName">Path to specific file (should be Player, Board, or Cards file)</param>
        /// <returns>True if valid file. Does not validate links to other files at this step. False if error occured.</returns>
        private static bool OpenGameObject(string fileName)
        {
            using (StreamReader file = File.OpenText(fileName))
            {
                string json = file.ReadToEnd();
                JsonParser.Deserialize(json);
                //should check objects and entries for array. Determine game objects based on if keys exist.
                //is_movable, front_image, allowed_owner_ids would establish the various objects if the keys existed and not false or null 
            }
            
            return true;
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
    }

}
