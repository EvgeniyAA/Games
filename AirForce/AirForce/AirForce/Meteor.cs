using System;
using System.Drawing;

namespace AirForce
{
    class Meteor : GameObject
    {
        private static readonly Bitmap MeteorImage = Properties.Resources.meteor;
        private static readonly Random Rnd = new Random();
        public Meteor(int pictureBoxWidth)
        {
            GameObjectSize.X = Rnd.Next(10,51);
            GameObjectSize.Y = GameObjectSize.X;
            Speed = 2;
            Hp = 5;
            ObjectType=ObjectType.Meteor;
            GameObjectPoint.X = Rnd.Next(pictureBoxWidth/4, pictureBoxWidth-pictureBoxWidth/4);
            GameObjectPoint.Y = 0;
        }

        public override void Move()
        {
            GameObjectPoint.Y += Speed;
            //if (GameObjectPoint.Y%2 == 0)
                GameObjectPoint.X -= Speed;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawImage(MeteorImage,
                new Rectangle(GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y));
        }
    }
}
