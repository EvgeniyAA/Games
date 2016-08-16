using System.Drawing;

namespace AirForce
{
    public class MyPlane : GameObject
    {
        private readonly int pictureBoxHeight;
        private static readonly Bitmap MyPlaneImage = Properties.Resources.myPlane1;

        public MyPlane(Point myPlanePoint, int pictureBoxHeight) : base(myPlanePoint)
        {
            this.pictureBoxHeight = pictureBoxHeight;
            GameObjectSize.X = 50;
            GameObjectSize.Y = 40;
            Speed = 5;
            Hp = 20;
            ObjectDirection = Direction.None;
            ObjectType = ObjectType.MyPlane;
        }

        public MyPlane(Point myPlanePoint) : this(myPlanePoint, 1)
        {
        }

        public override void Move()
        {
            if ((ObjectDirection == Direction.Up) && (GameObjectPoint.Y - Speed > 0))
                GameObjectPoint.Y -= Speed;

            if ((ObjectDirection == Direction.Down) && (GameObjectPoint.Y + Speed + GameObjectSize.Y < pictureBoxHeight))
                GameObjectPoint.Y += Speed;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawImage(MyPlaneImage,new Rectangle(GameObjectPoint.X,GameObjectPoint.Y,GameObjectSize.X,GameObjectSize.Y));
        }

    }
}