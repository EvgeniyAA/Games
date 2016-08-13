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
            if (countOfTicks%20 == 0)
                CreateEnemyPlane();
            PlayerStartShoot();
            AttackIfEnemyPlaneInLineWithMyPlane();
            EnableDodgeIfPlaneInLineWithShell();
            DamageObjects();
            DeleteObjectsIfCollisionWithMyPlane();
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

        public void CreateEnemyPlane()
        {
            int planeTypeNumber = Rnd.Next(3);
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
                   CheckObjectTypes(checkingObject1, checkingObject2);
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

        private static bool CheckObjectTypes(GameObject checkingObject1, GameObject checkingObject2)
        {
            if (checkingObject1.ObjectType == ObjectType.MyPlane && (checkingObject2.ObjectType == ObjectType.EnemyPlane ||
                checkingObject2.ObjectType == ObjectType.Shell || checkingObject2.ObjectType == ObjectType.Bird ||
                checkingObject2.ObjectType == ObjectType.Meteor || checkingObject2.ObjectType == ObjectType.Earth))
                return true;
            else if (checkingObject1.ObjectType == ObjectType.Shell && checkingObject1.ObjectDirection == Direction.Right &&
                     (checkingObject2.ObjectType == ObjectType.EnemyPlane ||
                     checkingObject2.ObjectType == ObjectType.Meteor))
                return true;
            else if (checkingObject1.ObjectType == ObjectType.Shell &&
                     checkingObject1.ObjectDirection == Direction.Left &&
                     (checkingObject2.ObjectType == ObjectType.MyPlane ||
                     checkingObject2.ObjectType == ObjectType.Meteor))
                return true;
            else if (checkingObject1.ObjectType == ObjectType.EnemyPlane &&
                     (checkingObject2.ObjectType == ObjectType.Meteor ||
                     checkingObject2.ObjectType == ObjectType.Earth))
                return true;
            return false;
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
                            if (gameObject.GameObjectPoint.Y > gameObject.DodgeCoord)
                                gameObject.ObjectDirection = Direction.Up;
                            else gameObject.ObjectDirection = Direction.Down;
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

        private void DeleteObjectsIfCollisionWithMyPlane()
        {
            Objects.RemoveAll(plane => CheckCollision(Objects[0], plane));
        }

        public void IncreaseScore()
        {
            Score += Objects.Count(Object => (Object.Hp <= 0 && Object.ObjectType == ObjectType.EnemyPlane));
        }

        public void DeleteWithoutHpOrIfOutside()
        {
            IncreaseScore();
            Objects.RemoveAll(shell => shell.ObjectType == ObjectType.Shell &&
                    (shell.GameObjectPoint.X >= width - shell.GameObjectSize.X || shell.GameObjectPoint.X <= 0));
            Objects.RemoveAll(plane => plane.Hp <= 0 || plane.GameObjectPoint.X + plane.GameObjectSize.X <= 0 ||
                    plane.GameObjectPoint.Y + plane.GameObjectSize.Y >= height);
        }

        public void DamageObjects()
        {
            foreach (GameObject gameObject1 in Objects)
            {
                foreach (GameObject gameObject2 in Objects)
                    if (CheckCollision(gameObject1, gameObject2))
                    {
                        if (gameObject1.ObjectType != ObjectType.MyPlane && gameObject2.ObjectType == ObjectType.Meteor)
                        {
                            gameObject1.Hp = 0;
                            gameObject2.TakeDamage();
                        }
                        else
                        {
                            gameObject2.TakeDamage();
                            gameObject1.TakeDamage();
                        }
                    }
            }
        }
    }
}