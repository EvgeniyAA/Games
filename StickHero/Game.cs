using System;
using System.Drawing;
using StickHero.Properties;

namespace StickHero
{
    public class Game
    {
        private const int AngleReduction = 5;
        private const int AnimationSpeed = 10;
        private const int SecPlatformCenter = 20;

        public event ScoreChanged ScoreChanged;
        public GameState GameState;
        private readonly GameObject stick = new GameObject();
        private readonly GameObject hero = new GameObject();
        private readonly GameObject firstPlatform = new GameObject();
        private readonly GameObject secondPlatform = new GameObject();
        private readonly Random rnd = new Random();
        private readonly int height;
        private readonly int width;
        private double stickLength;
        private int angle;
        private int score;

        public Game(int height, int width)
        {
            this.height = height;
            this.width = width;
            CreateWorld();
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Black, ToRectangle(firstPlatform));
            graphics.FillRectangle(Brushes.Black, ToRectangle(secondPlatform));
            graphics.FillRectangle(Brushes.Red, secondPlatform.ObjPoint.X + secondPlatform.ObjSize.X / 2 - SecPlatformCenter / 2,
                secondPlatform.ObjPoint.Y, SecPlatformCenter, 4);

            graphics.DrawImage(Resources.Hero, ToRectangle(hero));
            graphics.DrawLine(new Pen(Color.Blue, hero.ObjSize.X / 4), stick.ObjPoint, stick.ObjSize);

        }

        private static Rectangle ToRectangle(GameObject platform)
        {
            return new Rectangle(platform.ObjPoint.X, platform.ObjPoint.Y, platform.ObjSize.X, platform.ObjSize.Y);
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
            }
        }

        private void GrowStick()
        {
            if (stick.ObjPoint.Y - stick.ObjSize.Y >= width - stick.ObjPoint.X)
                GameState = GameState.LowerStick;
            else
            {
                stick.ObjSize.Y -= AnimationSpeed;
                stickLength = Math.Sqrt(Math.Pow(stick.ObjSize.Y - stick.ObjPoint.Y, 2) +
                                        Math.Pow(stick.ObjSize.X - stick.ObjPoint.X, 2));
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
            if (hero.ObjPoint.X >= stick.ObjSize.X - hero.ObjSize.X)
                GameState = GameState.GetResults;
            else
                hero.ObjPoint.X += AnimationSpeed;
        }

        private void GetResults()
        {   
            if (stick.ObjSize.X >= secondPlatform.ObjPoint.X &&
                stick.ObjSize.X <= secondPlatform.ObjPoint.X + secondPlatform.ObjSize.X)
                if (stick.ObjSize.X >= secondPlatform.ObjPoint.X + secondPlatform.ObjSize.X / 2 - 10 &&
                    stick.ObjSize.X <= secondPlatform.ObjPoint.X + secondPlatform.ObjSize.X / 2 + 10)
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
                hero.ObjPoint.Y += AnimationSpeed;
                angle -= AngleReduction;
                DropStick();
            }
        }

        private void CreateWorld()
        {
            firstPlatform.ObjPoint = new Point(0, height - height / 4);
            firstPlatform.ObjSize = new Point(width / 5, height);
            secondPlatform.ObjSize = new Point(rnd.Next(width / 10, width / 5), height);
            secondPlatform.ObjPoint =
                new Point(
                    rnd.Next(firstPlatform.ObjPoint.X + firstPlatform.ObjSize.X + width / 5,
                    width - secondPlatform.ObjSize.X), firstPlatform.ObjPoint.Y);

            hero.ObjSize = new Point(40, 60);
            hero.ObjPoint = new Point(firstPlatform.ObjSize.X - hero.ObjSize.X - 5, firstPlatform.ObjPoint.Y - hero.ObjSize.Y);

            stick.ObjPoint = new Point(hero.ObjPoint.X + hero.ObjSize.X + 2, hero.ObjPoint.Y + hero.ObjSize.Y + 2);
            stick.ObjSize = new Point(stick.ObjPoint.X, stick.ObjPoint.Y - hero.ObjSize.Y / 2);
            GameState = GameState.Nothing;
            angle = 90;
        }

        private void DropStick()
        {
            stick.ObjSize.X = (int)(stickLength * Math.Cos(Math.PI / 180 * angle) + stick.ObjPoint.X);
            stick.ObjSize.Y = (int)(stick.ObjPoint.Y - stickLength * Math.Sin(Math.PI / 180 * angle));
        }

        protected virtual void OnScoreChanged()
        {
            ScoreChanged?.Invoke(score);
        }
    }

    public delegate void ScoreChanged(object sender);
}