using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.Enemies
{
    public class TopDownBotEnemy : Enemy
    {
        public TopDownBotEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateTopDownBotAnimations(spriteSheet), startPos, new EnemyConfig
        {
                UseGravity = false,
                BoundingBoxSize = new Rectangle(0, 0, 18, 15)
            })
        {
            this.SpriteSheet = spriteSheet;
            ChangeState(GetRandomTopDownMoveState());
        }

        private IEnemyState GetRandomTopDownMoveState()
        {
            double roll = Random.Shared.NextDouble();
            if (roll < 0.25) return new TopDownMoveUpState();
            if (roll < 0.5) return new TopDownMoveDownState();
            if (roll < 0.75) return new TopDownMoveLeftState();
            return new TopDownMoveRightState();
        }
    }

}
