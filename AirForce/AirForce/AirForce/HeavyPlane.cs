using System.Drawing;

namespace AirForce
{
    public class HeavyPlane : GameObject
    {
        private static readonly Image MyHeavyPlaneImage = Properties.Resources.HeavyPlane3;
        public HeavyPlane(int objectX1, int objectY1) : base(objectX1,objectY1)
        {
            ObjectWidth = 60;
            ObjectHeight = 30;
            Speed = 1;
            Hp = 10;
            ObjectType=ObjectType.EnemyPlane;
        }

        //public override void Move()
        //{
        //    objectX1 -= Speed;
        //}

        public void Attack()
        {
            //if()
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawImageUnscaledAndClipped(MyHeavyPlaneImage, new Rectangle(ObjectX1, ObjectY1, ObjectWidth, ObjectHeight));
        }
    }
}
