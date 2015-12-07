using System;
using System.Collections.Generic;
using System.IO;

namespace CSC578_Project
{
    /// <summary>
    /// Creates and maintains a list of games that have the appropriate files. The file contents are not validated, only the files existence. 
    /// </summary>
    public static class AvailableGames
    {
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
            if (!Directory.Exists(primaryPath))
                return;

            var extensions = new string[] { ".rules", ".cards", ".board", ".players" };
            var filesWithExtension = Directory.GetFiles(primaryPath, "*" + extensions[0], SearchOption.AllDirectories);
            foreach (var file in filesWithExtension)
            {
                var fileNameNoExtension = Path.GetFileNameWithoutExtension(file);
                var currentPath = Path.GetDirectoryName(file) + @"\";
                var validGame = false;
                for (var i = 1; i < extensions.Length; i++)
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
                    if (File.Exists(currentPath + fileNameNoExtension + ".meta"))
                    {
                        using (StreamReader metaFile = File.OpenText(currentPath + fileNameNoExtension + ".meta"))
                        {
                            string json = metaFile.ReadToEnd();
                            gamePackage.MetaInformation = JsonParser.DeserializeMeta(json);
                        }
                    }
                    games.Add(gamePackage);
                }

            }
        }
    }
}
