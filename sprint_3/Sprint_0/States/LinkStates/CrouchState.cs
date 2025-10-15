//Dillon Brigode AU25
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.States.LinkStates
{
    public class CrouchState : IPlayerState
    {
        public void Enter(IPlayer player)
        {
            player.Velocity = Vector2.Zero;
            player.CurrentFrame = 0;
        }
        public void Exit(IPlayer player) { }

        public void HandleInput(IPlayer player) { }

        public void Update(IPlayer player, GameTime gameTime) { }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
            var effects = (player.FacingDirection == Direction.Left) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var source = SpriteFactory.GetCrouchSprite(player.FacingDirection, player.CurrentFrame);
            // Calculate the height difference between standing and crouching sprites
            var standingHeight = SpriteFactory.GetIdleSprite(player.FacingDirection, 0).Height; // Assuming GetIdleSprite exists
            var crouchingHeight = source.Height;
            var heightDifference = standingHeight - crouchingHeight;
            var adjustedPosition = new Vector2(player.Position.X, player.Position.Y + (heightDifference) / 2);

            spriteBatch.Draw(player.SpriteSheet, adjustedPosition, source, color, 0f, Vector2.Zero, 1f, effects, 0f);
        }
    }
}