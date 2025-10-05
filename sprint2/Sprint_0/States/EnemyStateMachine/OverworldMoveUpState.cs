using Microsoft.Xna.Framework;
using System;
using Sprint_0.Interfaces;
using Sprint_0.Enemies;

namespace Sprint_0.EnemyStateMachine
{
    internal class OverworldMoveUpState : IEnemyState
    {
        private float stateTimer;
        private float stateDuration;

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("MoveUp");
            enemy.Velocity = new Vector2(0, -60f);
            enemy.Facing = FacingDirection.Up;

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
                    enemy.ChangeState(new OverworldMoveDownState());
                }
                else if (roll < 0.5) // 25% chance
                {
                    enemy.ChangeState(new OverworldMoveDownState());
                }
                else if (roll < 0.75) // 25% chance
                {
                    enemy.ChangeState(new OverworldMoveDownState());
                }
                else if (roll < 1) // 25% chance
                {
                    enemy.ChangeState(new OverworldMoveDownState());
                }
            }
            if (enemy.Position.Y < 0)
            {
                enemy.ChangeState(new OverworldMoveDownState());
            }
        }
        public void Done(Enemy enemy) { }
    }
}
