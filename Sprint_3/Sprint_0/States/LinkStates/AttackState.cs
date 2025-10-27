using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.States.LinkStates
{
    public class AttackState : IPlayerState
    {
        private readonly AttackMode _mode;
        public AttackMode Mode => _mode;
        private float attackDuration = 0.5f;
        private float attackTimer;
        private InputState state = new InputState();
        private const int totalFrames = 2; // matches SpriteFactory.GetAttackingSprite count
        public AttackState() : this(AttackMode.Normal) { }

        public AttackState(AttackMode mode)
        {
            _mode = mode;
        }

        public void Enter(IPlayer player)
        {
            player.Velocity = Vector2.Zero;
            attackTimer = 0;
            player.CurrentFrame = 0;
        }

        public void Exit(IPlayer player)
        {
            // Clean up attack state
        }

        public void HandleInput(IPlayer player)
        {
            //state = new InputState();
        }

        public void Update(IPlayer player, GameTime gameTime)
        {
            attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update attack animation across totalFrames; clamp at last frame
            int frame = (int)((attackTimer / attackDuration) * totalFrames);
            if (frame >= totalFrames) frame = totalFrames - 1;
            player.CurrentFrame = frame;

            if (attackTimer >= attackDuration)
            {
                player.CurrentState = new IdleState();
            }
        }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
            Rectangle source;
            switch (_mode)
            {
                case AttackMode.UpThrust:
                    source = SpriteFactory.GetUpThrustSprite(player.FacingDirection, player.CurrentFrame); 
                    break;
                case AttackMode.DownThrust:
                    source = SpriteFactory.GetDownThrustSprite(player.FacingDirection, player.CurrentFrame);
                    break;
                case AttackMode.Crouch:

                    source = SpriteFactory.GetCrouchAttackSprite(player.FacingDirection, player.CurrentFrame);

                    break;
                default:
                    source = SpriteFactory.GetAttackingSprite(player.FacingDirection, player.CurrentFrame);
                    break;
            }
            var effects = (player.FacingDirection == Direction.Left)
    ? SpriteEffects.FlipHorizontally
    : SpriteEffects.None;

            var standingHeight = SpriteFactory.GetIdleSprite(player.FacingDirection, 0).Height; // Assuming GetIdleSprite exists
            var crouchingHeight = source.Height;
            var heightDifference = standingHeight - crouchingHeight;

            // Start with base position adjustment for crouching
            var adjustedPosition = new Vector2(player.Position.X, player.Position.Y + (heightDifference / 2));
            if (player.FacingDirection == Direction.Left)
            {
                adjustedPosition.X -= 8; // Move left by 15 pixels when flipped
            }

            spriteBatch.Draw(
                player.SpriteSheet,
                adjustedPosition,
                source,
                color,
                0f,
                Vector2.Zero,
                1f,
                effects,
                0f
            );
        }
    }
}
