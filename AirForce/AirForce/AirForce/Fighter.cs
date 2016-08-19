using System;
using System.Collections.Generic;
using System.Drawing;

namespace AirForce
{
    public class Fighter : GameObject
    {
        private static readonly Bitmap FighterImage = Properties.Resources.Fighter;
        private static readonly Random Rnd = new Random();
        private readonly int height;
        public Fighter(Point fighterPoint, int pictureBoxHeight) : base(fighterPoint)
        {
            GameObjectSize.X = 30;
            GameObjectSize.Y = 20;
            Speed = 4;
            Hp = 2;
            ObjectType = ObjectType.EnemyPlane;
            ObjectDirection = Direction.None;
            height = pictureBoxHeight;
        }        

        public override void Draw(Graphics graphics)
        {
            if (Hp <= 0)
                graphics.DrawImage(ExplosionImage, new Rectangle(GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y));
            else
                graphics.DrawImage(FighterImage, new Rectangle(GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y));
        }

        public override void Move(List<GameObject> objects)
        {
            base.Move(objects);
            EnableDodgeIfPlaneInLineWithShell(objects);
            Dodge();
        }
        private int NewRandomCoordYForFighterPlane(GameObject gameObject, GameObject myPlane)
        {
            int newObjectPoint = gameObject.GameObjectPoint.Y;
            while (newObjectPoint <= gameObject.GameObjectPoint.Y + gameObject.GameObjectSize.Y
                   && newObjectPoint >= gameObject.GameObjectPoint.Y)
            {
                newObjectPoint = Rnd.Next(myPlane.GameObjectPoint.Y / 2, height - gameObject.GameObjectSize.Y);
            }
            return newObjectPoint;
        }

        private void EnableDodgeIfPlaneInLineWithShell(List<GameObject> objects)
        {
            foreach (GameObject gameObject in objects)
            {
                if (gameObject is Fighter)
                {
                    foreach (GameObject shell in objects)
                    {
                        if (shell is Shell && CheckIsPlaneInLineWithSomeObject(gameObject, shell) &&
                            shell.ObjectType == ObjectType.MyShell && gameObject.GetDirection() == Direction.None)
                        {
                            gameObject.DodgeCoord = NewRandomCoordYForFighterPlane(gameObject,objects[0]);
                            gameObject.ObjectDirection = gameObject.GameObjectPoint.Y > gameObject.DodgeCoord ? Direction.Up : Direction.Down;
                        }
                    }
                    if ((gameObject.GetDirection() == Direction.Up &&
                         gameObject.DodgeCoord >= gameObject.GameObjectPoint.Y) ||
                        (gameObject.GetDirection() == Direction.Down &&
                         gameObject.DodgeCoord <= gameObject.GameObjectPoint.Y))
                        gameObject.ObjectDirection = Direction.None;
                }
            }
        }

        public void Dodge()
        {
            if (ObjectDirection == Direction.Up)
                GameObjectPoint.Y--;
            if (ObjectDirection == Direction.Down)
                GameObjectPoint.Y++;
        }
    }
}
