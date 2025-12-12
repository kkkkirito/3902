using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Managers;
using System;

namespace Sprint_0.EnemyStateMachine
{
    public class IdleState : IEnemyState
    {
        private float stateTimer;
        private float stateDuration;
        private readonly IAudioManager _audio;

        public IdleState(IAudioManager audio)
        {
            _audio = audio;
        }

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
                        enemy.ChangeState(new JumpState(_audio));
                    }
                    else if (enemy.CanCrouch)
                    {
                        enemy.ChangeState(new CrouchState(_audio));
                    }

                }
                else if (enemy.CanAttack && roll < .6)
                {
                    enemy.ChangeState(new AttackState(_audio));
                }
                else
                {
                    enemy.ChangeState(new MoveState(_audio));
                }
            }
            else if (stateTimer >= stateDuration)
            {
                enemy.ChangeState(new OctorokAttack(_audio));
            }
        }
        public void Done(Enemy enemy) { }
    }
}
