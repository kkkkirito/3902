using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using Sprint_0.Interfaces;

namespace Sprint_0.Enemies
{
    public class HorseHeadEnemy : Enemy
    {
        public HorseHeadEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateHorseHeadAnimations(spriteSheet), startPos, new EnemyConfig
            {
                CanMove = true,
                LockFacing = true,
                XPReward = 50,
                BoundingBoxSize = new Rectangle(0, 0, 15, 47),
                UseGravity = true
            })
        {
            this.SpriteSheet = spriteSheet;
            ChangeState(new BossIdleState());
        }
        protected override IEnemyState GetDefaultState()
        {
            return new BossIdleState();
        }
    }
}
