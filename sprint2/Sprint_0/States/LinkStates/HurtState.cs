using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0.States.LinkStates
{
    public class HurtState : IPlayerState
    {
        private float hurtDuration = 0.5f;
        private float hurtTimer;
        private InputState state = new InputState();
        public SpriteEffects s = SpriteEffects.None;

        public void Enter(IPlayer player)
        {
            player.Velocity = Vector2.Zero;
            hurtTimer = 0;
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
            hurtTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            /*if (hurtTimer >= hurtDuration)
            {
                player.ChangeState(new IdleState());
                
            }*/
        }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
            var effects = (player.FacingDirection == Direction.Left) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle source = SpriteFactory.GetHurtSprite(player.FacingDirection, player.CurrentFrame);
            spriteBatch.Draw(player.SpriteSheet, player.Position, source, color, 0f, Vector2.Zero, 1f, effects, 0f);
        }
    }
}
