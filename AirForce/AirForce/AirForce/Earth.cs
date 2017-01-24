using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirForce
{
    public class Earth : GameObject
    {
        private static readonly SolidBrush EarthBrush = new SolidBrush(Color.SaddleBrown);

        public Earth(int pictureBoxHeight, int pictureBoxWidth, int height)
        { 
            GameObjectPoint.X =1;
            GameObjectPoint.Y = pictureBoxHeight - height-1;
            GameObjectSize.X = pictureBoxWidth-1;
            GameObjectSize.Y = height-1;
            Hp = 190000;
            ObjectType = ObjectType.Earth;
        }
        public override void Draw(Graphics graphics)
        {
            graphics.FillRectangle(EarthBrush,new Rectangle(GameObjectPoint.X,GameObjectPoint.Y,GameObjectSize.X,GameObjectSize.Y));
        }
    }
}
