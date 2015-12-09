using CSC578_Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class PlayingSurfaceManagerTests
    {
        [TestMethod]
        public void TestRemoveGameObjectThatisInvalid()
        {
            var gameObject = new GameObject() {Id = 99};
            var actual = PlayingSurfaceManager.RemoveGameObject(gameObject);
            Assert.AreEqual(false, actual);
        }
    }
}
