using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using Sprint_0.Interfaces;

namespace Sprint_0.Enemies
{
    public class HorseHeadEnemy : Enemy
    {

        public HorseHeadEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateHorseHeadAnimations(spriteSheet), startPos)
        {
            this.SpriteSheet = spriteSheet;
            this.CanMove = true;
            this.CanJump = false;
            this.CanAttack = false;
            this.CanCrouch = false;
            this.LockFacing = true;
            this.Position = startPos;
            this.BoundingBox = new Rectangle((int)startPos.X, (int)startPos.Y, 15, 47);

            XPReward = 50;

            ChangeState(new BossIdleState());
        }

        protected override IEnemyState GetDefaultState()
            => new Sprint_0.EnemyStateMachine.BossIdleState();
    }
}
