using System;
using System.Drawing;
using System.Windows.Forms;

namespace AirForce
{
    public partial class Form1 : Form
    {
        public Direction Direction;
        private Game game;
        private int countOfTicks;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(1100, 600);
            pictureBox1.BackColor= Color.AliceBlue;
            pictureBox1.Width = 1100;
            pictureBox1.Height = 580;
            game = new Game(pictureBox1.Width,pictureBox1.Height);
            game.Restart();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.BackColor = Color.FromArgb(countOfTicks % 255, countOfTicks % 255, countOfTicks % 255);
            game.Draw(e.Graphics);            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                game.PressUp();
            if (e.KeyCode == Keys.S)
                game.PressDown();
            if (e.KeyCode == Keys.Space)
                game.StartAttack();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            countOfTicks++;
            game.Update(countOfTicks);
            if (game.IsGameOver())
            {
                timer1.Stop();
                MessageBox.Show("Game Over! Score="+game.Score);
                game.Restart();
                timer1.Start();
            }
            pictureBox1.Refresh();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.W) || (e.KeyCode==Keys.S))
                game.UnPress();
            if (e.KeyCode == Keys.Space)
                game.StopAttack();
        }
    }
}
