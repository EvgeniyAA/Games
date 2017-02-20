using System;
using System.Drawing;

namespace StickHero
{
    public class Segment
    {
        public Point PointA;
        public Point PointB;

        public double Length;
        public int Angle;

        //  рутим по часовой стрелке относительно точки PointA
        public void Rotate(int angle)
        {
            Angle -= angle;

            PointB.X = (int)(Length * Math.Cos(Math.PI / 180 * Angle) + PointA.X);
            PointB.Y = (int)(PointA.Y - Length * Math.Sin(Math.PI / 180 * Angle));
        }

        // “очка PointA стоит на месте, а увеличиваем длину в сторону PointB
        public void Grow(int animationSpeed)
        {
            PointB.Y -= animationSpeed;
            Length = Math.Sqrt(Math.Pow(PointB.Y - PointA.Y, 2) +
                               Math.Pow(PointB.X - PointA.X, 2));
        }
    }
}