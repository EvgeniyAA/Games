using System.Drawing;

namespace AirForce
{
    public class Shell
    {
        public Direction Direction; // ?
        public readonly int Speed;
        public readonly int Size;
        public int X;
        public readonly int Y;
        private readonly SolidBrush myshellsBrush;
        private readonly SolidBrush enemyshellsBrush;
        public Shell(Direction direction, int speed, int size, int x, int y)
        {
            Direction = direction;
            Speed = speed;
            Size = size;
            X = x;
            Y = y;
            myshellsBrush = new SolidBrush(Color.Orange);
            enemyshellsBrush = new SolidBrush(Color.Fuchsia);
        }

        public void ShellsMoving()
        {            
            if (Direction == Direction.Right)
                X += Speed;
            else
                X -= Speed;
        }

        public void Draw(Graphics graphics)
        {
            switch (Direction)
            {
                case Direction.Right:
                    graphics.FillRectangle(myshellsBrush, X, Y, Size, Size);
                    break;
                default:
                    graphics.FillRectangle(enemyshellsBrush, X, Y, Size, Size);
                    break;
            }
        }
    }
}