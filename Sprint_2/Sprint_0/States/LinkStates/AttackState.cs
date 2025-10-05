using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.States.LinkStates
{ 
    public class AttackState : IPlayerState
    {
        private float attackDuration = 0.5f;
        private float attackTimer;
        private InputState state = new InputState();
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
            state = new InputState();
        }

        public void Update(IPlayer player, GameTime gameTime)
        {
            attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update attack animation
            player.CurrentFrame = (int)((attackTimer / attackDuration) * 3); // 3 attack frames

            if (attackTimer >= attackDuration)
            {
                player.CurrentState = new IdleState();
            }
        }

        public void Draw(IPlayer player, SpriteBatch spriteBatch, Color color)
        {
            //Rectangle source = SpriteFactory.GetAttackSprite(player.FacingDirection, player.CurrentFrame);
            //spriteBatch.Draw(player.SpriteSheet, player.Position, source, color);
        }

       
    }
}
