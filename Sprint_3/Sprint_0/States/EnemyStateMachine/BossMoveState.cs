using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.EnemyStateMachine
{
    public class BossMoveState : IEnemyState
    {
        private float stateTimer;
        private float stateDuration;
        private int moveDirection;

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Move");
            float speed = 50f;

            moveDirection = Random.Shared.NextDouble() < 0.7 ? 1 : -1;

            enemy.Velocity = new Vector2(-speed * moveDirection, 0);

            stateTimer = 0f;
            stateDuration = (float)(1 + Random.Shared.NextDouble()* .2);
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {

            stateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (stateTimer >= stateDuration)
            {
                double roll = Random.Shared.NextDouble();

                if (roll < 0.1)
                {
                    enemy.ChangeState(new BossAttackState());
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
