using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Direction direction=Direction.None;
            GameObject shell = new Shell(new Point(-1,-1), direction);
            game.objects[0]=shell; //in Game myPlane is objects[0]
            game.DeleteShells();
            Assert.AreEqual(0, game.objects.Count);
        }

        [Test]
        public void TestPlanesDeletionFromAbroad()
        {
            Game game = new Game(1, 1);
            GameObject heavyPlane = new HeavyPlane(new Point(-60,0));
            game.objects[0]= heavyPlane;
            game.objects.Add(new Fighter(new Point(-game.objects[0].GameObjectSize.X, 0)));
            game.DeletePlanes();
            Assert.AreEqual(0, game.objects.Count);
        }
    }
}
