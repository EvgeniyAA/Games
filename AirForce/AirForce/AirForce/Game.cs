using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AirForce
{
    internal class Game
    {
        private readonly int width;
        private readonly int height;
        private static readonly Random Rnd = new Random();
        public List<GameObject> Objects;
        public int Score;
        private bool isPlayerShootint;
        private int shellFrequency;
        public Level GameLevel;
        private static readonly Dictionary<ObjectType, ObjectType[]> CollisionTypes= new Dictionary<ObjectType, ObjectType[]>()
        {
            { ObjectType.MyPlane,    new [] { ObjectType.EnemyPlane, ObjectType.EnemyShell, ObjectType.Bird, ObjectType.Meteor } },
            { ObjectType.MyShell,      new [] { ObjectType.EnemyPlane, ObjectType.Meteor  } },
            { ObjectType.EnemyShell,      new [] { ObjectType.MyPlane, ObjectType.Meteor  } },
            { ObjectType.Meteor,     new [] { ObjectType.EnemyPlane, ObjectType.MyPlane, ObjectType.MyShell,ObjectType.EnemyShell,  } },
            { ObjectType.Bird,     new [] { ObjectType.MyPlane } },
            { ObjectType.EnemyPlane,     new [] { ObjectType.MyPlane,ObjectType.MyShell, ObjectType.Meteor } }
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
            isPlayerShootint = false;
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
            if (isPlayerShootint && shellFrequency%10 == 0)
                CreateShell(Direction.Right, Objects[0]);
        }

        public void StopAttack()
        {
            isPlayerShootint = false;
        }

        public void StartAttack()
        {
            isPlayerShootint = true;
        }

        public void Update(int countOfTicks)
        {
            foreach (GameObject Object in Objects)
                Object.Move();
            if (countOfTicks%GameLevel.Frequency == 0)
                CreateEnemy();
            PlayerStartShoot();
            AttackIfEnemyPlaneInLineWithMyPlane();
            EnableDodgeIfPlaneInLineWithShell();
            DamageObjects();
            DeleteWithoutHpOrIfOutside();
        }

        public void CreateShell(Direction direction, GameObject plane)
        {
            int shellX = 0;
            if (direction == Direction.Right)
                shellX = plane.GameObjectPoint.X + plane.GameObjectSize.X + 1;
            if (direction == Direction.Left && plane.IsNeededToCreateShell)
                shellX = plane.GameObjectPoint.X - 1;
            int shellY = plane.GameObjectPoint.Y + plane.GameObjectSize.Y/2;
            Objects.Add(new Shell(new Point(shellX, shellY), direction));
        }

        public void CreateEnemy()
        {
            //Test Parameter
            //int planeTypeNumber = Rnd.Next(4);
            //Release Parameter
            int planeTypeNumber = Rnd.Next(GameLevel.PlaneTypesOnLevel);
            int planeHeightCoord = Rnd.Next(Objects[0].GameObjectSize.Y, height - Objects[0].GameObjectSize.Y);
            switch (planeTypeNumber)
            {
                case 0:
                    Objects.Add(new HeavyPlane(new Point(width, planeHeightCoord)));
                    break;
                case 1:
                    Objects.Add(new Fighter(new Point(width, planeHeightCoord)));
                    break;
                case 2:
                    Objects.Add(new Meteor(width));
                    break;
                case 3:
                    Objects.Add(new Bird(width,height));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private int NewRandomCoordYForFighterPlane(GameObject gameObject)
        {
            int newObjectPoint = gameObject.GameObjectPoint.Y;
            while (newObjectPoint <= gameObject.GameObjectPoint.Y + gameObject.GameObjectSize.Y
                   && newObjectPoint >= gameObject.GameObjectPoint.Y)
            {
                newObjectPoint = Rnd.Next(Objects[0].GameObjectPoint.Y/2, height - gameObject.GameObjectSize.Y);
            }
            return newObjectPoint;
        }

        private bool CheckCollision(GameObject checkingObject1, GameObject checkingObject2)
        {
            return CheckIntersect(checkingObject1, checkingObject2) &&
                   CheckObjectTypes(checkingObject1.ObjectType, checkingObject2.ObjectType);
        }

        private static bool CheckIntersect(GameObject checkingObject1, GameObject checkingObject2)
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

        private void EnableDodgeIfPlaneInLineWithShell()
        {
            foreach (GameObject gameObject in Objects)
            {
                if (gameObject is Fighter)
                {
                    foreach (GameObject shell in Objects)
                    {
                        if (shell is Shell && CheckIsPlaneInLineWithSomeObject(gameObject, shell) &&
                            shell.ObjectDirection == Direction.Right && gameObject.GetDirection() == Direction.None)
                        {
                            gameObject.DodgeCoord = NewRandomCoordYForFighterPlane(gameObject);
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

        private bool CheckIsPlaneInLineWithSomeObject(GameObject checkingObject1, GameObject checkingObject2)
        {
            GameObject checkingGameObject2;
            if (checkingObject2 is Shell)
                checkingGameObject2 = new Shell(checkingObject2.GameObjectPoint, checkingObject2.GetDirection());
            else
                checkingGameObject2 = new MyPlane(checkingObject2.GameObjectPoint);
            checkingGameObject2.GameObjectPoint.X = checkingObject1.GameObjectPoint.X;
            return CheckIntersect(checkingObject1, checkingGameObject2);
        }

        private void AttackIfEnemyPlaneInLineWithMyPlane()
        {
            List<GameObject> shellsToAdd = new List<GameObject>();
            foreach (GameObject gameObject in Objects)
            {
                if (gameObject is HeavyPlane)
                    if (CheckIsPlaneInLineWithSomeObject(gameObject, Objects[0]) && shellFrequency%10 == 0)
                        shellsToAdd.Add(
                            new Shell(
                                new Point(gameObject.GameObjectPoint.X - 1,
                                    gameObject.GameObjectPoint.Y + gameObject.GameObjectSize.Y/2), Direction.Left));
            }
            Objects.AddRange(shellsToAdd);
            shellsToAdd.Clear();
        }
        public void IncreaseScore() 
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

        public void DamageObjects()
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                for (int j = i+1; j < Objects.Count-1; j++)
                {
                    if (CheckCollision(Objects[i], Objects[j]))
                    {
                        if (Objects[i].ObjectType == ObjectType.MyPlane ||
                            (Objects[j].ObjectType == ObjectType.Meteor && Objects[j].ObjectType != ObjectType.MyPlane))
                        {
                            Objects[j].Hp = 0;
                            Objects[i].TakeDamage();
                        }
                        else
                        {
                            Objects[i].TakeDamage();
                            Objects[j].TakeDamage();
                        }
                    }
                }
            }
        }
    }
}