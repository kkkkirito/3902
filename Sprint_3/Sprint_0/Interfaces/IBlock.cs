using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint_0.Interfaces
{
    public interface IBlock : ICollidable
    {
        Vector2 Position { get; set; }
        bool IsSolid { get; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
