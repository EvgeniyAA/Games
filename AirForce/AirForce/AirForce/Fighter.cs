using System;
using System.Drawing;

namespace AirForce
{
    public class Fighter : GameObject
    {
        private readonly SolidBrush enemyFighterBrush;
        private static readonly Image FighterImage = Properties.Resources.Fighter;
        public Fighter(Point fighterPoint) : base(fighterPoint)
        {
            GameObjectSize.X = 30;
            GameObjectSize.Y = 20;
            Speed = 2;
            Hp = 5;
            //enemyFighterBrush = new SolidBrush(Color.LimeGreen);
            ObjectType = ObjectType.EnemyPlane;
            ObjectDirection = Direction.None;
        }

        public override void Draw(Graphics graphics)
        {
            //graphics.FillRectangle(enemyFighterBrush,GameObjectPoint.X,GameObjectPoint.Y,GameObjectSize.X, GameObjectSize.Y);
            graphics.DrawImageUnscaledAndClipped(FighterImage, new Rectangle(GameObjectPoint.X, GameObjectPoint.Y, GameObjectSize.X, GameObjectSize.Y));
        }

        public override void Move()
        {
            base.Move();
            Dodge();
        }

        public void Dodge()
        {
            if (ObjectDirection == Direction.Up)
                GameObjectPoint.Y--;
            if (ObjectDirection == Direction.Down)
                GameObjectPoint.Y++;
        }
    }
}
