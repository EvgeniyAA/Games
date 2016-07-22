using System.Drawing;

namespace AirForce
{
    public class MyPlane:Object
    {
        private readonly int pictureBoxHeight;
        private readonly SolidBrush planeBrush;
        //private readonly Bitmap image;
        public MyPlane(int planeX1, int planeY1, int pictureBoxHeight) : base(planeX1, planeY1)
        {
            this.pictureBoxHeight = pictureBoxHeight;
            PlaneWidth = 50;
            PlaneHeight = 40;
            Speed = 5;
            Hp = 10;
            planeBrush = new SolidBrush(Color.Blue);
            //image= new Bitmap(@"C:\Users\Asus\Desktop\Coding\Games\trunk\AirForce\AirForce\AirForce.myPlane.png");
            //image=new Bitmap(planeX1,planeY1);
        }

        public override void Move(Direction direction)
        {
            if ((direction == Direction.Up) && (PlaneY1 - Speed > 0))
                PlaneY1 -= Speed;

            if ((direction == Direction.Down) && (PlaneY1 + Speed + PlaneHeight < pictureBoxHeight))
                PlaneY1 += Speed;
        }
        //public override Direction GetDirection()
        //{
        //    return ObjectDirection = Direction.Right;
        //}
        public override void Draw(Graphics graphics)
        {
            //Graphics.FromImage(image);
            
            //graphics.DrawImage(image,new Point(PlaneX1,PlaneY1));
            graphics.FillRectangle(planeBrush, PlaneX1, PlaneY1, PlaneWidth, PlaneHeight);
        }
    }
}