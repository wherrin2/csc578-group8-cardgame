using System;
using System.Collections.Generic;
using System.IO;

namespace CSC578_Project
{
    public static class AvailableGames
    {
        private static List<string> games = new List<string>();
        private static string path = AppDomain.CurrentDomain.BaseDirectory;
        private static string gamesDirectory = @"games\";

        public static List<string> GetAvailableGames()
        {
            if (games.Count == 0)
                SearchForGames();

            return games;
        }
        public static string GameNameWithoutExtension(string game)
        {
            return (Path.GetFileNameWithoutExtension(game));
        }
        public static void RefreshGamesList()
        {
            games = new List<string>();
            SearchForGames();
        }

        private static void SearchForGames()
        {
            if (Directory.Exists(path + gamesDirectory))
            {
                var extensions = new List<string> { ".rules", ".cards", ".board", ".players" };
                var filesWithExtension = Directory.GetFiles(path + gamesDirectory, "*" + extensions[0], SearchOption.AllDirectories);
                foreach (var file in filesWithExtension)
                {
                    var fileNameNoExtension = Path.GetFileNameWithoutExtension(file);
                    var validGame = false;
                    for (int i = 1; i < extensions.Count; i++)
                    {
                        validGame = File.Exists(path + gamesDirectory + fileNameNoExtension + extensions[i]);
                        if (!validGame)
                            break;  
                    }
                    if (validGame)
                        games.Add(file);

                }
            }
        }
    }
}
