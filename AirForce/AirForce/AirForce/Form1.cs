using System;
using System.Drawing;
using System.Windows.Forms;

namespace AirForce
{
    public partial class Form1 : Form
    {
        private Game game;
        private int countOfTicks;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {           
            pictureBox1.Location = new Point(0,0);
            pictureBox1.Width = ClientSize.Width;
            pictureBox1.Height = ClientSize.Height-100;
            hpLabel.Location = new Point(0, pictureBox1.Height);
            HpBar.Location= new Point(0,hpLabel.Height+pictureBox1.Height);
            InfoLabel.Location= new Point(hpLabel.Width+10, pictureBox1.Height);
            textBoxInfo.Location = new Point(hpLabel.Width+10,pictureBox1.Height+InfoLabel.Height);
            textBoxInfo.SelectionStart = textBoxInfo.Text.Length;
            textBoxInfo.ScrollToCaret();
            game = new Game(pictureBox1.Width,pictureBox1.Height);
            game.Restart();
        }

        private void InfoTextBoxUpdate()
        {
            string info="";
            info += "Level "+game.GameLevel.LevelNumber + Environment.NewLine;
            info += "Need to kill " + (game.GameLevel.CountToKill-game.GameLevel.Killed + " "+game.GameLevel.TypeToKill +
                    " to complete the level  ");
            textBoxInfo.Text = info;
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.BackColor = Color.FromArgb(countOfTicks % 255, countOfTicks % 255, countOfTicks % 255);
            game.Draw(e.Graphics);            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W||e.KeyCode==Keys.Up)
                game.PressUp();
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
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
                HpBar.Value = 0;
                timer1.Stop();
                MessageBox.Show("Game Over on level - " + game.GameLevel.LevelNumber);
                game.Restart();
                timer1.Start();
            }
            if (game.Objects[0].Hp>=0)
                HpBar.Value = game.Objects[0].Hp;
            InfoTextBoxUpdate();

            pictureBox1.Refresh();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.W) || (e.KeyCode==Keys.S) || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                game.UnPress();
            if (e.KeyCode == Keys.Space)
                game.StopAttack();
        }
    }
}
