using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.States.LinkStates;
namespace Sprint_0.Interfaces
{
    public interface IPlayerState
    {

        void Enter(IPlayer player);
        void Exit(IPlayer player);
        void HandleInput(IPlayer player);
        void Update(IPlayer player, GameTime gameTime);
        void Draw(IPlayer player, SpriteBatch spriteBatch, Color color);
    }
}