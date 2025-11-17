using Microsoft.Xna.Framework;
using Sprint_0.Enemies;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.EnemyStateMachine
{
    internal class DeathState : IEnemyState
    {
        private float deathTimer = 1.0f; // length of death animation
        private float timer;
        private bool xpGranted;

        public void Start(Enemy enemy)
        {
            timer = 0f;
            enemy.SetAnimation("Death");
            enemy.Velocity = Vector2.Zero;
            enemy.BoundingBox = Rectangle.Empty;
            AudioManager.PlaySound(AudioManager.EnemyDieSound, 0.9f);
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= deathTimer)
            {
                if (!xpGranted)
                {
                    enemy.NotifyDeath();
                    XPManager.Spawn(enemy.SpriteSheet, enemy.Position, enemy.XPReward);

                    xpGranted = true;
                }



                enemy.IsDead = true;
            }

        }
        public void Done(Enemy enemy) { }
    }
}
