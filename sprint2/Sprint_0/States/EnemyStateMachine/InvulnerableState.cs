using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using Sprint_0.Enemies;

namespace Sprint_0.EnemyStateMachine
{
    internal class InvulnerableState : IEnemyState
    {
        private double duration;         
        private double timer;            
        private IEnemyState returnState; 
        private bool visible;            
        private double flashTimer;

        public InvulnerableState(IEnemyState previousState, double durationSeconds = 1.0)
        {
            this.returnState = previousState;
            this.duration = durationSeconds;
        }

        public void Start(Enemy enemy)
        {
            enemy.SetAnimation("Hurt"); 
            timer = duration;
            visible = true;
            flashTimer = 0.1; 
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            double dt = gameTime.ElapsedGameTime.TotalSeconds;

            timer -= dt;

            flashTimer -= dt;
            if (flashTimer <= 0)
            {
                flashTimer = 0.1;
                visible = !visible;
            }

            if (timer <= 0)
            {
                enemy.ChangeState(returnState);
            }
        }

        public void Done(Enemy enemy)
        {
            enemy.IsDead = false;
        }

        // Helper for Draw override 
        public void Draw(SpriteBatch spriteBatch, Enemy enemy)
        {
            if (visible)
            {
               // enemy.CurrentAnimation.Draw(spriteBatch, enemy.Position, SpriteEffects.None);
            }
        }
    }
}