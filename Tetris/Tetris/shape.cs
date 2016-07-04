using System;
using System.Collections.Generic;
using System.Drawing;

namespace Tetris
{
    public class Shape
    {
        public List<int> ShapeX = new List<int>();
        public List<int> ShapeY = new List<int>();
        public string position;//todo
        private readonly Random rnd = new Random();
        public char shape1;
        public char[] ShapeType { get; } ={'O', 'I', 'J', 'L', 'S', 'Z', 'T', '.'};

        public char CreateShape()
        {
            return ShapeType[rnd.Next(8)];
        }

        public Shape(int centerX, char shape)
        {
            shape1 = shape;
            if (shape1 == '1')
            {
                shape1 = CreateShape();
            }
            switch (shape1)
            {
                case 'J':
                    ShapeX.AddRange(new[] {centerX - 1, centerX, centerX, centerX});
                    ShapeY.AddRange(new[] {2, 2, 1, 0});
                    break;
                case 'L':
                    ShapeX.AddRange(new[] {centerX + 1, centerX, centerX, centerX});
                    ShapeY.AddRange(new[] {2, 2, 1, 0});
                    break;
                case 'O':
                    ShapeX.AddRange(new[] {centerX + 1, centerX+1, centerX, centerX});
                    ShapeY.AddRange(new[] {1, 0, 1, 0});
                    break;
                case 'T':
                    ShapeX.AddRange(new[] {centerX - 1, centerX + 1, centerX, centerX});
                    ShapeY.AddRange(new[] {0, 0, 0, 1});
                    break;
                case 'Z':
                    ShapeX.AddRange(new[] {centerX + 1, centerX - 1, centerX, centerX});
                    ShapeY.AddRange(new[] {1, 0, 1, 0});
                    break;
                case 'S':
                    ShapeX.AddRange(new[] {centerX - 1, centerX + 1, centerX, centerX});
                    ShapeY.AddRange(new[] {1, 0, 1, 0});
                    break;
                case 'I':
                    ShapeX.AddRange(new[] {centerX, centerX, centerX, centerX});
                    ShapeY.AddRange(new[] {0, 1, 2, 3});
                    break;
                case '.':
                    ShapeX.Add(centerX);
                    ShapeY.Add(0);
                    break;
            }
        }

        public void SpecialRotate(char checkShape)
        {
            if (checkShape == 'I')
            {
                int t = 0;
                bool rotated = false;
                if ((ShapeX[1] == ShapeX[2]) && (!rotated))
                {
                    for (int i = 0; i < ShapeX.Count; i++)
                        ShapeY[i] = ShapeY[1];
                    for (int j = 0; j < ShapeX.Count; j++)
                    {
                        t = j - 1;
                        ShapeX[j] = ShapeX[j] + t;
                    }
                    rotated = true;
                }
                if ((ShapeY[1] == ShapeY[2]) && (!rotated))
                {
                    for (int i = 0; i < ShapeX.Count; i++)
                        ShapeX[i] = ShapeX[1];
                    for (int j = 0; j < ShapeX.Count; j++)
                    {
                        t = j - 1;
                        ShapeY[j] = ShapeY[j] + t;
                    }
                    rotated = true;
                }
            }
            if (checkShape == 'L')
            {
                
            }
        }
        public void Rotate()
        {
            
            for (int i = 0; i < ShapeX.Count; i++)
            {
                bool rotated = false;
                if (i != 2)
                {
                    if ((ShapeX[i] > ShapeX[2]) && (ShapeY[i] == ShapeY[2]))  
                    {
                        ShapeY[i] = ShapeY[i] + 1;
                        ShapeX[i] = ShapeX[i] - 1;
                        rotated = true;
                    }
                    else if ((ShapeX[i] < ShapeX[2]) && (ShapeY[i] == ShapeY[2])) 
                    {
                        ShapeY[i] = ShapeY[i] - 1;
                        ShapeX[i] = ShapeX[i] + 1;
                        rotated = true;
                    }
                    if (!rotated)
                    {
                        if ((ShapeX[i] == ShapeX[2]) && (ShapeY[i] < ShapeY[2]))
                        {
                            ShapeY[i] = ShapeY[i] + 1;
                            ShapeX[i] = ShapeX[i] + 1;
                            rotated = true;
                        }
                        else if ((ShapeX[i] == ShapeX[2]) && (ShapeY[i] > ShapeY[2]))
                        {
                            ShapeY[i] = ShapeY[i] - 1;
                            ShapeX[i] = ShapeX[i] - 1;
                            rotated = true;
                        }
                    }
                    if(rotated==false)
                    {
                        if ((ShapeX[i] > ShapeX[2]) && (ShapeY[i] > ShapeY[2]))
                        {
                            ShapeX[i] = ShapeX[i] - 2;
                            rotated = true;
                        }
                        else if ((ShapeX[i] < ShapeX[2]) && (ShapeY[i] > ShapeY[2]))
                        {
                            ShapeY[i] = ShapeY[i] - 2;
                            rotated = true;
                        }

                        
                    }
                    
                    if(rotated==false)
                    {
                        if ((ShapeX[i] > ShapeX[2]) && (ShapeY[i] < ShapeY[2]))
                        {
                            ShapeY[i] = ShapeY[i] + 2;
                            rotated = true;
                        }
                        else if ((ShapeX[i] < ShapeX[2]) && (ShapeY[i] < ShapeY[2]))
                        {
                            ShapeX[i] = ShapeX[i] + 2;
                            rotated = true;
                        }
                    }
                }
            }
        }

    }
}
