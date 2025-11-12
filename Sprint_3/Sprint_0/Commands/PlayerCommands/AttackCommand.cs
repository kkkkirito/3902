using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Interfaces;
using Sprint_0.Managers;

namespace Sprint_0.Commands.PlayerCommands
{
    public class AttackCommand : ICommand
    {
        private readonly IPlayer player;
        private readonly SoundEffect attackSound;

        public AttackCommand(IPlayer player)
        {
            this.player = player;
            this.attackSound = attackSound;
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

                AudioManager.PlaySound(AudioManager.AttackSound, 0.8f);
                player.Attack(player.FacingDirection, mode);
            }
        }
    }
}
