using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using Sprint_0.Interfaces;

namespace Sprint_0.Enemies
{
    public class StalfosEnemy : Enemy
    {
        public StalfosEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateStalfosAnimations(spriteSheet), startPos, new EnemyConfig
            {
                CanMove = true,
                CanAttack = true,
                CanCrouch = true,
                DropItemOnDeath = true,
                XPReward = 30,
                BoundingBoxSize = new Rectangle(0, 0, 13, 30),
                UseGravity = true
            })
        {
            this.SpriteSheet = spriteSheet;
            this.IsGrounded = false;
            ChangeState(new StalfosFallState());
        }
        protected override IEnemyState GetDefaultState()
        {
            return new StalfosFallState();
        }
    }
}
