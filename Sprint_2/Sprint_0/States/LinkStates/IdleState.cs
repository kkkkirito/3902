using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.States.LinkStates
{
    public class IdleState : IPlayerState
    {
        private InputState state = new InputState();
        public void Enter(IPlayer player)
        {
            player.Velocity = Vector2.Zero;
            //player.AnimationTimer = 0.0;
            player.CurrentFrame = 0;
        }

        public void Exit(IPlayer player)
        {
            // Clean up if needed
        }

        public void HandleInput(IPlayer player)
        {
            // Handle movement
            state = new InputState();
        }

        public void Update(IPlayer player, GameTime gameTime)
        {
            // Update idle animation if any
            //player.AnimationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
            //Rectangle source = SpriteFactory.GetIdleSprite(player.FacingDirection, player.CurrentFrame);
            //spriteBatch.Draw(player.SpriteSheet, player.Position, source, color);
        }
    }
}