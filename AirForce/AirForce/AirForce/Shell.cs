using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirForce
{
    public class Shell : GameObject
    {
        private readonly SolidBrush myshellsBrush;
        private readonly SolidBrush enemyshellsBrush;

        public Shell(Point shell, Direction direction) : base(shell)
        {
            GameObjectSize.X = 5;
            GameObjectSize.Y = GameObjectSize.X;
            Speed = 3;
            Hp = 1;
            ObjectType = ObjectType.Shell;
            ObjectDirection = direction;
            myshellsBrush = new SolidBrush(Color.Orange);
            enemyshellsBrush = new SolidBrush(Color.Fuchsia);
        }

        public override void Move()
        {
            if (ObjectDirection == Direction.Right)
                GameObjectPoint.X += Speed;
            else
                GameObjectPoint.X -= Speed;
        }

        public override void Draw(Graphics graphics)
        {
            switch (ObjectDirection)
            {
                case Direction.Right:
                    graphics.FillRectangle(myshellsBrush, GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y);
                    break;
                default:
                    graphics.FillRectangle(enemyshellsBrush, GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y);
                    break;
            }
        }
    }
}


