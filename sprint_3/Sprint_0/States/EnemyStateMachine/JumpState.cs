using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;

namespace Sprint_0.EnemyStateMachine
{
    public class JumpState : IEnemyState
    {
        private const float JumpVelocity = -200f;
        private const float Gravity = 500f;
        private const float GroundY = 100f;

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Jump");
            enemy.Velocity = new Vector2(enemy.Velocity.X, JumpVelocity);
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
            enemy.Velocity += new Vector2(0, Gravity) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (enemy.Position.Y >= GroundY)
            {
                enemy.Position = new Vector2(enemy.Position.X, GroundY);
                enemy.Velocity = Vector2.Zero;
                enemy.ChangeState(new IdleState());
            }
        }
        public void Done(Enemy enemy) { }
    }
}
