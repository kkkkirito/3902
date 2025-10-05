using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0.Enemies
{
    public class OctorokEnemy : Enemy
    {
        private List<Projectile> projectiles;
        private Texture2D projectileTexture;

        public OctorokEnemy(Texture2D spriteSheet, Texture2D projectileTex, Vector2 startPos)
            : base(SpriteFactory.CreateOctorokAnimations(spriteSheet), startPos)
        {
            this.projectileTexture = projectileTex;
            this.projectiles = new List<Projectile>();

            ChangeState(new IdleState());
            SetAnimation("Idle");
        }

        public void Shoot(Vector2 direction)
        {
            var proj = new Projectile(
                projectileTexture,
                new Rectangle(0, 0, 8, 8), // projectile sprite, needs updated
                this.Position,
                direction * 100f
            );

            projectiles.Add(proj);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var proj in projectiles)
                proj.Update(gameTime);

            projectiles.RemoveAll(p => !p.IsActive);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (var proj in projectiles)
                proj.Draw(spriteBatch);
        }
    }
}