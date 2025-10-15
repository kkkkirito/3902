using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;

namespace Sprint_0.EnemyStateMachine
{
    internal class StalfosFallState : IEnemyState
    {

        public void Start(Enemy enemy)
        {

            enemy.SetAnimation("Fall");
            enemy.Velocity = new Vector2(0, 150f); // pixels per second downward
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {

            if (enemy.Position.Y >= 200) // Update Later for ground level
            {
                enemy.Position = new Vector2(enemy.Position.X, 200);
                enemy.Velocity = Vector2.Zero;

                enemy.ChangeState(new IdleState());
            }
        }
        public void Done(Enemy enemy) { }
    }
}
