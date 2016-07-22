using System;
using System.Collections.Generic;
using System.Drawing;

namespace AirForce
{
    internal class Game
    {
        private readonly int width;
        private readonly int height;
        private readonly Random rnd = new Random();
        private readonly List<Shell> shells;
        private readonly List<Object> objects;
        public int Score;
        private bool stopShoot=true;
        private int shellFrequency;

        public Game(int width, int height)
        {
            this.width = width;
            this.height = height;
            shells = new List<Shell>();
            objects = new List<Object> {new MyPlane(width/22, height/2, height)};
        }

        public void Draw(Graphics graphics)
        {
            foreach (Shell shell in shells)
                shell.Draw(graphics);

            foreach (Object plane in objects)
                plane.Draw(graphics);
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
            {
                CreateShell(Direction.Right, objects[0]);
            }
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
            foreach (Object plane in objects)
                plane.Move();
                
            if(countOfTicks%20==0)
                CreateEnemyPlane();
            
            foreach (Shell shell in shells)
                shell.ShellsMoving();
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
            foreach (Object plane in objects)
                if (plane.Hp == 0&&plane!=objects[0])
                    Score++;
            objects.RemoveAll(plane => (objects[0] != plane && plane.Hp <= 0));
        }
        private bool CheckShellIntersect(Shell checkingShell, Object checkingPlane)
        {
            return (Rectangle.Intersect(new Rectangle(checkingShell.X, checkingShell.Y, checkingShell.Size, checkingShell.Size),
                new Rectangle(checkingPlane.PlaneX1, checkingPlane.PlaneY1, checkingPlane.PlaneWidth,
                    checkingPlane.PlaneHeight)) != Rectangle.Empty);
        }
        private bool CheckPlaneIntersect(Object checkingPlane1, Object checkingPlane2)
        {
            return (Rectangle.Intersect(new Rectangle(checkingPlane1.PlaneX1, checkingPlane1.PlaneY1, checkingPlane1.PlaneWidth, checkingPlane1.PlaneHeight),
                new Rectangle(checkingPlane2.PlaneX1, checkingPlane2.PlaneY1, checkingPlane2.PlaneWidth,
                    checkingPlane2.PlaneHeight)) != Rectangle.Empty);
        }

        private void DamagePlane()
        {
            foreach (Object plane in objects)
            {
                if(CheckPlaneIntersect(objects[0], plane)&&plane!=objects[0])
                    objects[0].TakeDamage();
            }
                objects.RemoveAll(plane => (plane!=objects[0])&&CheckPlaneIntersect(objects[0], plane));
        }

        private void DeleteShells()
        {
            shells.RemoveAll(shell => shell.X >= width - shell.Size);
            foreach (Shell shell in shells)
                if (shell.Direction == Direction.Right)
                {
                    foreach (Object plane in objects)
                        if (CheckShellIntersect(shell, plane))                                               
                            plane.TakeDamage();
                }
            foreach (Object plane in objects)
                shells.RemoveAll(shell => CheckShellIntersect(shell, plane));
        }

        public void CreateShell(Direction direction, Object plane)
        {
            int shellX = plane.PlaneX1 + plane.PlaneWidth + 1;
            int shellY = plane.PlaneY1 + plane.PlaneHeight/2;
            shells.Add(new Shell(direction, 3, 5, shellX, shellY));
        }

        public void CreateEnemyPlane()
        {
            int planeTypeNumber = rnd.Next(2);
            int planeHeightCoord = rnd.Next(objects[0].PlaneHeight, height - objects[0].PlaneHeight);
            switch (planeTypeNumber)
            {
                case 0:
                    objects.Add(new HeavyPlane(width, planeHeightCoord));                    
                    break;
                case 1:
                    objects.Add(new Fighter(width, planeHeightCoord));
                    break;                   
            }
        }

        
    }
}