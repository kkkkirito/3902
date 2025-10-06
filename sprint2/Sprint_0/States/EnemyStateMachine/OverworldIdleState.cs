using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.EnemyStateMachine
{
    public class OverworldIdleState : IEnemyState
    {

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Idle");
            enemy.Velocity = Vector2.Zero;
        }
        public void Update(Enemy enemy, GameTime gameTime)
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
            else // 25% chance
            {
                enemy.ChangeState(new OverworldMoveDownState());
            }
        }
        public void Done(Enemy enemy) { }
    }
}
