using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.EnemyStateMachine
{
    public class MoveState : IEnemyState
    {
        private float stateTimer;
        private float stateDuration;
        private Vector2 playerPosition;

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Move");
            float speed = 50f;
            enemy.Velocity = (enemy.Facing == FacingDirection.Left)
                ? new Vector2(-speed, 0)
                : new Vector2(speed, 0);

            stateTimer = 0f;
            stateDuration = (float)(1 + Random.Shared.NextDouble() * 2);
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
            stateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (stateTimer >= stateDuration)
            {
                double roll = Random.Shared.NextDouble();

                if (roll < 0.05)
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
                else if (roll < 0.15 && enemy.CanIdle)
                {
                    enemy.ChangeState(new IdleState());
                }
                else if (roll < 0.5 && enemy.CanAttack)
                {
                    enemy.ChangeState(new AttackState());
                }
                else if (roll < 0.6 && !enemy.LockFacing)
                {
                    enemy.Facing = (enemy.Facing == FacingDirection.Left)
                        ? FacingDirection.Right
                        : FacingDirection.Left;
                    enemy.ChangeState(new MoveState());
                }
            }
            //boundary checks
            const int SCREEN_LEFT = 0;
            const int SCREEN_RIGHT = 800; 
            int enemyWidth = enemy.BoundingBox.Width > 0
                ? enemy.BoundingBox.Width
                : (enemy.GetAnimation("Move")?.FrameWidth ?? 16);

            if (enemy.Position.X < SCREEN_LEFT)
            {
                enemy.Position = new Vector2(SCREEN_LEFT, enemy.Position.Y);
                enemy.Facing = FacingDirection.Right;
                enemy.ChangeState(new MoveState());
                return;
            }

            if (enemy.Position.X + enemyWidth > SCREEN_RIGHT)
            {
                enemy.Position = new Vector2(SCREEN_RIGHT - enemyWidth, enemy.Position.Y);
                enemy.Facing = FacingDirection.Left;
                enemy.ChangeState(new MoveState());
                return;
            }
        }
        public void Done(Enemy enemy) { }
    }
}
