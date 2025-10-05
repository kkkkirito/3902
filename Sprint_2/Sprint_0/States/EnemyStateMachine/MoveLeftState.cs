using Microsoft.Xna.Framework;

namespace Sprint_0.EnemyStateMachine
{
    public class MoveLeftState : IEnemyState
    {
        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("MoveLeft");
            enemy.Velocity = new Vector2(-50f, enemy.Velocity.Y);
            enemy.Facing = FacingDirection.Left;
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
