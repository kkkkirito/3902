using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.EnemyStateMachine
{
    internal class OctorokAttack : IEnemyState
    {
        //Jump Variables
        private const float JumpVelocity = -150f;
        private const float GroundY = 200f;

        private double attackCooldown;
        private bool hasFired;

        private readonly IAudioManager _audio;

        public OctorokAttack(IAudioManager audio)
        {
            _audio = audio;
        }

        public void Start(Enemy enemy)
        {
            enemy.Velocity = new Vector2(enemy.Velocity.X, JumpVelocity);
            enemy.SetAnimation("Attack");
            attackCooldown = 0.2;
            hasFired = false;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {

            attackCooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            if (!hasFired && attackCooldown <= 0)
            {
                Vector2 direction = (enemy.Facing == FacingDirection.Right)
                    ? new Vector2(1, 0)
                    : new Vector2(-1, 0);

                (enemy as OctorokEnemy)?.Shoot(direction);

                hasFired = true;
            }


            if (hasFired)
            {
                enemy.ChangeState(new IdleState(_audio));
            }
        }

        public void Done(Enemy enemy) { }
    }
}
