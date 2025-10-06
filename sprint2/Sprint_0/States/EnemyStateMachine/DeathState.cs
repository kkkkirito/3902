using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;

namespace Sprint_0.EnemyStateMachine
{
    internal class DeathState : IEnemyState
    {
        private float deathTimer = 1.0f; // length of death animation
        private float timer;
        public void Start(Enemy enemy)
        {
            timer = 0f;
            enemy.SetAnimation("Death");
            enemy.Velocity = Vector2.Zero;
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        }
        public void Done(Enemy enemy) { }
    }
}
