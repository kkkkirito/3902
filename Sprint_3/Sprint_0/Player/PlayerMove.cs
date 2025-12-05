using Microsoft.Xna.Framework;
using Sprint_0.States.LinkStates;
using System;

namespace Sprint_0.Player_Namespace
{
    public class PlayerMove
    {
        private readonly Player _player;

        public PlayerMove(Player player)
        {
            _player = player;
        }

        public void ApplyMovement(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_player.GameMode == GameModeType.Platformer)
            {
                ApplyGravity(dt);
                ApplyPosition(dt);
                ResetHorizontalVelocity();
            }
            else if (_player.GameMode == GameModeType.TopDown)
            {
                _player.Position += _player.Velocity * dt;
                _player.Velocity = Vector2.Zero;
            }
        }

        private void ApplyGravity(float dt)
        {
            if (!_player.IsGrounded)
            {
                _player.VerticalVelocity += PlayerConstants.Gravity * dt;
                const float MaxFallSpeed = 800f;
                if (_player.VerticalVelocity > MaxFallSpeed)
                {
                    _player.VerticalVelocity = MaxFallSpeed;
                }
            }
        }

        private void ApplyPosition(float dt)
        {
            _player.Position += new Vector2(
                _player.Velocity.X * dt,
                _player.VerticalVelocity * dt
            );
        }

        private void ResetHorizontalVelocity()
        {
            _player.Velocity = new Vector2(0, _player.Velocity.Y);
        }

        public void Jump()
        {
            if (_player.GameMode != GameModeType.Platformer || !_player.IsGrounded)
                return;

            _player.VerticalVelocity = PlayerConstants.JumpStrength;
            _player.IsGrounded = false;
            _player.ChangeState(new JumpState());
        }
    }
}
