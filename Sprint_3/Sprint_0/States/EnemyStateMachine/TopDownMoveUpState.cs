using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.EnemyStateMachine
{
    public class TopDownMoveUpState : IEnemyState
    {
        private const float MoveSpeed = 50f;

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Walk");
            enemy.Facing = FacingDirection.Up;
            enemy.Velocity = new Vector2(0, -MoveSpeed);
            enemy.LastCollisionDirection = null;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            if (enemy.LastCollisionDirection == CollisionDirection.Top)
            {
                double roll = Random.Shared.NextDouble();
                if (roll < 0.33)
                    enemy.ChangeState(new TopDownMoveLeftState());
                else if (roll < 0.66)
                    enemy.ChangeState(new TopDownMoveRightState());
                else
                    enemy.ChangeState(new TopDownMoveDownState());
            }
        }

        public void Done(Enemy enemy)
        {
        }
    }
}