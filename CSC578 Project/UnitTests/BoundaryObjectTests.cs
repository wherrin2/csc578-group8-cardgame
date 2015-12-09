using CSC578_Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class BoundaryObjectTests
    {
        /// <summary>
        /// Standard Test
        /// </summary>
        [TestMethod]
        public void TestBoundaryNearestGridPoint()
        {
            var boundary = new BoundaryObject();
            var positionA = new Position() {X = 200, Y = 200};
            var positionB = new Position() {X = 300, Y = 900};
            var positionC = new Position() {X = -200, Y = -900};
            boundary.GridPoints = new []{ positionA, positionB, positionC};
            var expected = positionA;
            var actual = boundary.GetNearestGridPoint(new Position() {X = 140, Y = 100});
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Negative value test where a position exists with a negative value
        /// </summary>
        [TestMethod]
        public void TestBoundaryNearestGridPointNegative()
        {
            var boundary = new BoundaryObject();
            var positionA = new Position() { X = 200, Y = 200 };
            var positionB = new Position() { X = 300, Y = 900 };
            var positionC = new Position() { X = -200, Y = -900 };
            boundary.GridPoints = new[] { positionA, positionB, positionC };
            var expected = positionC;
            var actual = boundary.GetNearestGridPoint(new Position() { X = -100, Y = -800 });
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test where int.MaxValue is supplied for X & Y positions
        /// </summary>
        [TestMethod]
        public void TestBoundaryNearestGridPointMaxValue()
        {
            var boundary = new BoundaryObject();
            var positionA = new Position() { X = 200, Y = 200 };
            var positionB = new Position() { X = 300, Y = 900 };
            var positionC = new Position() { X = -200, Y = -900 };
            boundary.GridPoints = new[] { positionA, positionB, positionC };
            var expected = positionB;
            var actual = boundary.GetNearestGridPoint(new Position() { X = int.MaxValue, Y = int.MaxValue });
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A null position object is supplied. The current return should be int.MaxValue for both X & Y values
        /// </summary>
        [TestMethod]
        public void TestBoundaryNearestGridPointWithNullPosition()
        {
            var boundary = new BoundaryObject();
            var positionA = new Position() { X = 200, Y = 200 };
            var positionB = new Position() { X = 300, Y = 900 };
            var positionC = new Position() { X = -200, Y = -900 };
            boundary.GridPoints = new[] { positionA, positionB, positionC };
            var expected = new Position() {X = int.MaxValue, Y = int.MaxValue};
            var actual = boundary.GetNearestGridPoint(null);
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
        }


        /// <summary>
        /// Position that has a 'tie' score in X & Y values. The first Grid Point should be the selected value
        /// since the next 'score' shouldn't beat it.
        /// </summary>
        [TestMethod]
        public void TestBoundaryNearestGridPointEqualScore()
        {
            var boundary = new BoundaryObject();
            var positionA = new Position() { X = 200, Y = 200 };
            var positionB = new Position() { X = 300, Y = 300 };
            var positionC = new Position() { X = -200, Y = -900 };
            boundary.GridPoints = new[] { positionA, positionB, positionC };
            var expected = positionA; //should be first value input into array since positionB won't have a better score
            var actual = boundary.GetNearestGridPoint(new Position() { X = 250, Y = 250 });
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// Position that has a 'tie' score in X values. The Y value is closer to PositionA and should be selected.
        /// </summary>
        [TestMethod]
        public void TestBoundaryNearestGridPointEqualScoreX()
        {
            var boundary = new BoundaryObject();
            var positionA = new Position() { X = 200, Y = 200 };
            var positionB = new Position() { X = 300, Y = 300 };
            var positionC = new Position() { X = -200, Y = -900 };
            boundary.GridPoints = new[] { positionA, positionB, positionC };
            var expected = positionA; 
            var actual = boundary.GetNearestGridPoint(new Position() { X = 250, Y = 230 });
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// Position that has a 'tie' score in Y values. The X value is closer to PositionB and should be selected.
        /// </summary>
        [TestMethod]
        public void TestBoundaryNearestGridPointEqualScoreY()
        {
            var boundary = new BoundaryObject();
            var positionA = new Position() { X = 200, Y = 200 };
            var positionB = new Position() { X = 300, Y = 300 };
            var positionC = new Position() { X = -200, Y = -900 };
            boundary.GridPoints = new[] { positionA, positionB, positionC };
            var expected = positionB;
            var actual = boundary.GetNearestGridPoint(new Position() { X = 251, Y = 250 });
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        /// Position test when there have been no grid points entered for the boundary object
        /// </summary>
        [TestMethod]
        public void TestBoundaryNearestGridPointNoGridPoints()
        {
            var boundary = new BoundaryObject();
            var expected = new Position() {X = int.MaxValue, Y = int.MaxValue};
            var actual = boundary.GetNearestGridPoint(new Position() {X = 330, Y = 220});
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(expected.X, actual.X);
        }


        /// <summary>
        /// Standard Check Boundary with a valid ID. Should return true.
        /// </summary>
        [TestMethod]
        public void TestBoundaryCheckBoundary()
        {
            var boundary = new BoundaryObject();
            boundary.AddOwnerId(3);
            boundary.Height = 500;
            boundary.Width = 500;
            //boundary position should be 0,0 by default
            var actual = boundary.CheckBoundary(new Position() {X= 200, Y= 400}, 3);
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Standard Check Boundary with a invalid ID. Should return false.
        /// </summary>
        [TestMethod]
        public void TestBoundaryCheckBoundaryInvalidId()
        {
            var boundary = new BoundaryObject();
            boundary.AddOwnerId(3);
            boundary.Height = 500;
            boundary.Width = 500;
            //boundary position should be 0,0 by default
            var actual = boundary.CheckBoundary(new Position() { X = 200, Y = 400 }, 4);
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Check Boundary where the requested Position is not in the correct boundary. Should return false.
        /// </summary>
        [TestMethod]
        public void TestBoundaryCheckBoundaryInvalidPosition()
        {
            var boundary = new BoundaryObject();
            boundary.AddOwnerId(3);
            boundary.Height = 500;
            boundary.Width = 500;
            //boundary position should be 0,0 by default
            var actual = boundary.CheckBoundary(new Position() { X = 501, Y = 400 }, 3);
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Check Boundary where the value supplied is null. Should return false.
        /// </summary>
        [TestMethod]
        public void TestBoundaryCheckBoundaryNullPosition()
        {
            var boundary = new BoundaryObject();
            boundary.AddOwnerId(3);
            boundary.Height = 500;
            boundary.Width = 500;
            //has correct ID, but invalid position
            var actual = boundary.CheckBoundary(null, 3);
            Assert.AreEqual(false, actual);
        }
    }
}
