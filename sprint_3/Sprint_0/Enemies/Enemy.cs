using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using System.Collections.Generic;

namespace Sprint_0.Enemies
{
    public enum FacingDirection
    {
        Left,
        Right,
        Down,
        Up
    }
    public class Enemy : ICollidable
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Texture2D SpriteSheet { get; private set; }
        public IEnemyState _currentState { get; private set; }
        private Dictionary<string, Animation> animations;
        private Animation CurrentAnimation { get; set; }
        public FacingDirection Facing { get; set; }
        public int MaxHealth { get; private set; } = 2; // Can change for specific enemies, base is 2 hits
        public int CurrentHealth { get; private set; }
        public bool IsInvulnerable { get; private set; }
        private double invulnerableTimer;
        private const double InvulnerableDuration = 0.3;
        public bool IsDead { get; set; }

        //for testing purposes only
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);

        // Capabilities
        public bool CanMove { get; set; }
        public bool CanJump { get; set; }
        public bool CanAttack { get; set; }
        public bool CanCrouch { get; set; }

        internal Enemy(Dictionary<string, Animation> animations, Vector2 startPos)
        {
            this.animations = animations;
            this.Position = startPos;
            this.Velocity = Vector2.Zero;
            this.IsDead = false;
            this.Facing = FacingDirection.Left;
            this.CurrentHealth = MaxHealth;
        }
        public void ChangeState(IEnemyState newState)
        {
            _currentState?.Done(this);
            _currentState = newState;
            _currentState.Start(this);
        }

        public virtual void TakeDamage(int amount)
        {
            if (IsDead || IsInvulnerable)
                return;

            CurrentHealth -= amount;

            if (CurrentHealth <= 0)
            {
                Die();
            }
            else
            {
                // hurt animation and knockback later
                IsInvulnerable = true;
                invulnerableTimer = InvulnerableDuration;
            }
        }

        protected virtual void Die()
        {
            IsDead = true;
            Velocity = Vector2.Zero;

            if (animations.ContainsKey("Death"))
            {
                SetAnimation("Death");
            }

            // Disable behavior, collisions, etc. here later
        }

        public virtual void Update(GameTime gameTime)
        {
            _currentState.Update(this, gameTime);

            CurrentAnimation?.Update(gameTime);

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void SetAnimation(string key)
        {
            if (animations.ContainsKey(key))
            {
                CurrentAnimation = animations[key];
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var effects = (Facing == FacingDirection.Right) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (!IsDead && CurrentAnimation != null)
            {
                CurrentAnimation.Draw(spriteBatch, Position, effects);
            }
        }
        internal Animation GetAnimation(string key)
        {
            if (animations.ContainsKey(key))
            {
                return animations[key];
            }
            return null;
        } 
        }
    }
