using Microsoft.Xna.Framework;

namespace Sprint_0.EnemyStateMachine
{
    internal class MoveDownState : IEnemyState
    {
        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("MoveDown");
            enemy.Velocity = new Vector2(enemy.Velocity.X, -50f);
            enemy.Facing = FacingDirection.Down;
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
            if (enemy.Position.X < 0)
            {
                enemy.ChangeState(new MoveRightState());
            }
        }
        public void Done(Enemy enemy) { }
    }
}
