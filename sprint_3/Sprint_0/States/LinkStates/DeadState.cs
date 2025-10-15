using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.States.LinkStates
{
    public class DeadState : IPlayerState
    {
        private SpriteEffects s = SpriteEffects.None;
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
            if (player.FacingDirection == Direction.Left)
            {
                s = SpriteEffects.FlipHorizontally;
            }
            Rectangle source = SpriteFactory.GetIdleSprite(player.FacingDirection, player.CurrentFrame);
            spriteBatch.Draw(player.SpriteSheet, player.Position, source, Color.Red, 0f, Vector2.Zero, 1f, s, 0f);
        }
    }
}