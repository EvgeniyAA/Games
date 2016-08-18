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
        private static readonly SolidBrush MyshellsBrush = new SolidBrush(Color.Orange);
        private static readonly SolidBrush EnemyshellsBrush = new SolidBrush(Color.Fuchsia);

        public Shell(Point shell, Direction direction) : base(shell)
        {
            GameObjectSize.X = 5;
            GameObjectSize.Y = GameObjectSize.X;
            Speed = 3;
            Hp = 1;
            ObjectDirection = direction;
            ObjectType = direction==Direction.Right ? ObjectType.MyShell : ObjectType.EnemyShell;
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
                    graphics.FillRectangle(MyshellsBrush, GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y);
                    break;
                default:
                    graphics.FillRectangle(EnemyshellsBrush, GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y);
                    break;
            }
        }
    }
}


