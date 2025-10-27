//Dillon Brigode AU25
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;


namespace Sprint_0.States.LinkStates
{
    public class PickupState : IPlayerState
    {
        private const int totalFrames = 4;
        private const float frameTime = 0.25f; // seconds per frame
        private float timer;

        public void Enter(IPlayer player)
        {
            player.Velocity = Vector2.Zero;
            player.CurrentFrame = 0;
            timer = 0f;
        }

        public void Exit(IPlayer player) { }

        public void HandleInput(IPlayer player) { }

        public void Update(IPlayer player, GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer >= frameTime)
            {
                timer -= frameTime;
                player.CurrentFrame++;

                if (player.CurrentFrame >= totalFrames)
                {
                    player.CurrentState = new IdleState();
                    return;
                }
            }
        }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
            var effects = (player.FacingDirection == Direction.Left) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var source = SpriteFactory.GetPickupSprite(player.FacingDirection, player.CurrentFrame);
            spriteBatch.Draw(player.SpriteSheet, player.Position, source, color, 0f, Vector2.Zero, 1f, effects, 0f);
        }
    }
}