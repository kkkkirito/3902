using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;

namespace Sprint_0.EnemyStateMachine
{
    internal class OctorokAttack : IEnemyState
    {
        //Jump Variables
        private const float JumpVelocity = -150f;
        private const float Gravity = 500f;
        private const float GroundY = 200f;

        private double attackCooldown;
        private bool hasFired;

        public void Start(Enemy enemy)
        {
            enemy.Velocity = new Vector2(enemy.Velocity.X, JumpVelocity);
            enemy.SetAnimation("Attack");
            attackCooldown = 0.5;
            hasFired = false;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            enemy.Velocity += new Vector2(0, Gravity) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            attackCooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            if (!hasFired && attackCooldown <= 0)
            {
                Vector2 direction = (enemy.Facing == FacingDirection.Right)
                    ? new Vector2(1, 0)
                    : new Vector2(-1, 0);

                (enemy as OctorokEnemy)?.Shoot(direction);

                hasFired = true;
            }


            if (hasFired && enemy.Position.Y >= GroundY)
            {
                enemy.Position = new Vector2(enemy.Position.X, GroundY);
                enemy.Velocity = Vector2.Zero;
                enemy.ChangeState(new IdleState());
            }
        }

        public void Done(Enemy enemy) { }
    }
}
