using System;
using System.Collections.Generic;
using System.Drawing;

namespace AirForce
{
    public class HeavyPlane : GameObject
    {
        private static readonly Bitmap MyHeavyPlaneImage = Properties.Resources.HeavyPlane3;
        private static int shellFrequency;
        public HeavyPlane(Point heavyPlane) : base(heavyPlane)
        {
            GameObjectSize.X = 60;
            GameObjectSize.Y = 30;
            Speed = 3;
            Hp = 4;
            ObjectType = ObjectType.EnemyPlane;
        }

        public override void Move(List<GameObject> objects)
        {
            shellFrequency++;
            base.Move(objects);
            AttackIfEnemyPlaneInLineWithMyPlane(objects);
        }
        private void AttackIfEnemyPlaneInLineWithMyPlane(List<GameObject> objects)
        {
            List<GameObject> shellsToAdd = new List<GameObject>();

            foreach (GameObject gameObject in objects)
            {
                if (gameObject is HeavyPlane)
                {
                    if (CheckIsPlaneInLineWithSomeObject(gameObject, objects[0]) && shellFrequency % 100 == 0)
                        shellsToAdd.Add(
                            new Shell(
                                new Point(gameObject.GameObjectPoint.X - 1,
                                    gameObject.GameObjectPoint.Y + gameObject.GameObjectSize.Y / 2), ObjectType.EnemyShell));
                }
            }
            objects.AddRange(shellsToAdd);
            shellsToAdd.Clear();
        }
        public override void Draw(Graphics graphics)
        {
            graphics.DrawImage(MyHeavyPlaneImage, new Rectangle(GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y));
        }
    }
}
