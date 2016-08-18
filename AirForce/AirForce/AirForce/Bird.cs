using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirForce
{
    class Bird : GameObject
    {
        private static readonly Bitmap BirdImage= Properties.Resources.CrazyBird;
        private static readonly Random Rnd = new Random();
        private int amplitude = 5;
        public Bird(int pictureBoxWidth,int pictureBoxHeight)
        {
            GameObjectPoint.X = pictureBoxWidth;
            GameObjectPoint.Y = Rnd.Next(pictureBoxHeight - pictureBoxHeight/5, pictureBoxHeight - GameObjectSize.Y);
            GameObjectSize.X = 50;
            GameObjectSize.Y = 28;
            Speed = 2;
            Hp = 1;
            ObjectType = ObjectType.Bird;
            ObjectDirection=Direction.Down;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawImage(BirdImage, new Rectangle(GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y));
        }

        public override void Move()
        {
            base.Move();
            Fly();
        }

        private void Fly()
        {
            if (amplitude != 5 && ObjectDirection == Direction.Up)
            {
                amplitude++;
                GameObjectPoint.Y--;
                if(amplitude==5)
                    ObjectDirection=Direction.Down;
            }
            if (amplitude != 0 && ObjectDirection == Direction.Down)
            {
                amplitude--;
                GameObjectPoint.Y++;
                if (amplitude == 0)
                    ObjectDirection = Direction.Up;
            }
        }
    }
}
