using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private const int LengthHeight = 20;
        private const int LengthWidth = LengthHeight/2;
        private readonly int yOffset;
        private int xOffset;
        private readonly CellType[,] board = new CellType[LengthWidth, LengthHeight];
        private readonly SolidBrush shapeBrush = new SolidBrush(Color.Green);
        private bool isBuildingConstructed;
        private bool canMove = true;
        private bool isPause;
        private int score;
        public Shape CreatedShape;
        public Shape NextShape;
        public Shape TestShape;

        public Form1()
        {
            yOffset = 1;
            xOffset = 0;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(LengthWidth * 20 + 160, LengthHeight * 20 + 50);
            pictureBox1.Height = LengthHeight*20 + 1;
            pictureBox1.Width = LengthWidth * 20 + 1;
            pictureBox2.Location = new Point(LengthWidth * 22, LengthHeight*2);
            pictureBox2.Size = new Size(LengthWidth * 8, LengthHeight*4);
            Restart();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics draw = e.Graphics;
            pictureBox1.CreateGraphics();
            Pen linesPen = new Pen(Color.Gray);

            int cellSize = pictureBox1.Width/10;

            for (int i = 0; i <= pictureBox1.Width; i++)
            {
                draw.DrawLine(linesPen, cellSize*i, 0, cellSize*i, pictureBox1.Height);
                draw.DrawLine(linesPen, 0, i*cellSize, pictureBox1.Width, i*cellSize);
            }

            for (int j = 0; j < LengthHeight; j++)
            {
                for (int i = 0; i < LengthWidth; i++)
                {
                    if (board[i, j] != CellType.Building)
                        board[i, j] = CellType.Empty;
                }
            }

            for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                board[CreatedShape.ShapeX[i], CreatedShape.ShapeY[i]] = CellType.Shape;

            for (int i = 0; i < LengthWidth; i++)
                for (int j = 0; j < LengthHeight; j++)
                    if ((board[i, j] == CellType.Shape) || (board[i, j] == CellType.Building))
                        e.Graphics.FillRectangle(shapeBrush, i*cellSize + 1, j*cellSize + 1, cellSize - 1, cellSize - 1);
        }

        private void CountScore()
        {
            int k = 0;

            for (int j = LengthHeight - 1; j > 0;)
            {
                int countOfCells = 0;
                for (int i = 0; i < LengthWidth; i++)
                {
                    if (board[i, j] == CellType.Building)
                        countOfCells++;
                }
                if (countOfCells == LengthWidth)
                {
                    DropDown(j);
                    k++;
                }
                else j--;
            }
            if (k == 1)
                score += 100;
            if (k == 2)
                score += 300;
            if (k == 3)
                score += 700;
            if (k == 4)
                score += 1500;
            label1.Text = "Score:" + score;
        }

        private void IsGameOver()
        {
            bool isGameOver = false;
            for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                if ((board[CreatedShape.ShapeX[i], CreatedShape.ShapeY[i]] == CellType.Building) &&
                    (CreatedShape.ShapeY[i] < 5)) isGameOver = true;
            if (isGameOver)
            {
                timer1.Stop();
                MessageBox.Show("Lose! Score=" + score);
                CreatedShape.ShapeX.Clear();
                CreatedShape.ShapeY.Clear();
                NextShape.ShapeY.Clear();
                NextShape.ShapeY.Clear();
                Restart();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isBuildingConstructed)
                timer1.Interval = 600;
            Moving();
            CountScore();
            IsGameOver();
            pictureBox1.Refresh();
            pictureBox2.Refresh();
        }

        private void Moving()
        {
            isBuildingConstructed = false;
            int[] newY = new int[CreatedShape.ShapeX.Count];
            for (int j = 0; j < CreatedShape.ShapeX.Count; j++)
            {
                newY[j] = CreatedShape.ShapeY[j] + yOffset;
                if ((newY[j] == LengthHeight) || board[CreatedShape.ShapeX[j], newY[j]] == CellType.Building)
                {
                    for (int i = 0; i < CreatedShape.ShapeY.Count; i++)
                        board[CreatedShape.ShapeX[i], CreatedShape.ShapeY[i]] = CellType.Building;
                    CreatedShape.ShapeX.Clear();
                    CreatedShape.ShapeY.Clear();
                    CreatedShape = new Shape(LengthHeight/4, NextShape.Shape1);
                    NextShape.ShapeX.Clear();
                    NextShape.ShapeY.Clear();
                    timer1.Interval = 600;
                    NextShape = new Shape(2);
                    break;
                }
            }
            for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                CreatedShape.ShapeY[i] += yOffset;

        }

        private void MovingToward()
        {           
                for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
                {
                    if ((CreatedShape.ShapeX[i] + xOffset < 0)|| (CreatedShape.ShapeX[i] + xOffset >= LengthWidth) ||
                        (board[CreatedShape.ShapeX[i] + xOffset, CreatedShape.ShapeY[i]] == CellType.Building))
                        canMove = false;
                }
                if (canMove)
                    for (int i = 0; i < CreatedShape.ShapeX.Count; i++)                    
                        CreatedShape.ShapeX[i] += xOffset;                   
        }

        private void Restart()
        {
            score = 0;
            CreatedShape = new Shape(LengthWidth/2);
            NextShape = new Shape(2);
            for (int i = 0; i < LengthWidth; i++)
                for (int j = 0; j < LengthHeight; j++)
                    board[i, j] = CellType.Empty;
            timer1.Start();
        }

        private void CreateTestShape()
        {
            TestShape = new Shape(CreatedShape.ShapeX[2], CreatedShape.Shape1)
            {
                RotateCount = CreatedShape.RotateCount,
                position = CreatedShape.position
            };
            for (int i = 0; i < CreatedShape.ShapeX.Count; i++)
            {
                TestShape.ShapeY[i] = CreatedShape.ShapeY[i];
                TestShape.ShapeX[i] = CreatedShape.ShapeX[i];
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.A) || (e.KeyCode == Keys.Left))
            {
                xOffset = -1;
                MovingToward();
            }
            if ((e.KeyCode == Keys.D) || (e.KeyCode == Keys.Right))
            {
                xOffset = 1;
                MovingToward();
            }
            xOffset = 0;
            if (e.KeyCode == Keys.S)
                timer1.Interval = 100;

            if (e.KeyCode == Keys.W && (CreatedShape.Shape1 != '.') && (CreatedShape.Shape1 != 'O'))
            {
                CreateTestShape();
                if ((CreatedShape.Shape1 != 'I') && CanRotate())
                    CreatedShape.Rotate();
                if (CreatedShape.Shape1 == 'I' && CanRotate())
                    CreatedShape.LineRotate();
            }

            if (e.KeyCode == Keys.Space)
                timer1.Interval = 10;
            if (e.KeyCode == Keys.Escape)
            {
                if (isPause)
                {
                    timer1.Start();
                    isPause = false;
                }
                else
                {
                    timer1.Stop();
                    isPause = true;
                    for (int j = 0; j < LengthHeight; j++)
                    {
                        Console.WriteLine();
                        for (int i = 0; i < LengthWidth; i++)
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

        private bool CanRotate()
        {
            if (TestShape.Shape1 == 'I')
                TestShape.LineRotate();
            else TestShape.Rotate();
            for (int i = 0; i < TestShape.ShapeX.Count; i++)
            {
                if ((TestShape.ShapeX[i] > LengthHeight/2 - 1) || (TestShape.ShapeX[i] < 0) || (TestShape.ShapeY[i] < 0) ||
                    (TestShape.ShapeY[i] >= LengthHeight))
                    return false;
                if (board[TestShape.ShapeX[i], TestShape.ShapeY[i]] == CellType.Building)
                    return false;
            }
            TestShape.ShapeX.Clear();
            TestShape.ShapeY.Clear();
            return true;
        }

        private void DropDown(int lineNumber)
        {
            for (int i = 0; i < LengthWidth; i++)
                board[i, lineNumber] = CellType.Empty;

            for (int i = 0; i < LengthWidth; i++)
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
            label1.Left = LengthWidth * 20 + 20;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
                timer1.Interval = 600;
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            int cellSize = pictureBox2.Width/4;
            for (int i = 0; i < NextShape.ShapeX.Count; i++)
                e.Graphics.FillRectangle(shapeBrush, NextShape.ShapeX[i]*cellSize, NextShape.ShapeY[i]*cellSize,
                    cellSize - 1, cellSize - 1);
        }
    }
}