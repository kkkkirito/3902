﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint_0.Items
{
    public class KeyItem : IConsumableItem
    {
        public string Name => "Key";
        public bool IsConsumable => true;

        public Vector2 Position { get; set; }
        public bool IsCollected { get; set; }

        private Animation animation;
        private int keyCount = 0;

        public KeyItem(Vector2 position, Texture2D itemTextures)
        {
            Position = position;
            IsCollected = false;

            var animations = SpriteFactory.CreateItemAnimations(itemTextures);
            if (animations.ContainsKey("Key"))
            {
                this.animation = animations["Key"];
            }
        }

        public void Consume(IPlayer player)
        {
            if (!IsCollected)
            {
                keyCount++;
                IsCollected = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!IsCollected)
            {
                animation?.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
            {
                animation?.Draw(spriteBatch, Position, SpriteEffects.None);
            }
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
        }
    }
}