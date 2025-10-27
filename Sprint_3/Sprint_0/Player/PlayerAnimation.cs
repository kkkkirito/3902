using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.States.LinkStates;

namespace Sprint_0.Player_Namespace
{
    public class PlayerAnimation
    {
        private readonly Player _player;
        private double _animationTimer = 0;

        public PlayerAnimation(Player player)
        {
            _player = player;
        }

        public void Update(GameTime gameTime)
        {
            if (_player.CurrentState is HurtState)
            {
                _player.InvulnerabilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_player.InvulnerabilityTimer <= 0)
                {
                    _player.IsInvulnerable = false;
                    _player.ChangeState(new IdleState());
                    return;
                }
            }
            if (_player.CurrentState is MovingState)
            {
                _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (_animationTimer >= 0.15f)
                {
                    _player.CurrentFrame = (_player.CurrentFrame + 1) % 3;
                    _animationTimer = 0;
                }
            }
            else
            {
                _animationTimer = 0;
            }

            _player.CurrentState.Update(_player, gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var color = _player.IsInvulnerable ? Color.White : Color.White;
            _player.CurrentState.Draw(_player, spriteBatch, color);
        }
    }
}