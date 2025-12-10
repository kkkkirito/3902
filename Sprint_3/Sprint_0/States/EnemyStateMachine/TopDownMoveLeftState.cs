using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.EnemyStateMachine
{
    public class TopDownMoveLeftState : IEnemyState
    {
        private const float MoveSpeed = 50f;

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Walk");
            enemy.Facing = FacingDirection.Left;
            enemy.Velocity = new Vector2(-MoveSpeed, 0);
            enemy.LastCollisionDirection = null;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            if (enemy.LastCollisionDirection == CollisionDirection.Left)
            {
                double roll = Random.Shared.NextDouble();
                if (roll < 0.33)
                    enemy.ChangeState(new TopDownMoveUpState());
                else if (roll < 0.66)
                    enemy.ChangeState(new TopDownMoveDownState());
                else
                    enemy.ChangeState(new TopDownMoveRightState());
            }
        }

        public void Done(Enemy enemy)
        {
        }
    }
}