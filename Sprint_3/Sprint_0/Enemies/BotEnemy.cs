using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using Sprint_0.Managers;

namespace Sprint_0.Enemies
{
    public class BotEnemy : Enemy
    {
        public BotEnemy(Texture2D spriteSheet, Vector2 startPos, IAudioManager audio)
            : base(SpriteFactory.CreateBotAnimations(spriteSheet), startPos, new EnemyConfig
                {
                    CanMove = true,
                    CanJump = true,
                    XPReward = 2,
                    BoundingBoxSize = new Rectangle(0, 0, 17, 17),
                    UseGravity = true
                }, audio)
        {
            this.SetSpriteSheet(spriteSheet);
            ChangeState(new IdleState(audio));

        }
    }
}
