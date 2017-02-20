using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StickHero
{
    public partial class GameForm : Form
    {
        private readonly Game game;

        public GameForm()
        {
            InitializeComponent();

            Text = "Stick Hero";
            Location = new Point(0, 0);
            ClientSize = new Size(800, 800);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MinimizeBox = false;
            MaximizeBox = false;
            pictureBox.Size = new Size(800, 800);

            ScoreLabel.Location = new Point(pictureBox.Width / 2, pictureBox.Height / 5);
            ScoreLabel.BackColor = Color.Transparent;
            ScoreLabel.Parent = pictureBox;
            ScoreLabel.Text = "0";

            game = new Game(pictureBox.Size);
            game.ScoreChanged += score => ScoreLabel.Text = score.ToString();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            game.Draw(e.Graphics);
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                game.PressSpace();
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                game.UnPressSpace();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            game.Update();
            pictureBox.Refresh();
        }
    }
}
