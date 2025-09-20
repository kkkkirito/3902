using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project1.Sprites;
using System.Collections.Generic;

namespace SpritesDemo.Sprites
{
    public sealed class AnimatedSprite : BaseSprite
    {
        public AnimatedSprite(Texture2D tex, List<Rectangle> frames, Vector2 position, float scale = 3f, double frameSeconds = 0.12)
            : base(tex, frames, position, scale)
        {
            FrameDuration = frameSeconds;
        }

        public override void Update(GameTime gameTime)
        {
            AdvanceFrame(gameTime);
        }
    }
}
