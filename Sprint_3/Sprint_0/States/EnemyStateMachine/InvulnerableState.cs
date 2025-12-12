using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;

namespace Sprint_0.EnemyStateMachine
{
    internal class InvulnerableState : IEnemyState
    {
        private double duration;
        private double timer;
        private IEnemyState returnState;

        public InvulnerableState(IEnemyState previousState, double durationSeconds = 0.1)
        {
            this.returnState = previousState;
            this.duration = durationSeconds;
        }

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Hurt");
            timer = duration;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            double dt = gameTime.ElapsedGameTime.TotalSeconds;

            timer -= dt;

            if (timer <= 0)
            {
                enemy.Velocity = new Vector2(0, 0);
                enemy.ChangeState(returnState);
            }
        }

        public void Done(Enemy enemy)
        {
        }
    }
}