using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace Tetris
{
    public partial class Form1 : Form
    {
        private const int Length = 20;

        private readonly int yOffset;
        private bool shapeCreated;
        private bool gameOver;
        private readonly CellType[,] board = new CellType[Length/2, Length];
        private readonly CellType[,] smallBoard = new CellType[4, 4];
        private SolidBrush shapeBrush = new SolidBrush(Color.Green);
        private bool buildingConstructed;
        private bool canMove = true;
        private bool pause;
        private char shape;
        private int score;
        private char nextShape='1';
        public Shape CreatedShape = new Shape();

        public Form1()
        {
            yOffset = 1;
            shape = '1';
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(Length*10 + 160, Length*20 + 50);
            pictureBox2.Location = new Point(Length * 11  , Length*2);
            pictureBox2.Size = new Size(Length * 4, Length * 4);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics draw = e.Graphics;
            pictureBox1.CreateGraphics();
            Pen linesPen = new Pen(Color.Gray);
            pictureBox1.Height = Length*20 + 1;
            pictureBox1.Width = Length*10 + 1;
            int cellSize = pictureBox1.Width/10;


            for (int i = 0; i < pictureBox1.Width; i++)
                draw.DrawLine(linesPen, cellSize*i, 0, cellSize*i, pictureBox1.Height);
            for (int j = 0; j < pictureBox1.Height; j++)
                draw.DrawLine(linesPen, 0, j*cellSize, pictureBox1.Width, j*cellSize);
            draw.DrawLine(linesPen, pictureBox1.Height, 0, pictureBox1.Width, pictureBox1.Height);
            draw.DrawLine(linesPen, 0, pictureBox1.Height, pictureBox1.Width, pictureBox1.Height);
            int k = 0;
            
            for (int j = Length - 1; j > 0;)
            {
                int countOfCells = 0;
                for (int i = 0; i < Length/2; i++)
                {


                    if (board[i, j] == CellType.Building) countOfCells++;

                }
                if (countOfCells == Length/2)
                {
                    DropDown(j);
                    k++;
                }
                else j--;
            }
            

            if (!shapeCreated)
            {
                
                shapeCreated = true;
                shape = nextShape == '1' ? CreatedShape.CreatedShape() : nextShape;
                nextShape = CreatedShape.CreatedShape();
                timer1.Interval = 600;
                if (shape == 'J')
                {
                    int[] shapeJx = {4, 5, 5, 5};
                    int[] shapeJy = {2, 2, 1, 0};
                    CreatedShape.ShapeX.AddRange(shapeJx);
                    CreatedShape.ShapeY.AddRange(shapeJy);
                    shapeBrush = new SolidBrush(Color.Red);

                }

                if (shape == 'L')
                {
                    int[] shapeJx = {6, 5, 5, 5};
                    int[] shapeJy = {2, 2, 1, 0};
                    CreatedShape.ShapeX.AddRange(shapeJx);
                    CreatedShape.ShapeY.AddRange(shapeJy);
                    shapeBrush = new SolidBrush(Color.Blue);
                }

                if (shape == 'O')
                {
                    int[] shapeJx = {4, 4, 5, 5};
                    int[] shapeJy = {1, 0, 1, 0};
                    CreatedShape.ShapeX.AddRange(shapeJx);
                    CreatedShape.ShapeY.AddRange(shapeJy);
                    shapeBrush = new SolidBrush(Color.Orange);
                }
                if (shape == 'T')
                {
                    int[] shapeJx = {4, 6, 5, 5};
                    int[] shapeJy = {0, 0, 0, 1};
                    CreatedShape.ShapeX.AddRange(shapeJx);
                    CreatedShape.ShapeY.AddRange(shapeJy);
                    shapeBrush = new SolidBrush(Color.Red);
                }
                if (shape == 'Z')
                {
                    int[] shapeJx = {6, 4, 5, 5};
                    int[] shapeJy = {1, 0, 1, 0};
                    CreatedShape.ShapeX.AddRange(shapeJx);
                    CreatedShape.ShapeY.AddRange(shapeJy);
                    shapeBrush = new SolidBrush(Color.Indigo);
                }
                if (shape == 'S')
                {
                    int[] shapeJx = {4, 6, 5, 5};
                    int[] shapeJy = {1, 0, 1, 0};
                    CreatedShape.ShapeX.AddRange(shapeJx);
                    CreatedShape.ShapeY.AddRange(shapeJy);
                    shapeBrush = new SolidBrush(Color.Olive);
                }
                if (shape == 'I')
                {
                    int[] shapeJx = {5, 5, 5, 5};
                    int[] shapeJy = {0, 1, 2, 3};
                    CreatedShape.ShapeX.AddRange(shapeJx);
                    CreatedShape.ShapeY.AddRange(shapeJy);
                    shapeBrush = new SolidBrush(Color.HotPink);
                }
                if (shape == '.')
                {
                    CreatedShape.ShapeX.Add(5);
                    CreatedShape.ShapeY.Add(0);
                    shapeBrush = new SolidBrush(Color.Green);
                }

            }

            for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                if ((board[CreatedShape.ShapeX[i], CreatedShape.ShapeY[i]] == CellType.Building) &&
                    (CreatedShape.ShapeY[i] < 5)) gameOver = true;


            for (int j = 0; j < Length; j++)
            {
                for (int i = 0; i < Length/2; i++)
                {

                    if (board[i, j] != CellType.Building)
                        board[i, j] = CellType.Empty;
                }
            }

            for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
            {
                board[CreatedShape.ShapeX[i], CreatedShape.ShapeY[i]] = CellType.Shape;
            }

            if (k == 1) score = score + 100;
            if (k == 2) score = score + 300;
            if (k == 3) score = score + 700;
            if (k == 4) score = score + 1500;
            label1.Text = "Score:" + score;
            for (int i = 0; i < Length/2; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if ((board[i, j] == CellType.Shape) || (board[i, j] == CellType.Building))
                    {
                        e.Graphics.FillRectangle(shapeBrush, i*cellSize + 1, j*cellSize + 1, cellSize - 1, cellSize - 1);
                    }
                }
            }

            if (gameOver)
            {
                timer1.Stop();
                MessageBox.Show("Lose! Score="+score);

                Restart();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (buildingConstructed) timer1.Interval = 600;
            Moving();
            pictureBox1.Refresh();
            pictureBox2.Refresh();

        }

        private void Moving()
        {
            buildingConstructed = false;
            if (CreatedShape.ShapeX.Count == 1) ////////////./////////
            {
                int newY = CreatedShape.ShapeY[0] + yOffset;
                //shapeX[i] = shapeX[i - 1];
                if ((newY == Length) || (board[CreatedShape.ShapeX[0], newY]) == CellType.Building)
                {
                    board[CreatedShape.ShapeX[0], CreatedShape.ShapeY[0]] = CellType.Building;
                    CreatedShape.ShapeX.Clear();
                    CreatedShape.ShapeY.Clear();
                    shapeCreated = false;
                }
                else CreatedShape.ShapeY[0] = CreatedShape.ShapeY[0] + yOffset;
            }
            if (CreatedShape.ShapeX.Count > 1)
            {
                int[] newY = new int[CreatedShape.ShapeX.Count];
                for (int j = 0; j < CreatedShape.ShapeX.Count; j++)
                {
                    newY[j] = CreatedShape.ShapeY[j] + yOffset;
                    //shapeX[i] = shapeX[i - 1];
                    if ((newY[j] == Length) || (board[CreatedShape.ShapeX[j], newY[j]]) == CellType.Building)
                    {

                        for (int i = 0; i < CreatedShape.ShapeY.Count; i++)
                        {
                            board[CreatedShape.ShapeX[i], CreatedShape.ShapeY[i]] = CellType.Building;
                        }
                        CreatedShape.ShapeX.Clear();
                        CreatedShape.ShapeY.Clear();
                        shapeCreated = false;
                    }
                }
                for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                {
                    CreatedShape.ShapeY[i] = CreatedShape.ShapeY[i] + yOffset;
                }

            }

        }

        private void MovingToward(string direction)
        {
            if (direction == "right")
            {
                for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                {
                    if ((CreatedShape.ShapeX[i] - 1 < 0) ||
                        (board[CreatedShape.ShapeX[i] - 1, CreatedShape.ShapeY[i]] == CellType.Building))
                        canMove = false;
                }

                if (canMove)
                    for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                    {
                        CreatedShape.ShapeX[i] = CreatedShape.ShapeX[i] - 1;
                    }
            }
        
            if (direction == "left")
            {
                for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                {
                    if ((CreatedShape.ShapeX[i] + 1 >= Length / 2) ||
                        (board[CreatedShape.ShapeX[i] + 1, CreatedShape.ShapeY[i]] == CellType.Building))
                        canMove = false;
                }
                if (canMove)
                    for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                    {
                        CreatedShape.ShapeX[i] = CreatedShape.ShapeX[i] + 1;
                    }
            }
        }

        private void Restart()
        {
            shapeCreated = false;
            gameOver = false;
            score = 0;
            CreatedShape.ShapeX.Clear();
            CreatedShape.ShapeY.Clear();
            for (int i = 0; i < Length/2; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    board[i, j] = CellType.Empty;
                }
            }
            timer1.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if ((e.KeyCode == Keys.A) || (e.KeyCode == Keys.Left))
            {      
                MovingToward("right");
            }
            if ((e.KeyCode == Keys.D) || (e.KeyCode == Keys.Right))
            {
                MovingToward("left");
            }
            if (e.KeyCode == Keys.S)
            {

                if (shapeCreated)
                    timer1.Interval = 100;
                //pictureBox1.Refresh();
            }

            if (e.KeyCode == Keys.W)
            {
                if ((shape == 'Z') || (shape == 'S') || (shape == 'L') || (shape == 'J') || (shape == 'T'))
                    if (CheckRotate())
                        CreatedShape.Rotate();
                if (shape == 'I')
                    if (CheckRotate())
                        CreatedShape.SpecialRotate();
            }

            if (e.KeyCode == Keys.Space)
            {
                if (shapeCreated)
                    timer1.Interval = 10;
                pictureBox1.Refresh();
            }
            if (e.KeyCode == Keys.Escape)
            {
                if (pause)
                {
                    timer1.Start();
                    pause = false;
                }
                else
                {
                    timer1.Stop();

                    pause = true;

                    for (int j = 0; j < Length; j++)
                    {
                        Console.WriteLine();
                        for (int i = 0; i < Length/2; i++)
                        {
                            if (board[i, j] == CellType.Empty) Console.Write("0");
                            if (board[i, j] == CellType.Building) Console.Write("1");
                            if (board[i, j] == CellType.Shape) Console.Write("2");
                        }
                    }
                }
            }
            canMove = true;
            pictureBox1.Refresh();
        }

        private bool CheckRotate()
        {
            if (((shape == 'Z') || (shape == 'S') || (shape == 'L') || (shape == 'J')) &&
                (CreatedShape.ShapeX[2] < Length/2 - 1) && (CreatedShape.ShapeX[2] > 0) &&
                (board[CreatedShape.ShapeX[2] + 1, CreatedShape.ShapeY[2]] != CellType.Building) &&
                (board[CreatedShape.ShapeX[2] - 1, CreatedShape.ShapeY[2]] != CellType.Building) &&
                (board[CreatedShape.ShapeX[2], CreatedShape.ShapeY[2] + 1] != CellType.Building) &&
                (board[CreatedShape.ShapeX[2], CreatedShape.ShapeY[2] - 1] != CellType.Building) &&
                (CreatedShape.ShapeY[2] > 0))
                return true;
            if (shape == 'T')
            {
                for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                {
                    if ((CreatedShape.ShapeY[2] - 1 < 0) &&(CreatedShape.ShapeY[i] - 1 < 0)) return false;
                }
                if ((CreatedShape.ShapeX[2] < Length/2 - 1) && (CreatedShape.ShapeX[2] > 0) &&
                    (board[CreatedShape.ShapeX[2] + 1, CreatedShape.ShapeY[2]] != CellType.Building) &&
                    (board[CreatedShape.ShapeX[2] - 1, CreatedShape.ShapeY[2]] != CellType.Building) &&
                    (board[CreatedShape.ShapeX[2], CreatedShape.ShapeY[2] + 1] != CellType.Building) &&
                    (board[CreatedShape.ShapeX[2], CreatedShape.ShapeY[2] - 1] != CellType.Building) &&
                    (CreatedShape.ShapeY[2] > 0))
                    return true;
            }
            if (shape == 'I')
            {
                if((CreatedShape.ShapeX[1]==CreatedShape.ShapeX[3])&&(CreatedShape.ShapeX[3] < Length / 2 - 2) && (CreatedShape.ShapeX[3] !=0)) 
                if ((board[CreatedShape.ShapeX[1] + 2, CreatedShape.ShapeY[1]] != CellType.Building) &&
                    (board[CreatedShape.ShapeX[1] -1, CreatedShape.ShapeY[1]] != CellType.Building) &&
                    (board[CreatedShape.ShapeX[1], CreatedShape.ShapeY[1]-1] != CellType.Building) &&
                    (board[CreatedShape.ShapeX[1], CreatedShape.ShapeY[1] + 2] != CellType.Building))
                    return true;
                if((CreatedShape.ShapeY[1] == CreatedShape.ShapeY[3]) && (CreatedShape.ShapeX[1] >= 1))
                    if ((board[CreatedShape.ShapeX[1] + 2, CreatedShape.ShapeY[1]] != CellType.Building) &&
                    (board[CreatedShape.ShapeX[1] - 1, CreatedShape.ShapeY[1]] != CellType.Building) &&
                    (board[CreatedShape.ShapeX[1], CreatedShape.ShapeY[1] - 1] != CellType.Building) &&
                    (board[CreatedShape.ShapeX[1], CreatedShape.ShapeY[1] + 2] != CellType.Building))
                        return true;
            }

            return false;
        }

        private void DropDown(int lineNumber)
        {
            for (int i = 0; i < Length/2; i++)
            {
                board[i, lineNumber] = CellType.Empty;
            }
            for (int i = 0; i < Length/2; i++)
            {
                for (int j = lineNumber; j > 0; j--)
                {
                    board[i, j] = board[i, j - 1];
                    board[i, j - 1] = CellType.Empty;

                }
            }
        }

        private void label1_Paint(object sender, PaintEventArgs e)
        {
            label1.Left = Length*10 + 20;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
            {
                if (shapeCreated)
                    timer1.Interval = 600;
                pictureBox1.Refresh();
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            //Graphics draw = e.Graphics;
            pictureBox2.CreateGraphics();
            //Pen linesPen = new Pen(Color.Gray);
            int cellSize = pictureBox2.Width / 4;
             List<int> shapeXsmall = new List<int>();
             List<int> shapeYsmall = new List<int>();
            Console.WriteLine(nextShape);
            //draw.DrawLine(linesPen, 0, 0, 0, pictureBox2.Height);
            //draw.DrawLine(linesPen, 0, 0, pictureBox2.Width, 0);
            //draw.DrawLine(linesPen, pictureBox2.Width-1, 0, pictureBox2.Width-1, pictureBox2.Height);
            //draw.DrawLine(linesPen, 0, pictureBox2.Height-1, pictureBox2.Width, pictureBox2.Height-1);
            if (nextShape == 'J')
            {
                int[] shapeJx = { 1, 2, 2, 2 };
                int[] shapeJy = { 2, 2, 1, 0 };
                shapeXsmall.AddRange(shapeJx);
                shapeYsmall.AddRange(shapeJy);
                shapeBrush = new SolidBrush(Color.Red);

            }

            if (nextShape == 'L')
            {
                int[] shapeJx = { 3, 2, 2, 2 };
                int[] shapeJy = { 2, 2, 1, 0 };
                shapeXsmall.AddRange(shapeJx);
                shapeYsmall.AddRange(shapeJy);
                shapeBrush = new SolidBrush(Color.Blue);
            }

            if (nextShape == 'O')
            {
                int[] shapeJx = { 1, 1, 2, 2 };
                int[] shapeJy = { 1, 0, 1, 0 };
                shapeXsmall.AddRange(shapeJx);
                shapeYsmall.AddRange(shapeJy);
                shapeBrush = new SolidBrush(Color.Orange);
            }
            if (nextShape == 'T')
            {
                int[] shapeJx = { 1, 3, 2, 2 };
                int[] shapeJy = { 0, 0, 0, 1 };
                shapeXsmall.AddRange(shapeJx);
                shapeYsmall.AddRange(shapeJy);
                shapeBrush = new SolidBrush(Color.Red);
            }
            if (nextShape == 'Z')
            {
                int[] shapeJx = { 3, 1, 2, 2 };
                int[] shapeJy = { 1, 0, 1, 0 };
                shapeXsmall.AddRange(shapeJx);
                shapeYsmall.AddRange(shapeJy);
                shapeBrush = new SolidBrush(Color.Indigo);
            }
            if (nextShape == 'S')
            {
                int[] shapeJx = { 1, 3, 2, 2 };
                int[] shapeJy = { 1, 0, 1, 0 };
                shapeXsmall.AddRange(shapeJx);
                shapeYsmall.AddRange(shapeJy);
                shapeBrush = new SolidBrush(Color.Olive);
            }
            if (nextShape == 'I')
            {
                int[] shapeJx = { 2, 2, 2, 2 };
                int[] shapeJy = { 3, 2, 1, 0 };
                shapeXsmall.AddRange(shapeJx);
                shapeYsmall.AddRange(shapeJy);
                shapeBrush = new SolidBrush(Color.HotPink);
            }
            if (nextShape == '.')
            {
                shapeXsmall.Add(2);
                shapeYsmall.Add(0);
                shapeBrush = new SolidBrush(Color.Green);
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    smallBoard[i, j] = CellType.Empty;
                }
                
            }
            for (int i = 0; i < shapeXsmall.Count; i++)
            {
               
                
                smallBoard[shapeXsmall[i], shapeYsmall[i]] = CellType.Shape;
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (smallBoard[i, j] == CellType.Shape)
                    {
                        e.Graphics.FillRectangle(shapeBrush, i * cellSize + 1, j * cellSize + 1, cellSize - 1, cellSize - 1);
                    }
                }
            }
            shapeXsmall.Clear();
            shapeXsmall.Clear();
        }
    }
}
