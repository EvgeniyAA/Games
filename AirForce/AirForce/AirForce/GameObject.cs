﻿using System.Drawing;
using AirForce.Properties;

namespace AirForce
{

    public abstract class GameObject
    {
        public  static readonly Bitmap ExplosionImage = Resources.Explosion;
        public int Speed;
        public int Hp;
        public Direction ObjectDirection;
        public ObjectType ObjectType;
        public Point GameObjectPoint;
        public Point GameObjectSize;
        public int DodgeCoord;
        public bool IsNeededToCreateShell = false;

        protected GameObject(Point gameObjectPoint)
        {
            GameObjectPoint = gameObjectPoint;
        }

        protected GameObject()
        {
        }

        public virtual Direction GetDirection()
        {
            return ObjectDirection;
        }

        public virtual void Move()
        {
            GameObjectPoint.X -= Speed;
        }

        public void TakeDamage()
        {
            Hp--;
        }

        public abstract void Draw(Graphics graphics);
    }
}
