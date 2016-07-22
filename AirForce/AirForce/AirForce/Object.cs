using System.Drawing;

namespace AirForce
{
    public abstract class Object
    {
        public int PlaneX1;
        public int PlaneY1;
        public int PlaneWidth;
        public int PlaneHeight;
        public int Speed;
        public int Hp;
        public Direction ObjectDirection;
        protected Object(int planeX1, int planeY1)
        {
            PlaneX1 = planeX1;
            PlaneY1 = planeY1;
        }

        protected Object()
        {
        }

        public virtual Direction GetDirection()
        {
            return ObjectDirection=Direction.None;
        }
        public virtual void Move()
        {
            PlaneX1 -= Speed;
        }

        public void TakeDamage()
        {
            Hp--;
        }

        public abstract void Draw(Graphics graphics);

    }
}
