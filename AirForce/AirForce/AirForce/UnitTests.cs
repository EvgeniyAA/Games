using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace AirForce
{
    [TestFixture]
    public class UnitTests
    {
        [Test]
        public void TestShellsDeletionFromAbroad()
        {
            Game game = new Game(1,1);
            game.GameLevel = new Level();
            Direction direction=Direction.None;
            GameObject shell = new Shell(new Point(-1,-1), direction);
            game.Objects = new List<GameObject> {shell};
            game.DeleteWithoutHpOrIfOutside();
            Assert.AreEqual(0, game.Objects.Count);
        }

        [Test]
        public void TestPlanesDeletionFromAbroad()
        {
            Game game = new Game(1, 1);
            game.GameLevel = new Level();
            GameObject heavyPlane = new HeavyPlane(new Point(-60,0));
            game.Objects = new List<GameObject> {heavyPlane};
            game.Objects.Add(new Fighter(new Point(-game.Objects[0].GameObjectSize.X, 0)));
            game.DeleteWithoutHpOrIfOutside();
            Assert.AreEqual(0, game.Objects.Count);
        }
    }
}
