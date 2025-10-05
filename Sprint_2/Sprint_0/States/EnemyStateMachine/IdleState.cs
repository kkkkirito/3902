using Microsoft.Xna.Framework;
using System;

namespace Sprint_0.EnemyStateMachine
{
    public class IdleState : IEnemyState
    {
        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Idle");
            enemy.Velocity = Vector2.Zero;
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
            if (Random.Shared.NextDouble() < 0.01)
            {
                enemy.ChangeState(new MoveLeftState());
            }
        }
        public void Done(Enemy enemy) { }
    }
}
