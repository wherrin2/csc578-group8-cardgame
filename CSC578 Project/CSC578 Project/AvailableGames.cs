using System;
using System.Collections.Generic;
using System.IO;

namespace CSC578_Project
{
    public static class AvailableGames
    {
        private static List<GamePackage> games = new List<GamePackage>();
        private static string path = AppDomain.CurrentDomain.BaseDirectory + @"games\";

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
            if (Directory.Exists(path))
            {
                var extensions = new string[] { ".rules", ".cards", ".board", ".players" };
                string[] filesWithExtension = Directory.GetFiles(path, "*" + extensions[0], SearchOption.AllDirectories);
                foreach (string file in filesWithExtension)
                {
                    string fileNameNoExtension = Path.GetFileNameWithoutExtension(file);
                    bool validGame = false;
                    for (int i = 1; i < extensions.Length; i++)
                    {
                        validGame = File.Exists(path + fileNameNoExtension + extensions[i]);
                        if (!validGame)
                            break;  
                    }
                    if (validGame)
                    {
                        var gamePackage = new GamePackage
                        {
                            Path = path,
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
