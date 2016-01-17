using System.Linq;
using NUnit.Framework;
using RosettaObject;

namespace StdJsonTest
{
    [TestFixture]
    public class MultidimensionalTests
    {
        [Test]
        public void TestEnumeration222()
        {
            var expected = new[]
            {
                new[] {0, 0, 0},
                new[] {0, 0, 1},
                new[] {0, 1, 0},
                new[] {0, 1, 1},
                new[] {1, 0, 0},
                new[] {1, 0, 1},
                new[] {1, 1, 0},
                new[] {1, 1, 1}
            };
            var actual = Multidimensional.EnumerateIndexes(new[] {2, 2, 2}).ToArray();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestEnumeration32()
        {
            var expected = new[]
            {
                new[] {0, 0},
                new[] {0, 1},
                new[] {1, 0},
                new[] {1, 1},
                new[] {2, 0},
                new[] {2, 1},
            };
            var actual = Multidimensional.EnumerateIndexes(new[] { 3, 2 }).ToArray();
            Assert.AreEqual(expected, actual);
        }
    }
}