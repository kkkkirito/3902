using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.EnemyStateMachine
{
    internal class StalfosFallState : IEnemyState
    {
        private readonly IAudioManager _audio;

        public StalfosFallState(IAudioManager audio)
        {
            _audio = audio;
        }

        public void Start(Enemy enemy)
        {
            //enemy.Velocity = new Vector2(0, 150f);
            

        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
            enemy.SetAnimation("Falling");

            if (enemy.LastCollisionDirection  == CollisionDirection.Bottom)
            { 
                enemy.ChangeState(new IdleState(_audio));
            }
        }
        public void Done(Enemy enemy) {

        }
    }
}
