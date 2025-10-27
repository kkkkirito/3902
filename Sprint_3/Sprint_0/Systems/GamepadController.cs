using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint_0.Interfaces;
using Sprint_0.Commands.PlayerCommands;

namespace Sprint_0.Systems
{
    public class GamepadController : IController
    {
        private readonly PlayerIndex index;
        private readonly Dictionary<Buttons, ICommand> press = new();
        private readonly Dictionary<Buttons, ICommand> hold = new();
        private readonly Dictionary<Buttons, ICommand> release= new();
        private GamePadState prev;
        private ICommand cmdUp, cmdDown, cmdLeft, cmdRight;
        private ICommand cmdCrouch, cmdCrouchOff;
        private Vector2 prevLeftStick;
        private const float NoDZ = 0f;
        private const float DZ = 0.2f;
        public static IPlayer ControlPlayer { get; set; }
        public int blockSwitch { get; set; } = 0;

        public GamepadController(PlayerIndex index = PlayerIndex.One)
        {
            this.index = index;
            prev = GamePad.GetState(index);
        }
        public void Press(Buttons btn, ICommand cmd) => press[btn] = cmd;
        public void BindHold(Buttons btn, ICommand cmd) => hold[btn] = cmd;
        public void BindRelease(Buttons btn, ICommand cmd) => release[btn] = cmd;

        //Joystick
        public void BindJoystick(IPlayer player)
        {
            cmdUp = new MoveUpCommand(player);
            cmdDown = new MoveDownCommand(player);
            cmdLeft = new MoveLeftCommand(player);
            cmdRight = new MoveRightCommand(player);
            cmdCrouch = new MoveDownOrCrouchOnCommand(player);
            cmdCrouchOff = new CrouchOffCommand(player);
        }

        public void Update()
        {
            var cur = GamePad.GetState(index);
            if (!cur.IsConnected) return;

            foreach (var kv in hold)
                if (cur.IsButtonDown(kv.Key)) 
                    kv.Value.Execute();

            foreach (var kv in press)
                if (cur.IsButtonDown(kv.Key) && !prev.IsButtonDown(kv.Key))
                    kv.Value.Execute();

            foreach (var kv in release)
                if (prev.IsButtonDown(kv.Key) && cur.IsButtonUp(kv.Key))
                    kv.Value.Execute();
            //Left stick control
            var ls = cur.ThumbSticks.Left;
            if (ls.X < NoDZ) cmdLeft.Execute();
            else if (ls.X > -NoDZ) cmdRight.Execute();
            if (ls.Y > DZ) cmdUp.Execute();
            else if (ls.Y < -DZ) cmdCrouch.Execute();
            if (prevLeftStick.Y < NoDZ && ls.Y >= NoDZ) cmdCrouchOff.Execute();

            prevLeftStick = ls;
            prev = cur;
        }

    }
}
