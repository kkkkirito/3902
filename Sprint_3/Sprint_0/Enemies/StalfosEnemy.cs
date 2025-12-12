using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.Enemies
{
    public class StalfosEnemy : Enemy
    {
        private readonly IAudioManager _audio;
        public StalfosEnemy(Texture2D spriteSheet, Vector2 startPos, IAudioManager audio)
            : base(SpriteFactory.CreateStalfosAnimations(spriteSheet), startPos, new EnemyConfig
            {
                MaxHealth = 20,
                CanMove = true,
                CanAttack = true,
                CanCrouch = true,
                DropItemOnDeath = true,
                XPReward = 30,
                BoundingBoxSize = new Rectangle(0, 0, 13, 30),
                UseGravity = true
            }, audio)
        {
            this.SetSpriteSheet(spriteSheet);
            this.IsGrounded = false;
            _audio = audio;
            ChangeState(new StalfosFallState(_audio));
            
        }
        protected override IEnemyState GetDefaultState()
        {
            return new StalfosFallState(_audio);
        }
    }
}
