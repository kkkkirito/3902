using Sprint_0.Interfaces;
using Sprint_0.States.LinkStates;
using Microsoft.Xna.Framework.Input;

namespace Sprint_0.Command.PlayerCommand
{
    public class AttackCommand : ICommand
    {
        private readonly IPlayer player;

        public AttackCommand(IPlayer player)
        {
            this.player = player;
        }

        public void Execute()
        {
            if (player != null)
            {
                var ks = Keyboard.GetState();
                AttackMode mode = AttackMode.Normal;
                if (!player.IsGrounded && ks.IsKeyDown(Keys.W))
                    mode = AttackMode.UpThrust;
                else if (!player.IsGrounded && ks.IsKeyDown(Keys.S))
                    mode = AttackMode.DownThrust;
                else if (player.IsGrounded && player.IsCrouching)
                    mode = AttackMode.Crouch;

                player.Attack(player.FacingDirection, mode);
            }
        }
    }
}
