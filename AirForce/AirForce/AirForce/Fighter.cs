using System.Drawing;

namespace AirForce
{
    public class Fighter : GameObject
    {
        private readonly SolidBrush enemyFighterBrush;
        public Fighter(int objectX1, int objectY1) : base(objectX1,objectY1)
        {
            ObjectWidth = 30;
            ObjectHeight = 20;
            Speed = 2;
            Hp = 5;
            enemyFighterBrush = new SolidBrush(Color.LimeGreen);
            ObjectType=ObjectType.EnemyPlane;
        }

        public override void Move()
        {
            base.Move();
            Dodge(12);
        }

        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(enemyFighterBrush, ObjectX1, ObjectY1, ObjectWidth, ObjectHeight);
        }

        public void Dodge(int myPlaneCoordY)
        { 
            //if()
        }
    }
}
