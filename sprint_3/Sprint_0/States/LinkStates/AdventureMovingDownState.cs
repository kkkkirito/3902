//Dillon Brigode AU25
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.States.LinkStates
{
    public class AdventureMovingDownState : IPlayerState
    {
        public void Enter(IPlayer player)
        {
            player.CurrentFrame = 0;
        }

        public void Exit(IPlayer player) { }

        public void HandleInput(IPlayer player) { }

        public void Update(IPlayer player, GameTime gameTime)
        {
            // Handle animation frames if needed
        }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
            var source = SpriteFactory.GetWalkingSprite(player.FacingDirection, player.CurrentFrame);
            var effects = (player.FacingDirection == Direction.Left) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(player.SpriteSheet, player.Position, source, color, 0f, Vector2.Zero, 1f, effects, 0f);
        }
    }
}
