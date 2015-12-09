
using CSC578_Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class AvailableGamesTests
    {
        /// <summary>
        /// There is currently a game in the build directory but not in the Testing directory. It should return 0 games.
        /// </summary>
        [TestMethod]
        public void TestAvailableGamesGetAvailableGames()
        {
            var games = AvailableGames.GetAvailableGames();
            var expected = games.Count;
            Assert.AreEqual(expected, 0);
        }

        /// <summary>
        /// The original list and the refreshed list should contain the same information.
        /// </summary>
        [TestMethod]
        public void TestAvailableGamesGetAvailableGamesAndRefresh()
        {
            var gamesOriginal = AvailableGames.GetAvailableGames();
            AvailableGames.RefreshGamesList();
            var gamesRefreshed = AvailableGames.GetAvailableGames();
            Assert.AreEqual(gamesOriginal.Count, gamesRefreshed.Count);
        }
    }
}
