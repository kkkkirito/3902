using Microsoft.Xna.Framework;

namespace Sprint_0.EnemyStateMachine
{
    public class JumpState : IEnemyState
    {
        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Jump");
            enemy.Velocity = new Vector2(enemy.Velocity.X, -200f);
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
            enemy.Velocity += new Vector2(0, 500f) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (enemy.Position.X < 0)
            {
                enemy.Position = new Vector2(enemy.Position.X, 0);
                enemy.Velocity = Vector2.Zero;
                enemy.ChangeState(new IdleState());
            }
        }
        public void Done(Enemy enemy) { }
    }
}
