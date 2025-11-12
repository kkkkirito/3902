using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.EnemyStateMachine
{
    public class BossIdleState : IEnemyState
    {
        private float stateTimer;
        private float stateDuration;

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Idle");
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

                if (roll < .25)
                {
                    enemy.ChangeState(new BossAttackState());
                }
                else
                {
                    enemy.ChangeState(new BossMoveState());
                }
            }
        }
        public void Done(Enemy enemy) { }
    }
}
