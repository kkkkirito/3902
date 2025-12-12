using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using Sprint_0.Interfaces;
using Sprint_0.Managers;
using System;

namespace Sprint_0.Enemies
{
    public class TopDownBotEnemy : Enemy
    {
        public TopDownBotEnemy(Texture2D spriteSheet, Vector2 startPos, IAudioManager audio)
            : base(SpriteFactory.CreateTopDownBotAnimations(spriteSheet), startPos, new EnemyConfig
        {
                UseGravity = false,
                BoundingBoxSize = new Rectangle(1, 1, 16, 13)
            }, audio)
        {
            this.SetSpriteSheet(spriteSheet);
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
