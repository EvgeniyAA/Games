using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private const int Length = 20;

        private readonly int yOffset;
        private readonly CellType[,] board = new CellType[Length/2, Length];
        private readonly CellType[,] smallBoard = new CellType[4, 4];
        private readonly SolidBrush shapeBrush = new SolidBrush(Color.Green);
        private bool buildingConstructed;
        private bool canMove = true;
        private bool pause;
        private int score;
        public Shape CreatedShape;
        public Shape NextShape;

        public Form1()
        {
            yOffset = 1;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(Length*10 + 160, Length*20 + 50);
            pictureBox2.Location = new Point(Length*11, Length*2);
            pictureBox2.Size = new Size(Length*4, Length*4);
            Restart();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics draw = e.Graphics;
            pictureBox1.CreateGraphics();
            Pen linesPen = new Pen(Color.Gray);
            pictureBox1.Height = Length*20 + 1;
            pictureBox1.Width = Length*10 + 1;
            int cellSize = pictureBox1.Width/10;

            for (int i = 0; i <= pictureBox1.Width; i++)
            {
                draw.DrawLine(linesPen, cellSize*i, 0, cellSize*i, pictureBox1.Height);
                draw.DrawLine(linesPen, 0, i*cellSize, pictureBox1.Width, i*cellSize);
            }
            CountScore();
            if (GameOver())
            {
                timer1.Stop();
                MessageBox.Show("Lose! Score=" + score);
                CreatedShape.ShapeX.Clear();
                CreatedShape.ShapeY.Clear();
                NextShape.ShapeY.Clear();
                NextShape.ShapeY.Clear();
                Restart();
            }
            for (int j = 0; j < Length; j++)
            {
                for (int i = 0; i < Length/2; i++)
                {
                    if (board[i, j] != CellType.Building)
                        board[i, j] = CellType.Empty;
                }
            }

            for (int i = 0; i < CreatedShape.ShapeX.Count; i++)            
                board[CreatedShape.ShapeX[i], CreatedShape.ShapeY[i]] = CellType.Shape;            
      
            for (int i = 0; i < Length/2; i++)           
                for (int j = 0; j < Length; j++)                
                    if ((board[i, j] == CellType.Shape) || (board[i, j] == CellType.Building))                   
                        e.Graphics.FillRectangle(shapeBrush, i*cellSize + 1, j*cellSize + 1, cellSize - 1, cellSize - 1);          
        }

        private void CountScore()
        {
            int k = 0;

            for (int j = Length - 1; j > 0;)
            {
                int countOfCells = 0;
                for (int i = 0; i < Length / 2; i++)
                {
                    if (board[i, j] == CellType.Building)
                        countOfCells++;
                }
                if (countOfCells == Length / 2)
                {
                    DropDown(j);
                    k++;
                }
                else j--;
            }
            if (k == 1)
                score = score + 100;
            if (k == 2)
                score = score + 300;
            if (k == 3)
                score = score + 700;
            if (k == 4)
                score = score + 1500;
            label1.Text = "Score:" + score;
        }
        private bool GameOver()
        {
            bool gameOver = false;
            for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                if ((board[CreatedShape.ShapeX[i], CreatedShape.ShapeY[i]] == CellType.Building) &&
                    (CreatedShape.ShapeY[i] < 5)) gameOver = true;
            return gameOver;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (buildingConstructed)
                timer1.Interval = 600;
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
                if ((newY == Length) || (board[CreatedShape.ShapeX[0], newY]) == CellType.Building)
                {
                    board[CreatedShape.ShapeX[0], CreatedShape.ShapeY[0]] = CellType.Building;
                    CreatedShape.ShapeX.Clear();
                    CreatedShape.ShapeY.Clear();
                    CreatedShape = new Shape(Length/4,NextShape.shape1);
                    NextShape.ShapeX.Clear();
                    NextShape.ShapeY.Clear();
                    timer1.Interval = 600;
                    NextShape = new Shape(2, '1');
                }
                else CreatedShape.ShapeY[0] = CreatedShape.ShapeY[0] + yOffset;
            }
            if (CreatedShape.ShapeX.Count > 1)
            {
                int[] newY = new int[CreatedShape.ShapeX.Count];
                for (int j = 0; j < CreatedShape.ShapeX.Count; j++)
                {
                    newY[j] = CreatedShape.ShapeY[j] + yOffset;
                    if ((newY[j] == Length) || (board[CreatedShape.ShapeX[j], newY[j]]) == CellType.Building)
                    {
                        for (int i = 0; i < CreatedShape.ShapeY.Count; i++)
                        {
                            board[CreatedShape.ShapeX[i], CreatedShape.ShapeY[i]] = CellType.Building;
                        }
                        CreatedShape.ShapeX.Clear();
                        CreatedShape.ShapeY.Clear();
                        CreatedShape = new Shape(Length / 4, NextShape.shape1);
                        NextShape.ShapeX.Clear();
                        NextShape.ShapeY.Clear();
                        timer1.Interval = 600;
                        NextShape = new Shape(2, '1');
                    }
                }
                for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                    CreatedShape.ShapeY[i] = CreatedShape.ShapeY[i] + yOffset;
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
                    if ((CreatedShape.ShapeX[i] + 1 >= Length/2) ||
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
            score = 0;            
            CreatedShape = new Shape(Length / 4, '1');
            NextShape = new Shape(2, '1');
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
                    timer1.Interval = 100;
            }

            if (e.KeyCode == Keys.W)
            {
                if ((CreatedShape.shape1 == 'Z') || (CreatedShape.shape1 == 'S') || (CreatedShape.shape1 == 'L') || (CreatedShape.shape1 == 'J') || (CreatedShape.shape1 == 'T'))
                    if (CheckRotate())
                        CreatedShape.Rotate();
                if (CreatedShape.shape1 == 'I')
                    if (CheckRotate())
                        CreatedShape.SpecialRotate('I');
                if(CreatedShape.shape1=='L')
                    if(CheckRotate())
                        CreatedShape.SpecialRotate('L');
            }

            if (e.KeyCode == Keys.Space)
            {
                    timer1.Interval = 10;
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
                        for (int i = 0; i < Length / 2; i++)
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

        private bool CheckInBuilding()
        {
            return (board[CreatedShape.ShapeX[2] + 1, CreatedShape.ShapeY[2]] != CellType.Building) &&
                   (board[CreatedShape.ShapeX[2] - 1, CreatedShape.ShapeY[2]] != CellType.Building) &&
                   (board[CreatedShape.ShapeX[2], CreatedShape.ShapeY[2] + 1] != CellType.Building) &&
                   (board[CreatedShape.ShapeX[2], CreatedShape.ShapeY[2] - 1] != CellType.Building);
        }

        private bool CheckRotate()
        {
            if (((shape == 'L') || (shape == 'J'))
            {
                testShape = CreateShape.Rotate();
                if () { }
            }
                if((CreateShape.ShapeX[2] < Length / 2 - 1) && (CreateShape.ShapeX[2] > 0) && CheckInBuilding() &&
                (CreateShape.Rotate() ==
                (CreateShape.ShapeY[2] > 0)))
                return true;
            if (CreatedShape.shape1 == 'T')
            {
                for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                {
                    if ((CreatedShape.ShapeY[2] - 1 < 0) && (CreatedShape.ShapeY[i] - 1 < 0))
                        return false;
                }
                if ((CreatedShape.ShapeX[2] < Length/2 - 1) && (CreatedShape.ShapeX[2] > 0) && CheckInBuilding()&&
                    (CreatedShape.ShapeY[2] > 0))
                    return true;
            }
            if (CreatedShape.shape1 == 'I')
            {
                if ((CreatedShape.ShapeX[1] == CreatedShape.ShapeX[3]) && (CreatedShape.ShapeX[3] < Length/2 - 2) &&
                    (CreatedShape.ShapeX[3] != 0))
                    if ((board[CreatedShape.ShapeX[1] + 2, CreatedShape.ShapeY[1]] != CellType.Building) &&
                        (board[CreatedShape.ShapeX[1] - 1, CreatedShape.ShapeY[1]] != CellType.Building) &&
                        (board[CreatedShape.ShapeX[1], CreatedShape.ShapeY[1] - 1] != CellType.Building) &&
                        (board[CreatedShape.ShapeX[1], CreatedShape.ShapeY[1] + 2] != CellType.Building))
                        return true;
                if ((CreatedShape.ShapeY[1] == CreatedShape.ShapeY[3]) && (CreatedShape.ShapeX[1] >= 1))
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
           
                board[i, lineNumber] = CellType.Empty;
            
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
                    timer1.Interval = 600;
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            int cellSize = pictureBox2.Width/4;
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        smallBoard[i, j] = CellType.Empty;
                for (int i = 0; i < NextShape.ShapeX.Count; i++)
                    smallBoard[NextShape.ShapeX[i], NextShape.ShapeY[i]] = CellType.Shape;
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        if (smallBoard[i, j] == CellType.Shape)
                            e.Graphics.FillRectangle(shapeBrush, i*cellSize + 1, j*cellSize + 1, cellSize - 1, cellSize - 1);
            }
        }
}