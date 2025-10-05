//Dillon Brigode AU25
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.States.LinkStates
{
    public class JumpState : IPlayerState
    {
        private float attackTimer;
        public void Enter(IPlayer player)
        {
            player.CurrentFrame = 0;
            attackTimer = 0f;
        }

        public void Exit(IPlayer player) { }

        public void HandleInput(IPlayer player) { }
        public void Update(IPlayer player, GameTime gameTime)
        {
            const float frameTime = 0.3f;
            attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (attackTimer >= frameTime)
            {
            player.CurrentFrame = (player.CurrentFrame + 1) % 2; // 2 jump frames
            attackTimer -= frameTime;
            }
        }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
            var effects = (player.FacingDirection == Direction.Left) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var source = SpriteFactory.GetJumpSprite(player.FacingDirection, player.CurrentFrame);
            spriteBatch.Draw(player.SpriteSheet, player.Position, source, color, 0f, Vector2.Zero, 1f, effects, 0f);

        }

    }
}
    
