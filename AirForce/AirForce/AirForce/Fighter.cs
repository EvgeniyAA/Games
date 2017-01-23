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
        private int NewRandomCoordYForFighterPlane(GameObject myPlane)
        {
            int newObjectPoint = GameObjectPoint.Y;
            while (newObjectPoint <= GameObjectPoint.Y + GameObjectSize.Y
                   && newObjectPoint >= GameObjectPoint.Y)
            {
                newObjectPoint = Rnd.Next(myPlane.GameObjectPoint.Y / 2, height - GameObjectSize.Y);
            }
            return newObjectPoint;
        }

        private void EnableDodgeIfPlaneInLineWithShell(List<GameObject> objects)
        {
                    foreach (GameObject shell in objects)
                    {
                        if (shell is Shell && CheckIsPlaneInLineWithSomeObject( shell) &&
                            shell.ObjectType == ObjectType.MyShell && GetDirection() == Direction.None)
                        {
                            DodgeCoord = NewRandomCoordYForFighterPlane(objects[0]);
                            ObjectDirection = GameObjectPoint.Y > DodgeCoord ? Direction.Up : Direction.Down;
                        }
                    }
                    if ((GetDirection() == Direction.Up &&
                         DodgeCoord >= GameObjectPoint.Y) ||
                        (GetDirection() == Direction.Down &&
                         DodgeCoord <= GameObjectPoint.Y))
                        ObjectDirection = Direction.None;
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
