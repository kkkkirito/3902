using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;

namespace Sprint_0.EnemyStateMachine
{
    internal class StalfosFallState : IEnemyState
    {

        public void Start(Enemy enemy)
        {
            //enemy.Velocity = new Vector2(0, 150f);
            

        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
            enemy.SetAnimation("Falling");

            if (enemy.LastCollisionDirection  == CollisionDirection.Bottom)
            { 
                enemy.ChangeState(new IdleState());
            }
        }
        public void Done(Enemy enemy) {

        }
    }
}
