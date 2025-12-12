using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.Enemies
{
    public class BubbleEnemy : Enemy
    {
        public BubbleEnemy(Texture2D spriteSheet, Vector2 startPos, IAudioManager audio)
            : base(SpriteFactory.CreateBubbleAnimations(spriteSheet), startPos, new EnemyConfig
                {
                    MaxHealth = 20,
                    CanMove = true,
                    XPReward = 50,
                    BoundingBoxSize = new Rectangle(0, 0, 15, 14),
                    UseGravity = false
                }, audio)
        {
            this.SetSpriteSheet(spriteSheet);

            ChangeState(new BubbleState());
        }
        protected override IEnemyState GetDefaultState()
        {
            return new BubbleState();
        }
    }
}
