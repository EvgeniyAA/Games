using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Shape
    {
        public readonly List<int> ShapeX = new List<int>();
        public readonly List<int> ShapeY = new List<int>();
        private readonly Random rnd = new Random();
        public char[] ShapeType { get; } ={'O', 'I', 'J', 'L', 'S', 'Z', 'T', '.'};

        public char CreatedShape()
        {
            return ShapeType[rnd.Next(8)];
        }


        public void SpecialRotate()
        {
            int t = 0;
            bool rotated = false;
            if ((ShapeX[1] == ShapeX[2])&&(!rotated))
            {
                for (int i = 0; i < ShapeX.Count; i++)
                {
                    ShapeY[i] = ShapeY[1];
                }
                for (int j = 0; j < ShapeX.Count; j++)
                {
                    t = j - 1;
                    ShapeX[j] = ShapeX[j] + t;
                }
                rotated = true;
            }
            if ((ShapeY[1] == ShapeY[2])&&(!rotated))
            {
                for (int i = 0; i < ShapeX.Count; i++)
                {
                    ShapeX[i] = ShapeX[1];
                }
                for (int j = 0; j < ShapeX.Count; j++)
                {
                    t = j - 1;
                    ShapeY[j] = ShapeY[j] + t;
                }
                rotated = true;
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
