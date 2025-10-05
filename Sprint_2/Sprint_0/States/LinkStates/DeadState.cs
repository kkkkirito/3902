using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using Sprint_0.States.LinkStates;

namespace Sprint_0
{
    public class DeadState : IPlayerState
    {
        public void Enter(IPlayer player)
        {
            player.Velocity = Vector2.Zero;
        }

        public void Exit(IPlayer player) { }

        public void HandleInput(IPlayer player)
        {
            //Player is dead, ignore inputs
        }

        public void Update(IPlayer player, GameTime gameTime)
        {
            //Player is dead, ignore update
        }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
           // Rectangle source = SpriteFactory.GetDeadSprite();
            //spriteBatch.Draw(player.SpriteSheet, player.Position, source, color);
        }
    }
}