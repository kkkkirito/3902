using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.Blocks
{
    public class TopDownDoor : IBlock, ICollidable
    {
        private Animation lockedAnimation;
        private Animation unlockingAnimation;
        private bool isLocked;
        private bool isUnlocking;
        private float unlockTimer;
        private const float UNLOCK_DURATION = 1.8f;

        public bool IsSolid => isLocked || isUnlocking;
        public Vector2 Position { get; set; }

        public Rectangle BoundingBox => new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            7,
            42
        );

        public bool IsLocked => isLocked;

        public TopDownDoor(Vector2 position, Texture2D itemTextures)
        {
            Position = position;
            isLocked = true;
            isUnlocking = false;
            unlockTimer = 0f;

            var animations = SpriteFactory.CreateItemAnimations(itemTextures);
            if (animations.ContainsKey("TopDownLockedDoor"))
            {
                this.lockedAnimation = animations["TopDownLockedDoor"];
            }
            if (animations.ContainsKey("TopDownUnlockDoor"))
            {
                this.unlockingAnimation = animations["TopDownUnlockDoor"];
            }
        }

        public void Unlock()
        {
            if (isLocked)
            {
                isLocked = false;
                isUnlocking = true;
                unlockTimer = UNLOCK_DURATION;
            }
        }

        public void Lock()
        {
            isLocked = true;
            isUnlocking = false;
            unlockTimer = 0f;
        }

        public void Update(GameTime gameTime)
        {
            if (isUnlocking)
            {
                unlockTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                unlockingAnimation?.Update(gameTime);

                if (unlockTimer <= 0f)
                {
                    isUnlocking = false;
                }
            }
            else if (isLocked)
            {
                lockedAnimation?.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isLocked)
            {
                lockedAnimation?.Draw(spriteBatch, Position, SpriteEffects.None);
            }
            else if (isUnlocking)
            {
                unlockingAnimation?.Draw(spriteBatch, Position, SpriteEffects.None);
            }
        }

        public void Reset()
        {
            isLocked = true;
            isUnlocking = false;
            unlockTimer = 0f;
        }
    }
}
