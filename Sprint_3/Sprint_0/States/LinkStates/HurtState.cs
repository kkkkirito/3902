using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.States.LinkStates
{
    public class HurtState : IPlayerState
    {
        public SpriteEffects s = SpriteEffects.None;

        public void Enter(IPlayer player)
        {
            player.Velocity = Vector2.Zero;
            AudioManager.PlaySound(AudioManager.HurtSound, 0.9f);
        }

        public void Exit(IPlayer player)
        {
            // Clean up
        }

        public void HandleInput(IPlayer player)
        {
            // Can't control during hurt state
        }

        public void Update(IPlayer player, GameTime gameTime)
        {

        }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
            var effects = (player.FacingDirection == Direction.Left) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle source = SpriteFactory.GetHurtSprite(player.FacingDirection, player.CurrentFrame);
            spriteBatch.Draw(player.SpriteSheet, player.Position, source, color, 0f, Vector2.Zero, 1f, effects, 0f);
        }
    }
}
