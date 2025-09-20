using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Sprites;
using System.Collections.Generic;

namespace SpritesDemo.Sprites
{
    public sealed class StaticSprite : BaseSprite
    {
        public StaticSprite(Texture2D tex, Rectangle frame, Vector2 position, float scale = 3f)
            : base(tex, new List<Rectangle> { frame }, position, scale)
        {
        }

        public override void Update(GameTime gameTime)
        {
            // No animation, no movement
        }
    }
}
