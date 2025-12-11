using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.EnemyStateMachine
{
    public class IdleState : IEnemyState
    {
        private float stateTimer;
        private float stateDuration;

        public void Start(Enemy enemy)
        {
            Random rnd = new Random();
            enemy.SetAnimation("Idle");
            enemy.Velocity = Vector2.Zero;

            stateTimer = 0f;
            stateDuration = (float)(rnd.Next(1,4));
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {

            stateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (stateTimer >= stateDuration && enemy.CanMove)
            {
                double roll = Random.Shared.NextDouble();

                if (roll < .1) // 10% chance
                {
                    if (enemy.CanJump)
                    {
                        enemy.ChangeState(new JumpState());
                    }
                    else if (enemy.CanCrouch)
                    {
                        enemy.ChangeState(new CrouchState());
                    }

                }
                else if (enemy.CanAttack && roll < .4)
                {
                    enemy.ChangeState(new AttackState());
                }
                else
                {
                    enemy.ChangeState(new MoveState());
                }
            }
            else if (stateTimer >= stateDuration)
            {
                enemy.ChangeState(new OctorokAttack());
            }
        }
        public void Done(Enemy enemy) { }
    }
}
