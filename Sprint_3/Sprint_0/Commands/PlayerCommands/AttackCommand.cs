using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.Commands.PlayerCommands
{
    public class AttackCommand: ICommand
    {

        private readonly IPlayer player;
        private readonly IAudioManager audio;

        public AttackCommand(IPlayer player, IAudioManager audio)
        {
            this.player = player;
            this.audio = audio;
        }
        public void Execute()
        {
            if (player != null)
            {
                if (!CooldownManager.CanExecute("Attack", 0.5))
                    return;

                var ks = Keyboard.GetState();
                AttackMode mode = AttackMode.Normal;
                if (!player.IsGrounded && ks.IsKeyDown(Keys.W))
                    mode = AttackMode.UpThrust;
                else if (!player.IsGrounded && ks.IsKeyDown(Keys.S))
                    mode = AttackMode.DownThrust;
                else if (player.IsGrounded && player.IsCrouching)
                    mode = AttackMode.Crouch;

                audio.PlayAttack();
                player.Attack(player.FacingDirection, mode);
            }
        }
    }
}
