using Microsoft.Xna.Framework;

namespace Sprint_0.EnemyStateMachine
{
    internal class MoveUpState : IEnemyState
    {
        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("MoveUp");
            enemy.Velocity = new Vector2(enemy.Velocity.X, 50f);
            enemy.Facing = FacingDirection.Up;
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
