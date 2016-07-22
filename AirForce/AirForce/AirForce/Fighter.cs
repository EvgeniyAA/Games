using System.Drawing;

namespace AirForce
{
    public class Fighter : Object
    {
        private readonly SolidBrush enemyFighterBrush;
        public Fighter(int planeX1, int planeY1) : base(planeX1,planeY1)
        {
            PlaneWidth = 30;
            PlaneHeight = 20;
            Speed = 2;
            Hp = 5;
            enemyFighterBrush = new SolidBrush(Color.LimeGreen);
        }

        public override void Move(Direction direction)
        {
            base.Move(direction);
            Dodge(12);
        }

        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(enemyFighterBrush, PlaneX1, PlaneY1, PlaneWidth, PlaneHeight);
        }

        public void Dodge(int myPlaneCoordY)
        { 
            //if()
        }
    }
}
