using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.EnemyStateMachine
{
    public class OverworldMoveLeftState : IEnemyState
    {
        private float stateTimer;
        private float stateDuration;

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("MoveLeft");
            enemy.Velocity = new Vector2(-60f, 0);
            enemy.Facing = FacingDirection.Left;

            stateTimer = 0f;
            stateDuration = (float)(1 + Random.Shared.NextDouble() * 2);
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {

            stateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (stateTimer >= stateDuration)
            {
                double roll = Random.Shared.NextDouble();

                if (roll < 0.25) // 25% chance
                {
                    enemy.ChangeState(new OverworldMoveUpState());
                }
                else if (roll < 0.5) // 25% chance
                {
                    enemy.ChangeState(new OverworldMoveLeftState());
                }
                else if (roll < 0.75) // 25% chance
                {
                    enemy.ChangeState(new OverworldMoveRightState());
                }
                else if (roll < 1) // 25% chance
                {
                    enemy.ChangeState(new OverworldMoveDownState());
                }
            }

            if (enemy.Position.X < 0)
            {
                enemy.ChangeState(new OverworldMoveRightState());
            }
        }
        public void Done(Enemy enemy) { }
    }
}
