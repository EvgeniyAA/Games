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
        public readonly List<GameObject> objects;
        public int Score;
        private bool isPlayerShootint;
        private int shellFrequency;

        public Game(int width, int height)
        {
            this.width = width;
            this.height = height;
            objects= new List<GameObject> {new MyPlane(new Point(width/22,height/2),height)};
        }

        public void Draw(Graphics graphics)
        {
            foreach (GameObject Object in objects)
                Object.Draw(graphics);
        }

        public void PressUp()
        {
            objects[0].ObjectDirection=Direction.Up;
        }
        public void PressDown()
        {
            objects[0].ObjectDirection = Direction.Down;
        }
        public void UnPress()
        {
            objects[0].ObjectDirection = Direction.None;
        }

        private void PlayerStartShoot()
        {
            shellFrequency++;   
            if (isPlayerShootint&&shellFrequency%10==0)
                CreateShell(Direction.Right, objects[0]);       
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
            foreach (GameObject Object in objects)
            {
                Object.Move();
            }
                
            if(countOfTicks%20==0)
                CreateEnemyPlane();
            PlayerStartShoot();
            AttackIfPlaneInLineWithShell();
            EnableDodgeIfPlaneInLineWithShell();
            DeleteShells();
            DamageMyPlaneIfCollision();
            DeletePlanes();            
        }
        public bool IsGameOver()
        {
            return objects[0].Hp <= 0;
        }

        public void DeletePlanes()
        {
            foreach (GameObject plane in objects)
                if (plane.Hp == 0&&plane.ObjectType==ObjectType.EnemyPlane)
                    Score++;
            objects.RemoveAll(plane => plane.ObjectType == ObjectType.EnemyPlane && (plane.Hp <= 0||plane.GameObjectPoint.X+plane.GameObjectSize.X<=0));
        }
        
        private bool CheckCollision(GameObject checkingObject1, GameObject checkingObject2)
        {
            return CheckIntersect(checkingObject1, checkingObject2) && CheckObjectTypes(checkingObject1, checkingObject2);
        }
        private static bool CheckIntersect(GameObject checkingObject1, GameObject checkingObject2)
        {
            return Rectangle.Intersect(
                new Rectangle(checkingObject1.GameObjectPoint.X, checkingObject1.GameObjectPoint.Y, checkingObject1.GameObjectSize.X,
                    checkingObject1.GameObjectSize.Y),
                new Rectangle(checkingObject2.GameObjectPoint.X, checkingObject2.GameObjectPoint.Y, checkingObject2.GameObjectSize.X,
                    checkingObject2.GameObjectSize.Y)) != Rectangle.Empty;
        }

        private static bool CheckObjectTypes(GameObject checkingObject1, GameObject checkingObject2)
        {
            if (checkingObject1.ObjectType == ObjectType.MyPlane && checkingObject2.ObjectType == ObjectType.EnemyPlane ||
                checkingObject2.ObjectType == ObjectType.Shell || checkingObject2.ObjectType == ObjectType.Bird ||
                checkingObject2.ObjectType == ObjectType.Meteor || checkingObject2.ObjectType == ObjectType.Earth)
                return true;
            else 
            if (checkingObject1.ObjectType == ObjectType.Shell &&
                     checkingObject2.ObjectType == ObjectType.EnemyPlane ||
                     checkingObject2.ObjectType == ObjectType.Meteor)
                return true;
            else 
            if (checkingObject1.ObjectType == ObjectType.EnemyPlane &&
                     checkingObject2.ObjectType == ObjectType.Meteor || checkingObject2.ObjectType == ObjectType.Earth)
                return true;
            return false;

        }
        private void DamageMyPlaneIfCollision()
        {
            foreach (GameObject plane in objects)
            {
                if (CheckCollision(objects[0], plane) && plane != objects[0])
                    objects[0].TakeDamage();
            }
            objects.RemoveAll(plane => (plane != objects[0]) && CheckCollision(objects[0], plane));
        }

        public void DeleteShells()
        {
            objects.RemoveAll(shell => shell.ObjectType == ObjectType.Shell && (shell.GameObjectPoint.X >= width - shell.GameObjectSize.X||shell.GameObjectPoint.X<=0));
            foreach (GameObject shell in objects)
                if (shell.ObjectType==ObjectType.Shell&&shell.ObjectDirection == Direction.Right)
                {
                    foreach (GameObject plane in objects)
                        if (CheckCollision(shell, plane)&&plane.ObjectType!=ObjectType.Shell&&shell.GetDirection()==Direction.Right)
                        {
                            plane.TakeDamage();
                            shell.TakeDamage();
                        }
                }
            Score += objects.Count(Object => (Object.Hp <= 0 && Object.ObjectType == ObjectType.EnemyPlane));
            objects.RemoveAll(Object => Object.Hp <= 0);
        }

        public void CreateShell(Direction direction, GameObject plane)
        {
            int shellX = 0;
            if (direction==Direction.Right)
                shellX = plane.GameObjectPoint.X + plane.GameObjectSize.X + 1;
            if(direction==Direction.Left&&plane.IsNeededToCreateShell)
                shellX = plane.GameObjectPoint.X - 1;
            int shellY = plane.GameObjectPoint.Y + plane.GameObjectSize.Y / 2;
            objects.Add(new Shell(new Point(shellX,shellY), direction));            
        }

        private void EnableDodgeIfPlaneInLineWithShell()
        {
            foreach (GameObject gameObject in objects)
            {
                if (gameObject is Fighter)
                {
                    foreach (GameObject shell in objects)
                    {
                        if (shell is Shell && CheckIsPlaneInLineWithSomeObject(gameObject, shell) &&
                            shell.ObjectDirection == Direction.Right&&gameObject.GetDirection()==Direction.None)
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

        private int NewRandomCoordYForFighterPlane(GameObject gameObject)
        {
            int newObjectPoint = gameObject.GameObjectPoint.Y;
            while (newObjectPoint <= gameObject.GameObjectPoint.Y + gameObject.GameObjectSize.Y 
                && newObjectPoint >= gameObject.GameObjectPoint.Y)
            {
                newObjectPoint = Rnd.Next(objects[0].GameObjectPoint.Y/2, height - gameObject.GameObjectSize.Y);
            }
            return newObjectPoint;
        }

        private bool CheckIsPlaneInLineWithSomeObject(GameObject checkingObject1, GameObject checkingObject2)
        {          
            GameObject checkingGameObject2;
            if(checkingObject2 is Shell)
                checkingGameObject2 = new Shell(checkingObject2.GameObjectPoint,checkingObject2.GetDirection());
            else
                checkingGameObject2 = new MyPlane(checkingObject2.GameObjectPoint);
            checkingGameObject2.GameObjectPoint.X = checkingObject1.GameObjectPoint.X;
            return CheckIntersect(checkingObject1, checkingGameObject2);
        }

        private void AttackIfPlaneInLineWithShell()
        {
            List<GameObject> shellsToAdd = new List<GameObject>();
            foreach (GameObject gameObject in objects)
            {
                if (gameObject is HeavyPlane)
                    if (CheckIsPlaneInLineWithSomeObject(gameObject, objects[0])&&shellFrequency%10==0)
                        shellsToAdd.Add(
                            new Shell(
                                new Point(gameObject.GameObjectPoint.X - 1,
                                    gameObject.GameObjectPoint.Y + gameObject.GameObjectSize.Y/2), Direction.Left));
            }
            objects.AddRange(shellsToAdd);
            shellsToAdd.Clear();
        }

        public void CreateEnemyPlane()
        {
            int planeTypeNumber = Rnd.Next(2);
            int planeHeightCoord = Rnd.Next(objects[0].GameObjectSize.Y, height - objects[0].GameObjectSize.Y);
            switch (planeTypeNumber)
            {
                case 0:
                    objects.Add(new HeavyPlane(new Point(width,planeHeightCoord)));                    
                    break;
                case 1:
                    objects.Add(new Fighter(new Point(width, planeHeightCoord)));
                    break;
                default: 
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}