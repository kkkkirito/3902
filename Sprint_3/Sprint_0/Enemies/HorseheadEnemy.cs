using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.Enemies
{
    public class HorseHeadEnemy : Enemy
    {
        public HorseHeadEnemy(Texture2D spriteSheet, Vector2 startPos, IAudioManager audio)
            : base(SpriteFactory.CreateHorseHeadAnimations(spriteSheet), startPos, new EnemyConfig
            {
                MaxHealth = 50,
                CanMove = true,
                LockFacing = true,
                XPReward = 50,
                BoundingBoxSize = new Rectangle(0, 0, 15, 47),
                UseGravity = true
            }, audio)
        {
            this.SetSpriteSheet(spriteSheet);
            ChangeState(new BossIdleState());
        }
        protected override IEnemyState GetDefaultState()
        {
            return new BossIdleState();
        }
    }
}
