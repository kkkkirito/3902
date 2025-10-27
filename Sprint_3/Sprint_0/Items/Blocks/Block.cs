using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.Blocks
{
    public class Block : IBlock, ICollidable
    {
        private readonly ISprite sprite;
        public bool IsSolid { get; private set; }
        public Vector2 Position { get; set; }
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, 17, 17);
        public Block(ISprite sprite, Vector2 position, bool isSolid)
        {
            this.sprite = sprite;
            IsSolid = isSolid;
            Position = position;

        }

        public void Update(GameTime gameTime)
        {
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, Position, SpriteEffects.None);
        }
    }
}
