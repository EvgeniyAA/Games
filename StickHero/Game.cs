using System;
using System.Drawing;
using StickHero.Properties;

namespace StickHero
{
    public class Game
    {
        private const int AngleReduction = 5;
        private const int AnimationSpeed = 10;
        private const int DoublePointDiameter = 20;

        public event ScoreChanged ScoreChanged;
        public GameState GameState;
        private readonly GameObject stick = new GameObject();
        private readonly GameObject hero = new GameObject();
        private readonly GameObject firstPlatform = new GameObject();
        private readonly GameObject secondPlatform = new GameObject();
        private readonly Random rnd = new Random();
        private readonly int pictureBoxHeight;
        private readonly int pictureBoxWidth;
        private double stickLength;
        private int angle;
        private int score;

        public Game(int pictureBoxHeight, int pictureBoxWidth)
        {
            this.pictureBoxHeight = pictureBoxHeight;
            this.pictureBoxWidth = pictureBoxWidth;
            CreateWorld();
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Black, ToRectangle(firstPlatform));
            graphics.FillRectangle(Brushes.Black, ToRectangle(secondPlatform));
            graphics.FillRectangle(Brushes.Red, secondPlatform.Point.X + secondPlatform.Size.X / 2 - DoublePointDiameter / 2,
                secondPlatform.Point.Y, DoublePointDiameter, 4);

            graphics.DrawImage(Resources.Hero, ToRectangle(hero));
            graphics.DrawLine(new Pen(Color.Blue, hero.Size.X / 4), stick.Point, stick.Size);

        }

        private static Rectangle ToRectangle(GameObject platform)
        {
            return new Rectangle(platform.Point.X, platform.Point.Y, platform.Size.X, platform.Size.Y);
        }

        public void Update()
        {
            switch (GameState)
            {
                case GameState.GrowStick:
                    GrowStick();
                    break;
                case GameState.LowerStick:
                    LowerStick();
                    break;
                case GameState.MoveHero:
                    MoveHero();
                    break;
                case GameState.GetResults:
                    GetResults();
                    break;
                case GameState.Fall:
                    Fall();
                    break;
                case GameState.Nothing:
                    break;
                default: throw new ArgumentOutOfRangeException("Oops");
            }
        }

        private void GrowStick()
        {
            if (stick.Point.Y - stick.Size.Y >= pictureBoxWidth - stick.Point.X)
                GameState = GameState.LowerStick;
            else
            {
                stick.Size.Y -= AnimationSpeed;
                stickLength = Math.Sqrt(Math.Pow(stick.Size.Y - stick.Point.Y, 2) +
                                        Math.Pow(stick.Size.X - stick.Point.X, 2));
            }
        }

        private void LowerStick()
        {
            if (angle == 0)
                GameState = GameState.MoveHero;
            else
            {
                angle -= AngleReduction;
                DropStick();
            }
        }

        private void MoveHero()
        {
            if (hero.Point.X >= stick.Size.X - hero.Size.X)
                GameState = GameState.GetResults;
            else
                hero.Point.X += AnimationSpeed;
        }

        private void GetResults()
        {   
            if (stick.Size.X >= secondPlatform.Point.X &&
                stick.Size.X <= secondPlatform.Point.X + secondPlatform.Size.X)
                if (stick.Size.X >= secondPlatform.Point.X + secondPlatform.Size.X / 2 - 10 &&
                    stick.Size.X <= secondPlatform.Point.X + secondPlatform.Size.X / 2 + 10)
                {
                    score += 2;
                    OnScoreChanged();
                    CreateWorld();
                }
                else
                {
                    score++;
                    OnScoreChanged();
                    CreateWorld();
                }
            else
                GameState = GameState.Fall;
        }

        private void Fall()
        {
            if (angle == -90)
            {
                score = 0;
                OnScoreChanged();
                CreateWorld();
            }
            else
            {
                hero.Point.Y += AnimationSpeed;
                angle -= AngleReduction;
                DropStick();
            }
        }

        private void CreateWorld()
        {
            firstPlatform.Point = new Point(0, pictureBoxHeight - pictureBoxHeight / 4);
            firstPlatform.Size = new Point(pictureBoxWidth / 5, pictureBoxHeight);
            secondPlatform.Size = new Point(rnd.Next(pictureBoxWidth / 10, pictureBoxWidth / 5), pictureBoxHeight);
            secondPlatform.Point =
                new Point(
                    rnd.Next(firstPlatform.Point.X + firstPlatform.Size.X + pictureBoxWidth / 5,
                    pictureBoxWidth - secondPlatform.Size.X), firstPlatform.Point.Y);

            hero.Size = new Point(40, 60);
            hero.Point = new Point(firstPlatform.Size.X - hero.Size.X - 5, firstPlatform.Point.Y - hero.Size.Y);

            stick.Point = new Point(hero.Point.X + hero.Size.X + 2, hero.Point.Y + hero.Size.Y + 2);
            stick.Size = new Point(stick.Point.X, stick.Point.Y - hero.Size.Y / 2);
            GameState = GameState.Nothing;
            angle = 90;
        }

        private void DropStick()
        {
            stick.Size.X = (int)(stickLength * Math.Cos(Math.PI / 180 * angle) + stick.Point.X);
            stick.Size.Y = (int)(stick.Point.Y - stickLength * Math.Sin(Math.PI / 180 * angle));
        }

        protected virtual void OnScoreChanged()
        {
            ScoreChanged?.Invoke(score);
        }
    }

    public delegate void ScoreChanged(object sender);
}