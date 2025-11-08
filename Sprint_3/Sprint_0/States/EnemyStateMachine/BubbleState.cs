using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.EnemyStateMachine
{
    public class BubbleState : IEnemyState
    {

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Move");
            float speed = 50f;

            // Randomly choose initial direction
            double roll = Random.Shared.NextDouble();
            if (roll < .25)
            {
                enemy.Velocity = new Vector2(speed, speed);
            }
            else if (roll < .5)
            {
                enemy.Velocity = new Vector2(-speed, speed);
            }
            else if (roll < .75)
            {
                enemy.Velocity = new Vector2(speed, -speed);
            }
            else
            {
                enemy.Velocity = new Vector2(-speed, -speed);
            }


        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            // Check if we collided this frame
            if (enemy.LastCollisionDirection.HasValue)
            {
                var v = enemy.Velocity;
                var collisionDir = enemy.LastCollisionDirection.Value;

                System.Diagnostics.Debug.WriteLine($"Collision: {collisionDir}, Velocity before: {v}");

                switch (collisionDir)
                {
                    case CollisionDirection.Top:
                        // Hit ground from above - bounce up 
                        enemy.Velocity = new Vector2(v.X, -v.Y);
                        break;

                    case CollisionDirection.Bottom:
                        // Hit ceiling from below - bounce down 
                        enemy.Velocity = new Vector2(v.X, -v.Y);
                        break;

                    case CollisionDirection.Right:
                        // Hit right wall - bounce left
                        enemy.Velocity = new Vector2(-v.X, v.Y);
                        break;
                        
                    case CollisionDirection.Left:
                        // Hit left wall - bounce right 
                        enemy.Velocity = new Vector2(-v.X, v.Y);
                        break;
                }
            }
            enemy.LastCollisionDirection = null;
        }

        public void Done(Enemy enemy) { }
    }
}