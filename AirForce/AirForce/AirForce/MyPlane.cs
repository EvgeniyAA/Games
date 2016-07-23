using System.Drawing;

namespace AirForce
{
    public class MyPlane : GameObject
    {
        private readonly int pictureBoxHeight;
        private static readonly Image MyPlaneImage = Properties.Resources.myPlane;
        public MyPlane(int objectX1, int objectY1, int pictureBoxHeight) : base(objectX1, objectY1)
        {
            this.pictureBoxHeight = pictureBoxHeight;
            ObjectWidth = 50;
            ObjectHeight = 40;
            Speed = 5;
            Hp = 10;
            ObjectDirection=Direction.None;
            ObjectType=ObjectType.MyPlane;
        }

        public override void Move()
        {
            if ((ObjectDirection == Direction.Up) && (ObjectY1 - Speed > 0))
                ObjectY1 -= Speed;

            if ((ObjectDirection == Direction.Down) && (ObjectY1 + Speed + ObjectHeight < pictureBoxHeight))
                ObjectY1 += Speed;
        }
        //public override Direction GetDirection()
        //{
        //    return ObjectDirection = Direction.Right;
        //}
        public override void Draw(Graphics graphics)
        {
            graphics.DrawImageUnscaledAndClipped(MyPlaneImage,new Rectangle(ObjectX1, ObjectY1, ObjectWidth, ObjectHeight));
        }
    }
}