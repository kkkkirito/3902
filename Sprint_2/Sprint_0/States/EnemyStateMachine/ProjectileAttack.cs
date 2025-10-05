using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0.EnemyStateMachine
{
    internal class ProjectileAttack : IEnemyState
    {
        private double attackCooldown;
        private bool hasFired;

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Attack");
            attackCooldown = 0.5; // half second to "wind up"
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
                enemy.ChangeState(new IdleState());
            }
        }

        public void Done(Enemy enemy) { }
    }
}
