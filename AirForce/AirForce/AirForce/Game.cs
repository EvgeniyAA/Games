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
        private readonly Random rnd = new Random();
        private readonly List<GameObject> objects;
        public int Score;
        private bool stopShoot=true;
        private int shellFrequency;

        public Game(int width, int height)
        {
            this.width = width;
            this.height = height;
            objects = new List<GameObject> {new MyPlane(width/22, height/2, height)};
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
            if (!stopShoot&&shellFrequency%10==0)
                CreateShell(Direction.Right, objects[0]);
        }
        public void StopAttack()
        {
            stopShoot = true;
        }
        public void StartAttack()
        {
            stopShoot = false;
        }
        public void Update(int countOfTicks)
        {
            foreach (GameObject Object in objects)
                Object.Move();
                
            if(countOfTicks%20==0)
                CreateEnemyPlane();

            PlayerStartShoot();
            DeleteShells();
            DamagePlane();
            DeletePlanes();            
        }
        public bool IsGameOver()
        {
            return objects[0].Hp == 0;
        }

        public void DeletePlanes()
        {
            foreach (GameObject plane in objects)
                if (plane.Hp == 0&&plane.ObjectType==ObjectType.EnemyPlane)
                    Score++;
            objects.RemoveAll(plane => plane.ObjectType == ObjectType.EnemyPlane && plane.Hp <= 0);
        }
        private static bool CheckShellIntersect(GameObject checkingObject1, GameObject checkingObject2)
        {
            return (checkingObject1.ObjectType == ObjectType.Shell)
                && (checkingObject2.ObjectType == ObjectType.EnemyPlane)
                   && CheckIntersect(checkingObject1, checkingObject2);
        }
        private static bool CheckPlaneIntersect(GameObject checkingPlane1, GameObject checkingPlane2)
        {
            return checkingPlane1.ObjectType == ObjectType.MyPlane
                   && checkingPlane2.ObjectType == ObjectType.EnemyPlane
                   && CheckIntersect(checkingPlane1, checkingPlane2);
        }

        private static bool CheckIntersect(GameObject checkingPlane1, GameObject checkingPlane2)
        {
            return Rectangle.Intersect(
                new Rectangle(checkingPlane1.ObjectX1, checkingPlane1.ObjectY1, checkingPlane1.ObjectWidth,
                    checkingPlane1.ObjectHeight),
                new Rectangle(checkingPlane2.ObjectX1, checkingPlane2.ObjectY1, checkingPlane2.ObjectWidth,
                    checkingPlane2.ObjectHeight)) != Rectangle.Empty;
        }

        private void DamagePlane()
        {
            foreach (GameObject plane in objects)
            {
                if (CheckPlaneIntersect(objects[0], plane) && plane != objects[0])
                    objects[0].TakeDamage();
            }
            objects.RemoveAll(plane => (plane != objects[0]) && CheckPlaneIntersect(objects[0], plane));
        }

        private void DeleteShells()
        {
            objects.RemoveAll(shell => shell.ObjectType == ObjectType.Shell && shell.ObjectX1 >= width - shell.ObjectWidth);
            foreach (GameObject shell in objects)
                if (shell.ObjectType==ObjectType.Shell&&shell.ObjectDirection == Direction.Right)
                {
                    foreach (GameObject plane in objects)
                        if (CheckShellIntersect(shell, plane))
                        {
                            plane.TakeDamage();
                            shell.TakeDamage();
                        }
                }
            Score += objects.Count(Object => (Object.Hp <= 0&&Object.ObjectType==ObjectType.EnemyPlane));
            objects.RemoveAll(Object => Object.Hp <= 0);
            //for (int i = 1; i < objects.Count-1; i++)
            //{
            //    for (int j = i+1; j < objects.Count; j++)
            //    {
            //        if(CheckShellIntersect(objects[i],objects[j])&&objects[i].ObjectType==ObjectType.Shell&&objects[j].ObjectType==ObjectType.EnemyPlane)
            //            objects.RemoveAt(i);
            //    }
            //}
            //foreach (GameObject plane in objects)
            //{
            //    objects.RemoveAll(shell => CheckShellIntersect(shell, plane));  
            //}
        }

        public void CreateShell(Direction direction, GameObject plane)
        {
            int shellX = plane.ObjectX1 + plane.ObjectWidth + 1;
            int shellY = plane.ObjectY1 + plane.ObjectHeight/2;
            objects.Add(new Shell(shellX, shellY,direction));
        }

        public void CreateEnemyPlane()
        {
            int planeTypeNumber = rnd.Next(2);
            int planeHeightCoord = rnd.Next(objects[0].ObjectHeight, height - objects[0].ObjectHeight);
            switch (planeTypeNumber)
            {
                case 0:
                    objects.Add(new HeavyPlane(width, planeHeightCoord));                    
                    break;
                case 1:
                    objects.Add(new Fighter(width, planeHeightCoord));
                    break;
                default: 
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}