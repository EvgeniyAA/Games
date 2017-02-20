using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using StickHero.Properties;

namespace StickHero
{
    public class Game
    {
        private const int AngleReduction = 5;
        private const int AnimationSpeed = 10;
        private const int DoublePointDiameter = 20;

        private Size gameField;

        private GameState gameState;
        private Rectangle hero = new Rectangle(0, 0, 0, 0);
        private Rectangle firstPlatform = new Rectangle(0, 0, 0, 0);
        private Rectangle secondPlatform = new Rectangle(0, 0, 0, 0);
        private readonly Segment stick = new Segment();
        private readonly Random rnd = new Random();
       
        private int score;
        public event Action<int> ScoreChanged = delegate { };

        public Game(Size gameField)
        {
            this.gameField = gameField;
            StartNewRound();
        }

        public void Draw(Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle gameFieldRectangle = new Rectangle(Point.Empty, gameField);
            graphics.FillRectangle(new LinearGradientBrush(gameFieldRectangle, Color.LightCyan, Color.Aquamarine, 45F), gameFieldRectangle);

            graphics.FillRectangle(Brushes.Black, firstPlatform);
            graphics.FillRectangle(Brushes.Black, secondPlatform);
            graphics.FillRectangle(Brushes.Red, secondPlatform.X + secondPlatform.Size.Width / 2 - DoublePointDiameter / 2,
                secondPlatform.Y, DoublePointDiameter, 4);

            graphics.DrawImage(Resources.Hero, hero);

            int stickWidth = hero.Size.Width / 4;
            graphics.DrawLine(new Pen(Color.Blue, stickWidth), stick.PointA, stick.PointB);
        }

        public void Update()
        {
            switch (gameState)
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
                case GameState.ProcessResults:
                    ProcessResults();
                    break;
                case GameState.Fall:
                    Fall();
                    break;
                case GameState.Nothing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void PressSpace()
        {
            if (gameState == GameState.Nothing)
                gameState = GameState.GrowStick;
        }

        public void UnPressSpace()
        {
            if (gameState == GameState.GrowStick)
                gameState = GameState.LowerStick;
        }

        private void GrowStick()
        {
            bool isStickTooLong = stick.Length >= gameField.Width - stick.PointA.X;
            if (isStickTooLong)
                gameState = GameState.LowerStick;
            else
                stick.Grow(AnimationSpeed);
        }

        private void LowerStick()
        {
            if (stick.Angle == 0)
                gameState = GameState.MoveHero;
            else
                stick.Rotate(AngleReduction);
        }

        private void MoveHero()
        {
            bool didHeroReachStickEnd = hero.X >= stick.PointB.X - hero.Size.Width;
            if (didHeroReachStickEnd)
                gameState = GameState.ProcessResults;
            else
                hero.X += AnimationSpeed;
        }

        private void ProcessResults()
        {
            int gainedScore = CalculateScore();

            if (gainedScore > 0)
            {
                SetScore(score + gainedScore);
                StartNewRound();
            }
            else
                gameState = GameState.Fall;
        }

        private int CalculateScore()
        {
            bool didStickReachPlatform = stick.PointB.X >= secondPlatform.X &&
                                         stick.PointB.X <= secondPlatform.X + secondPlatform.Size.Width;

            bool didStickReachPlatformBonusArea = 
                stick.PointB.X >= secondPlatform.X + secondPlatform.Size.Width / 2 - DoublePointDiameter / 2 &&
                stick.PointB.X <= secondPlatform.X + secondPlatform.Size.Width / 2 + DoublePointDiameter / 2;

            if (didStickReachPlatformBonusArea)
                return 2;

            if (didStickReachPlatform)
                return 1;

            return 0;
        }

        private void Fall()
        {
            if (stick.Angle == -90)
            {
                SetScore(0);
                StartNewRound();
            }
            else
            {
                hero.Y += AnimationSpeed;
                stick.Rotate(AngleReduction);
            }
        }

        private void StartNewRound()
        {
            firstPlatform.Location = new Point(0, gameField.Height - gameField.Height / 4);
            firstPlatform.Size = new Size(gameField.Width / 5, gameField.Height);

            hero.Size = new Size(40, 60);
            hero.Location = new Point(firstPlatform.Size.Width - hero.Size.Width - 5, firstPlatform.Y - hero.Size.Height);

            stick.PointA = new Point(hero.X + hero.Size.Width + 2, hero.Y + hero.Size.Height + 2);
            stick.PointB = new Point(stick.PointA.X, stick.PointA.Y - hero.Size.Height / 2);
            stick.Angle = 90;
            //stick.Length = Math.Sqrt(Math.Pow(stick.PointB.Y - stick.PointA.Y, 2) +
            //                   Math.Pow(stick.PointB.X - stick.PointA.X, 2));
            int secondPlatformWidth = rnd.Next(gameField.Width / 10, gameField.Width / 5);
            int secondPlatformX = rnd.Next(firstPlatform.X + firstPlatform.Size.Width + gameField.Width / 5,
                                           gameField.Width - secondPlatform.Size.Width);
            secondPlatform.Size = new Size(secondPlatformWidth, gameField.Height);
            secondPlatform.Location = new Point(secondPlatformX, firstPlatform.Y);

            gameState = GameState.Nothing;
        }

        private void SetScore(int newScore)
        {
            score = newScore;
            ScoreChanged(score);
        }
    }
}