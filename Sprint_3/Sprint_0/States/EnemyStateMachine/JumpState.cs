using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.EnemyStateMachine
{
    public class JumpState : IEnemyState
    {
        private const float JumpVelocity = -200f;
        private const float Gravity = 500f;

        private readonly IAudioManager _audio;

        public JumpState(IAudioManager audio)
        {
            _audio = audio;
        }

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Jump");
            enemy.Velocity = new Vector2(enemy.Velocity.X, JumpVelocity);
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {

            enemy.ChangeState(new IdleState(_audio));
        }

        public void Done(Enemy enemy) { }
    }
}
