using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using System;
using System.Collections.Generic;

namespace Sprint_0.Enemies
{
    public enum FacingDirection { Left, Right, Down, Up }

    public class EnemyConfig
    {
        public int MaxHealth { get; set; } = 2;
        public int XPReward { get; set; } = 10;
        public bool CanMove { get; set; }
        public bool CanJump { get; set; }
        public bool CanAttack { get; set; }
        public bool CanCrouch { get; set; }
        public bool CanIdle { get; set; } = true;
        public bool LockFacing { get; set; }
        public bool DropItemOnDeath { get; set; }
        public bool UseGravity { get; set; } = true;
        public Rectangle BoundingBoxSize { get; set; }
    }

    public class Enemy : ICollidable, IResettable
    {
        private const float Gravity = 500f;
        private const double InvulnerableDuration = 0.3;

        // Core properties
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 StartPosition { get; set; }
        public float GroundY { get; set; }
        public Rectangle BoundingBox { get; set; }
        public FacingDirection Facing { get; set; }
        public Texture2D SpriteSheet { get; set; }

        // State
        public IEnemyState _currentState { get; private set; }
        public bool IsDead { get; set; }
        public bool IsGrounded { get; set; }
        public CollisionDirection? LastCollisionDirection { get; set; }

        // Health
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; set; }
        public bool IsInvulnerable { get; set; }
        protected double invulnerableTimer;

        // Config properties
        public bool CanMove { get; set; }
        public bool CanJump { get; set; }
        public bool CanAttack { get; set; }
        public bool CanCrouch { get; set; }
        public bool CanIdle { get; set; }
        public bool LockFacing { get; set; }
        public bool DropItemOnDeath { get; set; }
        public int XPReward { get; set; }
        public bool UseGravity { get; set; }

        public event Action<Enemy> OnDeath;

        private Dictionary<string, Animation> animations;
        private Animation CurrentAnimation { get; set; }

        internal Enemy(Dictionary<string, Animation> animations, Vector2 startPos, EnemyConfig config)
        {
            this.animations = animations;
            Position = StartPosition = startPos;
            GroundY = startPos.Y;
            Velocity = Vector2.Zero;
            Facing = FacingDirection.Left;

            MaxHealth = CurrentHealth = config.MaxHealth;
            XPReward = config.XPReward;
            CanMove = config.CanMove;
            CanJump = config.CanJump;
            CanAttack = config.CanAttack;
            CanCrouch = config.CanCrouch;
            CanIdle = config.CanIdle;
            LockFacing = config.LockFacing;
            DropItemOnDeath = config.DropItemOnDeath;
            UseGravity = config.UseGravity;

            BoundingBox = new Rectangle((int)startPos.X, (int)startPos.Y,
                config.BoundingBoxSize.Width, config.BoundingBoxSize.Height);
        }

        internal Enemy(Dictionary<string, Animation> animations, Vector2 startPos)
            : this(animations, startPos, new EnemyConfig()) { }

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
            if (CurrentHealth <= 0)
                Die();
            else
            {
                IsInvulnerable = true;
                invulnerableTimer = InvulnerableDuration;
            }
        }

        protected virtual void Die() => ChangeState(new EnemyStateMachine.DeathState());

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
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Position += Velocity * dt;
                if (UseGravity) Velocity += new Vector2(0, Gravity) * dt;

                BoundingBox = new Rectangle((int)Position.X, (int)Position.Y,
                    BoundingBox.Width, BoundingBox.Height);
            }
            else
                BoundingBox = Rectangle.Empty;
        }

        public void SetAnimation(string key)
        {
            if (animations.ContainsKey(key))
                CurrentAnimation = animations[key];
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsDead || CurrentAnimation == null) return;

            var effects = Facing == FacingDirection.Right
                ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            CurrentAnimation.Draw(spriteBatch, Position, effects);
        }

        internal Animation GetAnimation(string key)
            => animations.ContainsKey(key) ? animations[key] : null;

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
                BoundingBox = new Rectangle((int)Position.X, (int)Position.Y,
                    anim.FrameWidth, anim.FrameHeight);
        }

        protected virtual IEnemyState GetDefaultState()
            => new EnemyStateMachine.IdleState();

        public virtual IEnumerable<ICollidable> GetExtraCollidables()
        { yield break; }
    }
}