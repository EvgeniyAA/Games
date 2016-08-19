using System.Collections.Generic;
using System.Drawing;
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
        public bool IsNeededToCreateShell;

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

        public virtual void Move(List<GameObject> objects)
        {
            GameObjectPoint.X -= Speed;
        }

        public void TakeDamage()
        {
            Hp--;
        }

        public abstract void Draw(Graphics graphics);

        protected bool CheckIsPlaneInLineWithSomeObject(GameObject checkingObject1, GameObject checkingObject2)
        {
            return  (checkingObject2.GameObjectPoint.Y <=
                     checkingObject1.GameObjectPoint.Y +  checkingObject1.GameObjectSize.Y &&
                     checkingObject2.GameObjectPoint.Y >= checkingObject1.GameObjectPoint.Y) ||
                    (checkingObject2.GameObjectPoint.Y +  checkingObject2.GameObjectSize.Y >=
                     checkingObject1.GameObjectPoint.Y &&
                     checkingObject2.GameObjectPoint.Y +  checkingObject2.GameObjectSize.Y <=
                     checkingObject1.GameObjectPoint.Y +  checkingObject1.GameObjectSize.Y);
        }
    }
}
