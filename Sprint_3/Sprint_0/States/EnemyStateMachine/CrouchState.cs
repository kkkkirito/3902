using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Managers;
using System;

namespace Sprint_0.EnemyStateMachine
{
    internal class CrouchState : IEnemyState
    {
        private float stateTimer;
        private float stateDuration;

        private readonly IAudioManager _audio;

        public CrouchState(IAudioManager audio)
        {
            _audio = audio;
        }
        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Crouch");
            enemy.Velocity = Vector2.Zero;

            stateTimer = 0f;
            stateDuration = 1.0f;
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {

            stateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (stateTimer >= stateDuration)
            {
                double roll = Random.Shared.NextDouble();

                if (roll < 0.15) // 15% chance
                {
                    enemy.ChangeState(new MoveState(_audio));
                }
                else if (roll < 0.3)
                {
                    enemy.ChangeState(new IdleState(_audio));
                }
                else
                {
                    enemy.ChangeState(new CrouchState(_audio));
                }
            }
        }
        public void Done(Enemy enemy) { }
    }
}
