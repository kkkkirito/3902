using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint_0.Interfaces
{
    public interface IGameState
    {
        void Enter();
        void Exit();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void Reset();
    }
}
