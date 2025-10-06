//Dillon brigode AU25
using Microsoft.Xna.Framework;
using Sprint_0.Interfaces;
namespace Sprint_0.Command.PlayerCommand
{
    public class MoveDownOrCrouchOnCommand : ICommand
    {
        private readonly IPlayer _player;
        public MoveDownOrCrouchOnCommand(IPlayer player) => _player = player;

        public void Execute()
        {
            if (_player.GameMode == GameModeType.TopDown)
            {
                _player.Move(new Vector2(0f, +1f));   // walk down
            }
            else // Platformer
            {
                _player.SetCrouch(true);           // hold to crouch
            }
        }
    }
}