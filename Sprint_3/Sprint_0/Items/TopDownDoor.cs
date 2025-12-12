using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using System;

namespace Sprint_0.Blocks
{
    public class TopDownDoor : IBlock, ICollidable, IResettable
    {
        private Animation lockedAnimation;
        private Animation unlockAnimation;
        private Animation currentAnimation;
        private Texture2D texture;
        private bool isUnlocked = false;
        private const float UNLOCK_DURATION = 1.7f;
        private float unlockTimer = 0f;
        private bool animationComplete = false;
        public bool IsSolid => !isUnlocked;
        public bool IsUnlocked => isUnlocked;
        public Vector2 Position { get; set; }
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, 8, 48);

        // Event fired when door is unlocked - passes the door's bounding box for transition check
        public static event Action<Rectangle> OnDoorUnlocked;

        public TopDownDoor(Vector2 position, Texture2D itemTextures)
        {
            Position = position;
            texture = itemTextures;
            var animations = SpriteFactory.CreateItemAnimations(itemTextures);
            if (animations.ContainsKey("LockedDoor"))
                this.lockedAnimation = animations["LockedDoor"];
            if (animations.ContainsKey("UnlockDoor"))
                this.unlockAnimation = animations["UnlockDoor"];
            else
                this.unlockAnimation = this.lockedAnimation;
            currentAnimation = lockedAnimation;
        }
        public void Unlock()
        {
            if (isUnlocked) return;

            Rectangle doorBounds = BoundingBox;

            isUnlocked = true;
            unlockTimer = 0f;
            currentAnimation = unlockAnimation;

            OnDoorUnlocked?.Invoke(doorBounds);
        }

        public void Update(GameTime gameTime)
        {
            currentAnimation?.Update(gameTime);
            if (isUnlocked && !animationComplete)
            {
                unlockTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (unlockTimer >= UNLOCK_DURATION)
                {
                    animationComplete = true;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (animationComplete) return;
            currentAnimation?.Draw(spriteBatch, Position, SpriteEffects.None);
        }

        public void ResetState()
        {
            isUnlocked = false;
            unlockTimer = 0f;
            animationComplete = false;
            currentAnimation = lockedAnimation;
            // Reset the animation to its first frame
            lockedAnimation?.Reset();
            unlockAnimation?.Reset();
        }
    }
}