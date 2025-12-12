using System;
using Microsoft.Xna.Framework;
using Sprint_0.Managers;
using Sprint_0.States.LinkStates;

namespace Sprint_0.Player_Namespace
{
    public class PlayerCombat
    {
        private readonly Player _player;
        private readonly IAudioManager _audio;

        public PlayerCombat(Player player, IAudioManager audio)
        {
            _player = player;
            _audio = audio;
        }

        public void Attack(Direction direction, AttackMode mode = AttackMode.Normal)
        {
            if (_player.CurrentState is HurtState or AttackState)
                return;

            _player.FacingDirection = direction;
            _player.Velocity = Vector2.Zero;

            ApplyAttackThrust(mode);
            _player.ChangeState(new AttackState(mode));
        }
        private void ApplyAttackThrust(AttackMode mode)
        {
            if (!_player.IsGrounded && mode == AttackMode.DownThrust)
                _player.VerticalVelocity = Math.Max(_player.VerticalVelocity, 400f);
            else if (!_player.IsGrounded && mode == AttackMode.UpThrust)
                _player.VerticalVelocity = Math.Min(_player.VerticalVelocity, -250f);
        }

        public void TakeDamage(int damage)
        {
            if (_player.CurrentState is DeadState)
                return;
            if (_player.IsInvulnerable)
                return;

            _player.CurrentHealth = Math.Max(0, _player.CurrentHealth - damage);
            _player.IsInvulnerable = true;
            _player.InvulnerabilityTimer = 0.5f;


            if (_player.CurrentHealth <= 0)
            {
                
                if (_player.Lives > 1)
                {
                    _player.Lives -= 1;                          
                   
                    _player.LivesAvailable = true;
                }
                else
                {

                    _player.LivesAvailable = false;
                }
                _player.IsDying = true;
                _player.ChangeState(new DeadState(_audio));
            }
            else
            {
                _player.ChangeState(new HurtState(_audio));
            }
        }

        public void UpdateInvulnerability(GameTime gameTime)
        {
            if (!_player.IsInvulnerable)
                return;

            _player.InvulnerabilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_player.InvulnerabilityTimer <= 0)
                _player.IsInvulnerable = false;
        }
    }
}
