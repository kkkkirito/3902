﻿using Microsoft.Xna.Framework;
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
                HandleLanding();
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
                _player.VerticalVelocity += PlayerConstants.Gravity * dt;
        }

        private void ApplyPosition(float dt)
        {
            _player.Position += new Vector2(
                _player.Velocity.X * dt,
                _player.VerticalVelocity * dt
            );
        }

        private void HandleLanding()
        {
            if (_player.Position.Y < _player.groundY)
            {
                _player.IsGrounded = false;
                return;
            }

            _player.Position = new Vector2(_player.Position.X, _player.groundY);
            _player.VerticalVelocity = 0;
            _player.IsGrounded = true;

            if (_player.CurrentState is JumpState)
                _player.ChangeState(new IdleState());
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
