using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;

namespace Sprint_0.EnemyStateMachine
{
    internal class BossAttackState : IEnemyState
    {
        private float attackDuration = 0.8f;
        private float timer;

        public void Start(Enemy enemy)
        {
            timer = 0f;
            if (enemy.Velocity != Vector2.Zero)
            {
                enemy.SetAnimation("MoveAttack");
            }
            else
            {
                enemy.SetAnimation("MoveAttack");
            }
            // Hit Box Later                              
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= attackDuration)
            {
                if (enemy.Velocity != Vector2.Zero)
                {
                    enemy.ChangeState(new BossMoveState());
                }
                else
                {
                    enemy.ChangeState(new BossIdleState());
                }
            }
        }

        public void Done(Enemy enemy) { }


    }
}
