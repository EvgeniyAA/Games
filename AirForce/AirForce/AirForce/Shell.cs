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

        public Shell(int objectX1, int objectY1, Direction direction) : base(objectX1, objectY1)
        {
            ObjectType= ObjectType.Shell;
            ObjectDirection = direction;
            Speed = 3;
            ObjectWidth = 5;
            ObjectHeight = ObjectWidth;
            Hp=1;
            myshellsBrush = new SolidBrush(Color.Orange);
            enemyshellsBrush = new SolidBrush(Color.Fuchsia);
        }


        public override void Move()
        {
            if (ObjectDirection == Direction.Right)
                ObjectX1 += Speed;
            else
                ObjectY1 -= Speed;
        }

        public override void Draw(Graphics graphics)
        {
            switch (ObjectDirection)
            {
                case Direction.Right:
                    graphics.FillRectangle(myshellsBrush, ObjectX1, ObjectY1, ObjectWidth, ObjectHeight);
                    break;
                default:
                    graphics.FillRectangle(enemyshellsBrush, ObjectX1, ObjectY1, ObjectWidth, ObjectHeight);
                    break;
            }
        }
    }
}


