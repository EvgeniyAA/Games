﻿using System.Drawing;

namespace AirForce
{
    public class HeavyPlane : Object
    {
        private readonly SolidBrush enemyHeavyPlaneBrush ;
        private readonly Image myHeavyPlaneImage = Properties.Resources.HeavyPlane3;
        public HeavyPlane(int planeX1, int planeY1) : base(planeX1,planeY1)
        {
            PlaneWidth = 60;
            PlaneHeight = 30;
            Speed = 1;
            Hp = 10;
            enemyHeavyPlaneBrush = new SolidBrush(Color.HotPink);
        }

        //public override void Move()
        //{
        //    PlaneX1 -= Speed;
        //}

        public void Attack()
        {
            //if()
        }

        public override void Draw(Graphics graphics)
        {
            //graphics.FillRectangle(enemyHeavyPlaneBrush, PlaneX1, PlaneY1, PlaneWidth, PlaneHeight);
            graphics.DrawImageUnscaledAndClipped(myHeavyPlaneImage, new Rectangle(PlaneX1, PlaneY1, PlaneWidth, PlaneHeight));
        }
    }
}