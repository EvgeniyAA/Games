using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirForce
{
    class Meteor : GameObject
    {
        private static readonly Bitmap MeteorImage = Properties.Resources.meteor;
        private static readonly Random Rnd = new Random();
        private bool currentlyAnimating = false;
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

        //public void AnimateImage()
        //{
        //    if (!currentlyAnimating)
        //    {
        //        ImageAnimator.Animate(MeteorImage,new EventHandler(this.OnFrameChanged));
        //        currentlyAnimating = true;
        //    }
        //}

        //private void OnFrameChanged(object sender, EventArgs e)
        //{
            
        //    this.Invalidate();
        //}

        public override void Draw(Graphics graphics)
        {
            //AnimateImage();
            //ImageAnimator.UpdateFrames();
            graphics.DrawImage(MeteorImage,
                new Rectangle(GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y));
        }
    }
}
