using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using System.Collections.Generic;
using Sprint_0.Interfaces;

namespace Sprint_0.Enemies
{
    public class OctorokEnemy : Enemy
    {
        private List<Projectile> projectiles;
        private Animation projectileAnimation;

        public OctorokEnemy(Texture2D spriteSheet, Texture2D projectileTexture, Vector2 startPos)
            : base(SpriteFactory.CreateOctorokAnimations(spriteSheet), startPos)
        {
            this.SpriteSheet = spriteSheet;
            this.CanMove = false;
            this.CanJump = true;
            this.CanAttack = true;
            this.CanCrouch = false;
            this.DropItemOnDeath = true;

            this.projectileAnimation = GetAnimation("Projectile");

            this.projectiles = new List<Projectile>();
            this.BoundingBox = new Rectangle((int)startPos.X, (int)startPos.Y, 16, 16);
            XPReward = 10;

            ChangeState(new IdleState());
            SetAnimation("Idle");
        }

        public void Shoot(Vector2 direction)
        {
            // clone the projectile animation so each projectile animates independently
            Animation animClone = projectileAnimation.Clone();
            var proj = new Projectile(animClone, this.Position, direction * 100f);
            projectiles.Add(proj);
        }

        public IEnumerable<IEnemyProjectile> GetActiveProjectiles()
        {
            foreach (var p in projectiles)
            {
                if (p.IsActive) yield return p;
            }
        }

        public override IEnumerable<ICollidable> GetExtraCollidables()
        {
            foreach (var p in projectiles)
            {
                if (p.IsActive) yield return p;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var proj in projectiles) proj.Update(gameTime);
            projectiles.RemoveAll(p => !p.IsActive);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (var proj in projectiles) proj.Draw(spriteBatch);
        }
    }
}