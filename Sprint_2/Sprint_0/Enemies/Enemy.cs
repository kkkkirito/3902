using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint_0.EnemyStateMachine
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

        internal Enemy(Dictionary<string, Animation> animations, Vector2 startPos)
        {
            this.animations = animations;
            this.Position = startPos;
            this.Velocity = Vector2.Zero;
            this.IsDead = false;
            this.Facing = FacingDirection.Right;
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
            if (!IsDead && CurrentAnimation != null)
            {
                CurrentAnimation.Draw(spriteBatch, Position, SpriteEffects.None);
            }
        }
    }
}
