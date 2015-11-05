using System;
using System.Collections.Generic;
using System.IO;

namespace CSC578_Project
{
    public static class AvailableGames
    {
        /// <summary>
        /// Creates and maintains a list of games that have the appropriate files. The file contents are not validated, only the files existence. 
        /// </summary>
       
        private static List<GamePackage> games = new List<GamePackage>();
        private static string primaryPath = AppDomain.CurrentDomain.BaseDirectory + @"games\";

        public static List<GamePackage> GetAvailableGames()
        {
            if (games.Count == 0)
                SearchForGames();

            return games;
        }
        public static void RefreshGamesList()
        {
            games = new List<GamePackage>();
            SearchForGames();
        }

        private static void SearchForGames()
        {
            if (Directory.Exists(primaryPath))
            {
                var extensions = new string[] { ".rules", ".cards", ".board", ".players" };
                string[] filesWithExtension = Directory.GetFiles(primaryPath, "*" + extensions[0], SearchOption.AllDirectories);
                foreach (string file in filesWithExtension)
                {
                    string fileNameNoExtension = Path.GetFileNameWithoutExtension(file);
                    string currentPath = Path.GetDirectoryName(file) + @"\";
                    bool validGame = false;
                    for (int i = 1; i < extensions.Length; i++)
                    {
                        validGame = File.Exists(currentPath + fileNameNoExtension + extensions[i]);
                        if (!validGame)
                            break;  
                    }
                    if (validGame)
                    {
                        var gamePackage = new GamePackage
                        {
                            Path = currentPath,
                            Name = fileNameNoExtension,
                            FileExtensions = extensions
                        };

                        games.Add(gamePackage);
                    }

                }
            }
        }
    }
}
