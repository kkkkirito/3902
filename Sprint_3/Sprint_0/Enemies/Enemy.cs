using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using System;
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

    public class Enemy : ICollidable, IResettable
    {
        public float GroundY { get; set; }
        private const float Gravity = 500f;
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        private float VerticalVelocity = 0f;
        public Texture2D SpriteSheet { get; set; }
        public IEnemyState _currentState { get; private set; }
        private Dictionary<string, Animation> animations;
        private Animation CurrentAnimation { get; set; }
        public FacingDirection Facing { get; set; }
        public int MaxHealth { get; private set; } = 2;
        public int CurrentHealth { get; set; }
        public bool IsInvulnerable { get; set; }
        protected double invulnerableTimer;
        private const double InvulnerableDuration = 0.3;
        public bool IsDead { get; set; }
        public bool IsGrounded { get; set; }
        public bool LockFacing { get; protected set; } = false;
        public bool DropItemOnDeath { get; set; } = false;
        public int XPReward { get; set; } = 10;
        public event Action<Enemy> OnDeath;

        public Rectangle BoundingBox { get; set; }
        public CollisionDirection? LastCollisionDirection { get; set; }

        public bool CanMove { get; protected set; }
        public bool CanJump { get; protected set; }
        public bool CanAttack { get; protected set; }
        public bool CanCrouch { get; protected set; }
        public bool CanIdle { get; protected set; } = true;

        public Vector2 StartPosition { get; set; }

        internal Enemy(Dictionary<string, Animation> animations, Vector2 startPos)
        {
            this.animations = animations;
            this.Position = startPos;
            this.StartPosition = startPos;
            this.GroundY = startPos.Y;
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
            if (IsDead || IsInvulnerable) return;

            CurrentHealth -= amount;
            if (CurrentHealth <= 0) Die();
            else
            {
                IsInvulnerable = true;
                invulnerableTimer = InvulnerableDuration;
            }
        }

        protected virtual void Die()
        {
            ChangeState(new EnemyStateMachine.DeathState());
        }

        internal void NotifyDeath() => OnDeath?.Invoke(this);

        public virtual void Update(GameTime gameTime)
        {
            if (IsInvulnerable)
            {
                invulnerableTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (invulnerableTimer <= 0) IsInvulnerable = false;
            }

            _currentState?.Update(this, gameTime);
            CurrentAnimation?.Update(gameTime);

            if (!IsDead && !(_currentState is EnemyStateMachine.DeathState))
            {
                Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height);
            }
            else
            {
                BoundingBox = Rectangle.Empty;
            }

            Velocity += new Vector2(0, Gravity) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void SetAnimation(string key)
        {
            if (animations.ContainsKey(key)) CurrentAnimation = animations[key];
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var effects = (Facing == FacingDirection.Right) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (!IsDead && CurrentAnimation != null) CurrentAnimation.Draw(spriteBatch, Position, effects);
        }

        internal Animation GetAnimation(string key)
        {
            if (animations.ContainsKey(key)) return animations[key];
            return null;
        }

        protected void UpdateCurrentAnimation(GameTime gameTime) => CurrentAnimation?.Update(gameTime);

        // Reset contract implemented via IResettable
        public virtual void ResetState()
        {
            Position = StartPosition;
            Velocity = Vector2.Zero;
            IsDead = false;
            CurrentHealth = MaxHealth;
            IsInvulnerable = false;

            var defaultState = GetDefaultState();
            if (defaultState != null) ChangeState(defaultState);

            SetAnimation("Idle");
            var anim = GetAnimation("Idle");
            if (anim != null)
                BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, anim.FrameWidth, anim.FrameHeight);
        }

        protected virtual IEnemyState GetDefaultState() => new Sprint_0.EnemyStateMachine.IdleState();

        public virtual IEnumerable<ICollidable> GetExtraCollidables()
        {
            yield break;
        }
    }
}
