using System;
using System.Drawing;

namespace AirForce
{
    public class HeavyPlane : GameObject
    {
        private static readonly Image MyHeavyPlaneImage = Properties.Resources.HeavyPlane31;

        public HeavyPlane(Point heavyPlane) : base(heavyPlane)
        {
            GameObjectSize.X = 60;
            GameObjectSize.Y = 30;
            Speed = 1;
            Hp = 10;
            ObjectType = ObjectType.EnemyPlane;
        }
        //public override void Move()
        //{
        //    objectX1 -= Speed;
        //}

        public void Attack()
        {
            //if()
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawImageUnscaledAndClipped(MyHeavyPlaneImage, new Rectangle(GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y));
        }
    }
}
