using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.EnemyStateMachine
{
    internal class AttackState : IEnemyState
    {
        private float attackDuration = 0.5f; // half a second attack
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
                enemy.SetAnimation("IdleAttack");
            }
            AudioManager.PlaySound(AudioManager.EnemyAttackSound, 0.9f);
            // Hit Box Later                              
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= attackDuration)
            {
                if (enemy.Velocity != Vector2.Zero)
                {
                    enemy.ChangeState(new MoveState());
                }
                else
                {
                    enemy.ChangeState(new IdleState());
                }
            }
        }

        public void Done(Enemy enemy) { }


    }
}
