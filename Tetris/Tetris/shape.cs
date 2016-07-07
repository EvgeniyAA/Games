using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Shape
    {
        public List<int> ShapeX = new List<int>();
        public List<int> ShapeY = new List<int>();
        public enum Position
        {
            FromTopToDown,
            FromRightToLeft
        }

        public Position position;
        private static readonly Random Rnd = new Random();
        private readonly bool[] mustBeRotated = new bool[4];
        public int RotateCount;
        public char Shape1;
        private readonly char[] shapeType ={'O', 'I', 'J', 'L', 'S', 'Z', 'T', '.'};

        private char CreateShape()
        {
            return shapeType[Rnd.Next(8)];
        }

        private void SwitchShapeType(int centerX)
        {
            switch (Shape1)
            {
                case 'J':
                    ShapeX.AddRange(new[] { centerX - 1, centerX, centerX, centerX });
                    ShapeY.AddRange(new[] { 2, 2, 1, 0 });
                    break;
                case 'L':
                    ShapeX.AddRange(new[] { centerX + 1, centerX, centerX, centerX });
                    ShapeY.AddRange(new[] { 2, 2, 1, 0 });
                    break;
                case 'O':
                    ShapeX.AddRange(new[] { centerX + 1, centerX + 1, centerX, centerX });
                    ShapeY.AddRange(new[] { 1, 0, 1, 0 });
                    break;
                case 'T':
                    ShapeX.AddRange(new[] { centerX - 1, centerX + 1, centerX, centerX });
                    ShapeY.AddRange(new[] { 0, 0, 0, 1 });
                    break;
                case 'Z':
                    ShapeX.AddRange(new[] { centerX + 1, centerX - 1, centerX, centerX });
                    ShapeY.AddRange(new[] { 1, 0, 1, 0 });
                    break;
                case 'S':
                    ShapeX.AddRange(new[] { centerX - 1, centerX + 1, centerX, centerX });
                    ShapeY.AddRange(new[] { 1, 0, 1, 0 });
                    break;
                case 'I':
                    ShapeX.AddRange(new[] { centerX, centerX, centerX, centerX });
                    ShapeY.AddRange(new[] { 0, 1, 2, 3 });
                    break;
                case '.':
                    ShapeX.Add(centerX);
                    ShapeY.Add(0);
                    break;
            }
        }
        public Shape(int centerX, char shape)
        {
            Shape1 = shape;
            SwitchShapeType(centerX);
            position = Position.FromTopToDown;
        }

       // public Shape(int centerX) : this(centerX, '1') { }
        public Shape(int centerX)
        {
            Shape1 = CreateShape();
            SwitchShapeType(centerX);
            position = Position.FromTopToDown;
        }

        public void LineRotate()
        {
            bool isRotated = false;
            if ((position == Position.FromTopToDown) && !isRotated)
            {
                for (int i = 0; i < ShapeX.Count; i++)
                        ShapeY[i] = ShapeY[1];
                    for (int j = 0; j < ShapeX.Count; j++)
                    {
                        int t = j - 1;
                        ShapeX[j] += t;
                    }

                position = Position.FromRightToLeft;
                isRotated = true;
            }
            if ((position == Position.FromRightToLeft) && !isRotated)
            {
                for (int i = 0; i < ShapeX.Count; i++)
                        ShapeX[i] = ShapeX[1];
                    for (int j = 0; j < ShapeX.Count; j++)
                    {
                        int t = j - 1;
                        ShapeY[j] += t;
                    }
                position = Position.FromTopToDown;
            }
        }

        public void Rotate()
        {
            if ((RotateCount % 2 != 0) && ((Shape1 == 'Z') || (Shape1 == 'S')))
            {
                BackRotateCross();
                BackRotateAngles();
                RotateCount = 0;
            }
            else if (RotateCount%2 == 0||((Shape1!='Z')&&(Shape1!='S')))
            {
                RotateCross();
                RotateAngles();
                RotateCount++;
            }           
        }

        private void BackRotateAngles()
        {
            for (int i = 0; i < ShapeX.Count; i++)
            {
                if (i != 2 )
                    if ((ShapeX[i] > ShapeX[2]) && (ShapeY[i] < ShapeY[2]) && (mustBeRotated[i]))
                        ShapeX[i] -= 2;
                    else if ((ShapeX[i] > ShapeX[2]) && (ShapeY[i] > ShapeY[2]) && (mustBeRotated[i]))
                        ShapeY[i] -= 2;
            }
        }
        private void BackRotateCross()
        {
            for (int i = 0; i < ShapeX.Count; i++)
            {
                mustBeRotated[i] = false;
                bool isRotated = false;
                if (i != 2)
                {
                    if (!isRotated && (ShapeX[i] > ShapeX[2]) && (ShapeY[i] == ShapeY[2]) && Shape1 == 'Z')
                    {
                        ShapeY[i] -= 1;
                        ShapeX[i] -= 1;
                        isRotated = true;
                    }
                    if (!isRotated && (ShapeX[i] == ShapeX[2]) && (ShapeY[i] > ShapeY[2]) && Shape1 == 'Z')
                    {
                        ShapeY[i] -= 1;
                        ShapeX[i] += 1;
                        isRotated = true;
                    }
                    if (!isRotated && (ShapeX[i] > ShapeX[2]) && (ShapeY[i] == ShapeY[2]) && Shape1 == 'S')
                    {
                        ShapeY[i] -= 1;
                        ShapeX[i] -= 1;
                        isRotated = true;
                    }
                    if (!isRotated && (ShapeX[i] == ShapeX[2]) && (ShapeY[i] < ShapeY[2]) && Shape1 == 'S')
                    {
                        ShapeY[i] += 1;
                        ShapeX[i] -= 1;
                        isRotated = true;
                    }
                    if (!isRotated) mustBeRotated[i] = true;
                }
            }
        }

        private void RotateCross()
        {         
            for (int i = 0; i < ShapeX.Count; i++)
            {
                mustBeRotated[i] = false;
                bool isRotated = false;
                if (i != 2)
                {
                    if ((ShapeX[i] > ShapeX[2]) && (ShapeY[i] == ShapeY[2]))
                    {
                        ShapeY[i] += 1;
                        ShapeX[i] -= 1;
                        isRotated = true;  
                    }
                    else if ((ShapeX[i] < ShapeX[2]) && (ShapeY[i] == ShapeY[2])) 
                    {
                        ShapeY[i] -= 1;
                        ShapeX[i] += 1;
                        isRotated = true;                        
                    }
                    if (!isRotated)
                    { 
                        if ((ShapeX[i] == ShapeX[2]) && (ShapeY[i] < ShapeY[2]))
                        {
                            ShapeY[i] += 1;
                            ShapeX[i] += 1;
                            isRotated = true;                          
                        }
                        else if ((ShapeX[i] == ShapeX[2]) && (ShapeY[i] > ShapeY[2]))
                        {
                            ShapeY[i] -= 1;
                            ShapeX[i] -= 1;
                            isRotated = true;                           
                        }
                    }
                    if (!isRotated)
                        mustBeRotated[i] = true;
                }
            }
        }
        private void RotateAngles()
        {
            for (int i = 0; i < ShapeX.Count; i++)
            {
                bool isRotated = false;
                if (i != 2)
                {
                    if (!isRotated)
                    {
                        if ((ShapeX[i] > ShapeX[2]) && (ShapeY[i] > ShapeY[2])&&(mustBeRotated[i]))
                        {
                            ShapeX[i] -= 2;
                            isRotated = true;                           
                        }
                        else if ((ShapeX[i] < ShapeX[2]) && (ShapeY[i] > ShapeY[2]) && (mustBeRotated[i]))
                        {
                            ShapeY[i] -= 2;
                            isRotated = true;                           
                        }                        
                    }
                    if (!isRotated)
                    {
                        if ((ShapeX[i] > ShapeX[2]) && (ShapeY[i] < ShapeY[2]) && (mustBeRotated[i]))
                            ShapeY[i] += 2;                    
                        else if ((ShapeX[i] < ShapeX[2]) && (ShapeY[i] < ShapeY[2]) && (mustBeRotated[i]))
                            ShapeX[i] += 2;
                    }
                }
            }
        }
    }
}
