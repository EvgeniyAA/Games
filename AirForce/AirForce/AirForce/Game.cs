using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AirForce
{
    internal class Game
    {
        private const int EarthHeight = 100;
        private readonly int width;
        private readonly int height;
        private static readonly Random Rnd = new Random();
        public List<GameObject> Objects;
        public int Score;
        private bool isPlayerShooting;
        private int shellFrequency;
        public Level GameLevel;
        private static readonly Dictionary<ObjectType, ObjectType[]> CollisionTypes= new Dictionary<ObjectType, ObjectType[]>
        {
            { ObjectType.Earth,      new [] {ObjectType.EnemyPlane, ObjectType.EnemyShell, ObjectType.Meteor,ObjectType.MyPlane, ObjectType.MyShell }},
            { ObjectType.MyPlane,    new [] { ObjectType.EnemyPlane, ObjectType.EnemyShell, ObjectType.Bird, ObjectType.Meteor } },
            { ObjectType.Meteor,     new [] { ObjectType.EnemyPlane, ObjectType.MyShell,ObjectType.EnemyShell  } },
            { ObjectType.MyShell,    new [] { ObjectType.EnemyPlane } },
            { ObjectType.EnemyShell, new      ObjectType[] {  } },            
            { ObjectType.Bird,       new      ObjectType[] {  } },
            { ObjectType.EnemyPlane, new      ObjectType[] {  } },
            
        };

        public Game(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public bool IsGameOver()
        {
            return (Objects[0].ObjectType!=ObjectType.MyPlane);
        }

        public void Restart()
        {
            if (Objects != null)
                Objects.Clear();
            Objects = new List<GameObject> {new MyPlane(new Point(width/22, height/2), height)};
            Objects.Add(new Earth(height,width, EarthHeight));
            isPlayerShooting = false;
            shellFrequency = 0;
            GameLevel = new Level();
        }

        public void Draw(Graphics graphics)
        {
            foreach (GameObject Object in Objects)
                Object.Draw(graphics);
        }

        public void PressUp()
        {
            Objects[0].ObjectDirection = Direction.Up;
        }

        public void PressDown()
        {
            Objects[0].ObjectDirection = Direction.Down;
        }

        public void UnPress()
        {
            Objects[0].ObjectDirection = Direction.None;
        }

        private void PlayerStartShoot()
        {
            shellFrequency++;
            if (isPlayerShooting && shellFrequency%10 == 0)
                CreateShell(ObjectType.MyShell, Objects[0]);
        }

        public void StopAttack()
        {
            isPlayerShooting = false;
        }

        public void StartAttack()
        {
            isPlayerShooting = true;
        }

        public void Update(int countOfTicks)
        {
            for (int i = 0; i < Objects.Count; i++)
                Objects[i].Move(Objects);
            if (countOfTicks%GameLevel.Frequency == 0)
                CreateEnemy();
            DamageObjects();
            DeleteWithoutHpOrIfOutside();
            PlayerStartShoot();
        }

        private void CreateShell(ObjectType objectType, GameObject plane)
        {
            int shellX = 0;
            if (objectType == ObjectType.MyShell)
                shellX = plane.GameObjectPoint.X + plane.GameObjectSize.X + 1;
            if (objectType == ObjectType.EnemyShell && plane.IsNeededToCreateShell)
                shellX = plane.GameObjectPoint.X - 1;
            int shellY = plane.GameObjectPoint.Y + plane.GameObjectSize.Y/2;
            Objects.Add(new Shell(new Point(shellX, shellY), objectType));
        }

        private void CreateEnemy()
        {
            //Test Parameter
            int planeTypeNumber = Rnd.Next(4);
            //Release Parameter
            //int planeTypeNumber = Rnd.Next(GameLevel.PlaneTypesOnLevel);
            int planeHeightCoord = Rnd.Next(Objects[0].GameObjectSize.Y, height - Objects[0].GameObjectSize.Y);
            switch (planeTypeNumber)
            {
                case 0:
                    Objects.Add(new HeavyPlane(new Point(width, planeHeightCoord)));
                    break;
                case 1:
                    Objects.Add(new Fighter(new Point(width, planeHeightCoord),height));
                    break;
                case 2:
                    Objects.Add(new Meteor(width));
                    break;
                case 3:
                    Objects.Add(new Bird(width,height, EarthHeight));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CheckCollision(GameObject checkingObject1, GameObject checkingObject2)
        {
            return CheckIntersect(checkingObject1, checkingObject2) &&
                   CheckObjectTypes(checkingObject1.ObjectType, checkingObject2.ObjectType);
        }

        private bool CheckIntersect(GameObject checkingObject1, GameObject checkingObject2)
        {
            return Rectangle.Intersect(
                new Rectangle(checkingObject1.GameObjectPoint.X, checkingObject1.GameObjectPoint.Y,
                    checkingObject1.GameObjectSize.X,
                    checkingObject1.GameObjectSize.Y),
                new Rectangle(checkingObject2.GameObjectPoint.X, checkingObject2.GameObjectPoint.Y,
                    checkingObject2.GameObjectSize.X,
                    checkingObject2.GameObjectSize.Y)) != Rectangle.Empty;
        }

        private static bool CheckObjectTypes(ObjectType typeA, ObjectType typeB)
        {
            return CollisionTypes[typeA].Contains(typeB);
        }

        private void IncreaseScore() 
        {
            Score += Objects.Count(Object => (Object.Hp <= 0 && Object.ObjectType == ObjectType.EnemyPlane));
            GameLevel.Killed += Objects.Count(Object => (Object.Hp <= 0 && Object.ObjectType == GameLevel.TypeToKill));
            if (GameLevel.CountToKill==GameLevel.Killed)
            {
                GameLevel.UpGameLevel();
                Score = 0;
            }
        }

        public void DeleteWithoutHpOrIfOutside()
        {
            IncreaseScore();
            Objects.RemoveAll(shell => (shell.ObjectType == ObjectType.MyShell|| shell.ObjectType==ObjectType.EnemyShell) &&
                    (shell.GameObjectPoint.X >= width - shell.GameObjectSize.X || shell.GameObjectPoint.X <= 0));
            Objects.RemoveAll(plane => plane.Hp <= 0 || plane.GameObjectPoint.X + plane.GameObjectSize.X <= 0 ||
                    plane.GameObjectPoint.Y + plane.GameObjectSize.Y >= height);
        }

        private void DamageObjects()
        {
            foreach (GameObject checkingObject1 in Objects)
            {
                foreach (GameObject checkingObject2 in Objects)
                {
                    if (CheckCollision(checkingObject1, checkingObject2))
                    {
                        if (checkingObject1.ObjectType == ObjectType.MyPlane ||
                            (checkingObject1.ObjectType == ObjectType.Meteor && checkingObject2.ObjectType != ObjectType.MyPlane))
                        {
                            checkingObject2.Hp = 0;
                            checkingObject1.TakeDamage();
                        }
                        if (checkingObject2.ObjectType == ObjectType.Earth)
                            checkingObject1.Hp = 0;
                        else
                        {
                            checkingObject1.TakeDamage();
                            checkingObject2.TakeDamage();
                        }
                    }
                }
            }
        }
    }
}