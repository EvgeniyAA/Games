using System;
using System.Drawing;
using System.Windows.Forms;

namespace AirForce
{
    public partial class Form1 : Form
    {
        // TODO: Moving some things to Game

        public Direction Direction;
        private Game game;
        private bool isFire;
        private int countOfTicks;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(1100, 600);
            pictureBox1.Width = 1100;
            pictureBox1.Height = 580;
            game = new Game(pictureBox1.Width,pictureBox1.Height);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            game.Draw(e.Graphics);            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                Direction = Direction.Up;
            if (e.KeyCode == Keys.S)
                Direction = Direction.Down;
            if (e.KeyCode == Keys.Space)
                isFire = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            countOfTicks++;
            game.Update(Direction, isFire, countOfTicks);
            if (game.IsGameOver())
            {
                timer1.Stop();
                MessageBox.Show("Game Over! Score="+game.Score);
                
            }
            pictureBox1.Refresh();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.W) || (e.KeyCode==Keys.S))
                Direction = Direction.None;
            if (e.KeyCode == Keys.Space)
                isFire = false;
        }
    }
}
