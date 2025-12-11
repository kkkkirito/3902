using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.Blocks
{
    public class TrapBlock : IBlock, ICollidable, IResettable
    {
        private ISprite solidSprite;
        private ISprite breakingSprite;
        private TrapState state;
        private float breakTimer;
        private Texture2D blockTextures;
        private const float delay = .5f;

        // Keep solid during both Solid AND Breaking states
        public bool IsSolid => state == TrapState.Solid || state == TrapState.Breaking;

        public Vector2 Position { get; set; }
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);

        private enum TrapState
        {
            Solid,
            Breaking,
            Broken
        }

        public TrapBlock(ISprite solidSprite, ISprite breakingSprite, Vector2 position)
        {
            this.solidSprite = solidSprite;
            this.breakingSprite = breakingSprite;
            Position = position;
            state = TrapState.Solid;
            breakTimer = 0f;
        }

        public TrapBlock(Vector2 position, Texture2D blockTextures)
        {
            Position = position;
            this.blockTextures = blockTextures;
            state = TrapState.Solid;
            breakTimer = 0f;
            var blockAnimations = SpriteFactory.CreateBlockTextures(blockTextures);
            solidSprite = blockAnimations["bb"].Clone();
            breakingSprite = blockAnimations["bbBreak"].Clone();
        }

        public void TriggerBreak()
        {
            if (state == TrapState.Solid)
            {
                breakTimer = delay;
                state = TrapState.Breaking;
            }
        }

        public bool IsBroken => state == TrapState.Broken;

        public void Update(GameTime gameTime)
        {
            if (state == TrapState.Breaking)
            {
                breakTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (breakTimer <= 0f)
                {
                    state = TrapState.Broken;
                }
            }

            // Update sprite animations
            solidSprite?.Update(gameTime);
            breakingSprite?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case TrapState.Solid:
                    solidSprite?.Draw(spriteBatch, Position, SpriteEffects.None);
                    break;
                case TrapState.Breaking:
                    breakingSprite?.Draw(spriteBatch, Position, SpriteEffects.None);
                    break;
                case TrapState.Broken:
                    // Don't draw anything
                    break;
            }
        }

        public void Reset()
        {
            state = TrapState.Solid;
            breakTimer = 0f;
        }

        public void ResetState() => Reset();
    }
}