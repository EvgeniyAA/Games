using System.Collections.Generic;
using System.Drawing;

namespace AirForce
{
    public class Shell : GameObject
    {
        private static readonly SolidBrush MyshellsBrush = new SolidBrush(Color.Orange);
        private static readonly SolidBrush EnemyshellsBrush = new SolidBrush(Color.Fuchsia);

        public Shell(Point shell, ObjectType objectType) : base(shell)
        {
            GameObjectSize.X = 7;
            GameObjectSize.Y = GameObjectSize.X;
            Speed = 7;
            Hp = 1;
            ObjectType = objectType;
        }

        public override void Move(List<GameObject> objects)
        {
            if (ObjectType==ObjectType.MyShell)
                GameObjectPoint.X += Speed;
            else
                GameObjectPoint.X -= Speed;
        }

        public override void Draw(Graphics graphics)
        {
            switch (ObjectType)
            {
                case ObjectType.MyShell:
                    graphics.FillRectangle(MyshellsBrush, GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y);
                    break;
                default:
                    graphics.FillRectangle(EnemyshellsBrush, GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y);
                    break;
            }
        }
    }
}


