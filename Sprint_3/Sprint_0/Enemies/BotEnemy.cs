using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;

namespace Sprint_0.Enemies
{
    public class BotEnemy : Enemy
    {
        public BotEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateBotAnimations(spriteSheet), startPos, new EnemyConfig
                {
                    CanMove = true,
                    CanJump = true,
                    XPReward = 2,
                    BoundingBoxSize = new Rectangle(0, 0, 17, 17),
                    UseGravity = true
                })
        {
            this.SpriteSheet = spriteSheet;
            ChangeState(new IdleState());
        }
    }
}
