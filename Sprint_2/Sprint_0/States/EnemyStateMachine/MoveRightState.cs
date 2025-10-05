using Microsoft.Xna.Framework;

namespace Sprint_0.EnemyStateMachine
{
    public class MoveRightState : IEnemyState
    {
        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("MoveRight");
            enemy.Velocity = new Vector2(50f, enemy.Velocity.Y);
            enemy.Facing = FacingDirection.Right;
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
            if (enemy.Position.X < 0)
            {
                enemy.ChangeState(new MoveLeftState());
            }
        }        
        public void Done(Enemy enemy) { }
    }
}
