using System;
using System.Collections.Generic;
using System.Drawing;

namespace AirForce
{
    class Meteor : GameObject
    {
        private static readonly Bitmap MeteorImage = Properties.Resources.meteor;
        private static readonly Random Rnd = new Random();
        public Meteor(int pictureBoxWidth)
        {
            GameObjectSize.X = Rnd.Next(50,201);
            GameObjectSize.Y = GameObjectSize.X;
            Speed = 5;
            Hp = 5;
            ObjectType=ObjectType.Meteor;
            GameObjectPoint.X = Rnd.Next(pictureBoxWidth/4, pictureBoxWidth-pictureBoxWidth/4);
            GameObjectPoint.Y = -GameObjectSize.Y;
        }

        public override void Move(List<GameObject> objects)
        {
            GameObjectPoint.Y += Speed;
            GameObjectPoint.X -= Speed;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawImage(MeteorImage,
                new Rectangle(GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y));
        }
    }
}
