using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.States.LinkStates
{
    public class MovingState : IPlayerState
    {
        private Vector2 movementDirection;
        private InputState state = new InputState();
        private SpriteEffects s = SpriteEffects.None;
        public void Enter(IPlayer player)
        {
            //player.AnimationTimer = 0;
            player.CurrentFrame = 0;
        }

        public void Exit(IPlayer player)
        {
            player.Velocity = Vector2.Zero;
        }

        public void HandleInput(IPlayer player)
        {
            state = new InputState();
        }

        public void Update(IPlayer player, GameTime gameTime)
        {
            // Update walking animation
            //player.AnimationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (player.AnimationTimer > 0.1f) // Animation frame rate
            /*{
                player.CurrentFrame = (player.CurrentFrame + 1) % 4; // Assume 4 frames
                player.AnimationTimer = 0;
            }*/
        }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
            //Rectangle source = SpriteFactory.GetMovingSprite(player.FacingDirection, player.CurrentFrame);
            //spriteBatch.Draw(player.SpriteSheet, player.Position, source, color);
            var source = SpriteFactory.GetWalkingSprite(player.FacingDirection, player.CurrentFrame);
            var effects = (player.FacingDirection == Direction.Left) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(player.SpriteSheet, player.Position, source, color, 0f, Vector2.Zero, 1f, effects, 0f);
        }
    }
}
