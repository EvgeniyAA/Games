using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace minesweeper
{
    public partial class Form1 : Form
    {
        public bool[,] Used = new bool[NumberOfCells, NumberOfCells];
        public const int NumberOfCells = 9;
        public const int ButtonSize = 30;
        public int BombsToWin = 10;
        public int Bombs = 10;
        public int[,] Cells = new int[NumberOfCells, NumberOfCells];
        public int CursorX;
        public int CursorY;

        public Brush[] Brushes = new Brush[]
        {
            System.Drawing.Brushes.Black,
            System.Drawing.Brushes.Blue,
            System.Drawing.Brushes.Green,
            System.Drawing.Brushes.Red,
            System.Drawing.Brushes.DarkRed,
            System.Drawing.Brushes.DarkRed,
            System.Drawing.Brushes.DarkRed,
            System.Drawing.Brushes.DarkRed,
            System.Drawing.Brushes.DarkRed
        };

        private readonly Random rnd = new Random();
        private readonly Button[,] button = new Button[NumberOfCells, NumberOfCells];

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics draw = e.Graphics;
            Pen linesPen = new Pen(Color.Black);
            Font font = new Font(new FontFamily(GenericFontFamilies.SansSerif), 14);
            StringFormat stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Width = ButtonSize*NumberOfCells + 1;
            pictureBox1.Height = ButtonSize*NumberOfCells + 1;
            int cellWidth = pictureBox1.Width/NumberOfCells;
            int cellHeight = pictureBox1.Height/NumberOfCells;
            for (int i = 0; i <= NumberOfCells; i++)
                draw.DrawLine(linesPen, i*cellHeight, 0, i*cellHeight, pictureBox1.Height);
            for (int j = 0; j <= NumberOfCells; j++)
                draw.DrawLine(linesPen, 0, j*cellWidth, pictureBox1.Width, j*cellWidth);

            for (int i = 0; i < NumberOfCells; i++)
            {
                draw.DrawLine(linesPen, i*cellHeight, 0, i*cellHeight, pictureBox1.Height);
                for (int j = 0; j < NumberOfCells; j++)
                {
                    draw.DrawLine(linesPen, 0, j*cellWidth, pictureBox1.Width, j*cellWidth);
                    if (Cells[i, j] > 0)
                        draw.DrawString(Cells[i, j].ToString(), font, Brushes[Cells[i, j]],
                            new RectangleF(j*cellWidth, i*cellHeight, cellWidth, cellHeight), stringFormat);
                }
            }
        }


        public Form1()
        {
            InitializeComponent();
            ClientSize = new Size(ButtonSize*NumberOfCells + 1, ButtonSize*NumberOfCells + 25);
            for (int i = 0; i < NumberOfCells; i++)
            {
                for (int j = 0; j < NumberOfCells; j++)
                {
                    button[i, j] = new Button
                    {
                        Location = new Point(j*ButtonSize + 1, i*ButtonSize + 1),
                        Size = new Size(ButtonSize - 1, ButtonSize - 1)
                    };
                    button[i, j].MouseClick += button_MouseClick;
                    button[i, j].MouseDown += button_MouseDown;
                    button[i, j].Tag = i;
                    button[i, j].ImageIndex = j;
                    Controls.Add(button[i, j]);
                    button[i, j].BringToFront();
                    button[i, j].BackColor = default(Color);
                }
            }
            Restart();
        }

        private void Restart()
        {
            Bombs = 10;
            BombsToWin = 10;
            BombsLabel.Text = "Bombs:" + Bombs;
            for (int i = 0; i < NumberOfCells; i++)
            {
                for (int j = 0; j < NumberOfCells; j++)
                {
                    Cells[i, j] = 0;
                    button[i, j].Show();
                    button[i, j].BackColor = default(Color);
                    Used[i, j] = false;
                }
            }
            int k = 0;
            while (k < 10)
            {
                int i = rnd.Next(9);
                int j = rnd.Next(9);
                if (Cells[i, j] == 0)
                {
                    Cells[i, j] = -1;
                    k++;
                }
            }
            for (int i = 0; i < NumberOfCells; i++)
            {
                for (int j = 0; j < NumberOfCells; j++)
                {
                    for (int l = -1; l <= 1; l++)
                    {
                        for (int m = -1; m <= 1; m++)
                        {
                            if (Cells[i, j] == -1)
                            {
                                if ((i + l >= 0) && (i + l < NumberOfCells) && (j + m >= 0) && (j + m < NumberOfCells) &&
                                    (Cells[i + l, j + m] != -1))
                                    Cells[i + l, j + m]++;
                            }
                        }
                    }
                }
            }
            pictureBox1.Refresh();
        }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            int i = Convert.ToInt16((sender as Button).Tag);
            int j = (sender as Button).ImageIndex;
            if ((e.Button == MouseButtons.Right) && (button[i, j].BackColor == Color.Blue))
            {
                Bombs++;
                if (Bombs >= 0)
                    BombsLabel.Text = "Bombs:" + Bombs;
                if (Cells[i, j] == -1)
                    BombsToWin++;
                button[i, j].BackColor = default(Color);
            }
            else if (e.Button == MouseButtons.Right)
            {
                Bombs--;
                if (Bombs >= 0)
                    BombsLabel.Text = "Bombs:" + Bombs;
                if (Cells[i, j] == -1)
                    BombsToWin--;
                button[i, j].BackColor = Color.Blue;
            }
            if ((BombsToWin == 0) && (Bombs == 0))
            {
                MessageBox.Show("You Win!");
                Restart();
            }
        }

        public void Dfs(int[,] x, int i, int j)
        {
            if (Used[i, j] == false)
            {
                Used[i, j] = true;
                if (x[i, j] == 0)
                {
                    if (button[i, j].BackColor == Color.Blue)
                        Bombs++;
                    if (Bombs >= 0) BombsLabel.Text = "Bombs:" + Bombs;
                    button[i, j].Hide();
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            if ((i + k >= 0) && (i + k < NumberOfCells) && (j + l >= 0) && (j + l < NumberOfCells))
                                Dfs(x, i + k, j + l);
                        }
                    }
                }
                if (x[i, j] > 0)
                {
                    if (button[i, j].BackColor == Color.Blue)
                        Bombs++;
                    if (Bombs >= 0) BombsLabel.Text = "Bombs:" + Bombs;
                    button[i, j].Hide();
                }
                if (x[i, j] == -1)
                {
                    for (int k = 0; k < NumberOfCells; k++)
                    {
                        for (int l = 0; l < NumberOfCells; l++)
                        {
                            if (Cells[k, l] == -1)
                                button[k, l].BackColor = Color.Red;
                        }
                    }

                    MessageBox.Show("You Lose!");
                    Restart();
                }
            }
        }

        private void button_MouseClick(object sender, MouseEventArgs e)
        {
            int i = Convert.ToInt16((sender as Button).Tag);
            int j = (sender as Button).ImageIndex;
            if (!(button[i, j].BackColor == Color.Blue))
                Dfs(Cells, i, j);
        }


        private void BombsLabel_TextChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            CursorX = Cursor.Position.X;
            CursorY = Cursor.Position.Y;
            Console.Write(CursorX + " " + CursorY);
        }
    }
}