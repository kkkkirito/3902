using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint_0
{
    public interface ISprite
    {

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects);

    }
}
