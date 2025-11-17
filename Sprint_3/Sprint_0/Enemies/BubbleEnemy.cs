using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;

namespace Sprint_0.Enemies
{
    public class BubbleEnemy : Enemy
    {
        public BubbleEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateBubbleAnimations(spriteSheet), startPos)
        {
            this.SpriteSheet = spriteSheet;
            this.CanMove = true;
            this.CanJump = false;
            this.CanAttack = false;
            this.CanCrouch = false;
            this.Position = startPos;
            this.BoundingBox = new Rectangle((int)startPos.X, (int)startPos.Y, 15, 14);

            XPReward = 50;

            ChangeState(new BubbleState());
        }

        // Override Update to remove gravity effect
        public override void Update(GameTime gameTime)
        {
            if (IsInvulnerable)
            {
                invulnerableTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (invulnerableTimer <= 0)
                    IsInvulnerable = false;
            }

            _currentState?.Update(this, gameTime);

            if (!IsDead && !(_currentState is DeathState))
            {
                Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height);
            }
            else
            {
                BoundingBox = Rectangle.Empty;
            }
        }
    }
}
