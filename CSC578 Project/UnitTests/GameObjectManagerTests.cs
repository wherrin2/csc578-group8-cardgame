using System;
using System.Collections.Generic;
using CSC578_Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class GameObjectManagerTests
    {
        /// <summary>
        /// Game Package is null should return false indicating it was not successful.
        /// </summary>
        [TestMethod]
        public void TestOpenGamePackageTestNull()
        {
            var result = GameObjectManager.OpenGamePackage(new GamePackage());
            Assert.AreEqual(result, false);
        }

        /// <summary>
        /// The Game Object Dictionary is also null in this case
        /// </summary>
        [TestMethod]
        public void TestGetGameObjectsWithInvalidKey()
        { 
            var result = GameObjectManager.GetGameObjects("invalid key");
            Assert.AreEqual(result, null);
        }

        /// <summary>
        /// There is a chance that all objects end up in the same position, but unlikely as the number grows.
        /// </summary>
        [TestMethod]
        public void TestShuffle()
        {
            var gameObjects = new List<GameObject>();
            gameObjects.Add(new GameObject() {Name = "1", Id = 1});
            gameObjects.Add(new GameObject() { Name = "2", Id = 2 });
            gameObjects.Add(new GameObject() { Name = "3", Id = 3 });
            gameObjects.Add(new GameObject() { Name = "4", Id = 4 });
            gameObjects.Add(new GameObject() { Name = "5", Id = 5 });
            gameObjects.Add(new GameObject() { Name = "6", Id = 6 });
            gameObjects.Add(new GameObject() { Name = "7", Id = 7 });
            gameObjects.Add(new GameObject() { Name = "8", Id = 8 });
            gameObjects.Add(new GameObject() { Name = "9", Id = 9 });
            gameObjects.Add(new GameObject() { Name = "9", Id = 10 });
            GameObjectManager.Shuffle(gameObjects);
            var result = true;
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].Id != i + 1)
                {
                    result = false;
                    break;
                }
            }
            Assert.AreEqual(result, false);
        }
    }
}
