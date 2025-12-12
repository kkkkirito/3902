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
        private readonly IAudioManager _audio;


        public AttackState(IAudioManager audio)
        {
            _audio = audio;
        }

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
            _audio.PlayEnemyAttack();
            // Hit Box Later                              
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= attackDuration)
            {
                if (enemy.Velocity != Vector2.Zero)
                {
                    enemy.ChangeState(new MoveState(_audio));
                }
                else
                {
                    enemy.ChangeState(new IdleState(_audio));
                }
            }
        }

        public void Done(Enemy enemy) { }


    }
}
