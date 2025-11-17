using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.States.LinkStates
{
    public class DeadState : IPlayerState
    {
        private SpriteEffects _effects = SpriteEffects.None;

        public void Enter(IPlayer player)
        {
            player.Velocity = Vector2.Zero;
            AudioManager.PlaySound(AudioManager.DeathSound, 0.9f);
        }

        public void Exit(IPlayer player)
        {
            
        }

        public void HandleInput(IPlayer player)
        {
        }

        public void Update(IPlayer player, GameTime gameTime)
        {
        }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
            if (player.FacingDirection == Direction.Left)
            {
                _effects = SpriteEffects.FlipHorizontally;
            }
            
            Rectangle source = SpriteFactory.GetHurtSprite(player.FacingDirection, player.CurrentFrame);
            spriteBatch.Draw(player.SpriteSheet, player.Position, source, Color.Black, 0f, Vector2.Zero, 1f, _effects, 0f);
        }
    }
}