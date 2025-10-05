using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Sprint_0.Interfaces;

namespace Sprint_0.Enemies
{
    public enum FacingDirection
    {
        Left,
        Right,
        Down,
        Up
    }
    public class Enemy
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Texture2D SpriteSheet { get; private set; }
        public IEnemyState _currentState { get; private set; }
        private Dictionary<string, Animation> animations;
        private Animation CurrentAnimation { get; set; }
        public bool IsDead { get; set; }
        public FacingDirection Facing { get; set; }

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
        }
        public void ChangeState(IEnemyState newState)
        {
            _currentState?.Done(this);
            _currentState = newState;
            _currentState.Start(this);
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
