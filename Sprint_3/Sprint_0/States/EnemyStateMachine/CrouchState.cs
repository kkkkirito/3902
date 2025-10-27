using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.EnemyStateMachine
{
    internal class CrouchState : IEnemyState
    {
        private float stateTimer;
        private float stateDuration;
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
                    enemy.ChangeState(new MoveState());
                }
                else if (roll < 0.3)
                {
                    enemy.ChangeState(new IdleState());
                }
                else
                {
                    enemy.ChangeState(new CrouchState());
                }
            }
        }
        public void Done(Enemy enemy) { }
    }
}
