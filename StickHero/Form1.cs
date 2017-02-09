using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StickHero
{
    public partial class Form1 : Form
    {
        private Game game;
        private Rectangle rc;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(800, 800);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MinimizeBox = false;
            MaximizeBox = false;
            Text = "Stick Hero";
            Location= new Point(0,0);
            pictureBox.Size = new Size(800, 800);
            game = new Game(pictureBox.Height, pictureBox.Width);
            label1.Location = new Point(pictureBox.Width/2,pictureBox.Height/5);
            label1.Parent = pictureBox;
            label1.Text = "0";
            label1.BackColor = Color.Transparent;
            game.ScoreChanged += score => label1.Text = score.ToString();
            rc= new Rectangle(pictureBox.Location, pictureBox.Size);
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(rc, Color.LightCyan, Color.Aquamarine, 45F))
            {
                e.Graphics.FillRectangle(brush, rc);
            }
            game.Draw(e.Graphics);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && game.GameState == GameState.Nothing)
                game.GameState = GameState.GrowStick;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && game.GameState == GameState.GrowStick)
                game.GameState = GameState.LowerStick;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            game.Update();
            pictureBox.Refresh();
        }
    }
}
