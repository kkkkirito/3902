using Microsoft.Xna.Framework;
using Sprint_0.Interfaces;
using Sprint_0.States.LinkStates;

namespace Sprint_0.Player_Namespace
{
    public sealed class PlayerAttackHitbox : ICollidable
    {
        private readonly IPlayer player;

        public PlayerAttackHitbox(IPlayer player) => this.player = player;

        public Rectangle BoundingBox
        {
            get
            {
                if (player?.CurrentState is not AttackState atk) return Rectangle.Empty;

                Rectangle body = player.BoundingBox;
                int w = 16, h = 16; // tune per sprite
                var pos = player.Position;

                switch (atk.Mode)
                {
                    case AttackMode.UpThrust:
                        // A box above Link’s head
                        return new Rectangle((int)pos.X, (int)(pos.Y - h), body.Width, h);

                    case AttackMode.DownThrust:
                        // A box below Link’s feet
                        return new Rectangle((int)pos.X, (int)(pos.Y + body.Height), body.Width, h);

                    case AttackMode.Crouch:
                        // A low, short box to the facing side
                        if (player.FacingDirection == Direction.Right)
                            return new Rectangle((int)(pos.X + body.Width), (int)(pos.Y + body.Height - h), w, h);
                        else
                            return new Rectangle((int)(pos.X - w), (int)(pos.Y + body.Height - h), w, h);

                    default: // Normal stab
                        if (player.FacingDirection == Direction.Right)
                            return new Rectangle((int)(pos.X + body.Width), (int)(pos.Y + body.Height / 2 - h / 2), w, h);
                        else
                            return new Rectangle((int)(pos.X - w), (int)(pos.Y + body.Height / 2 - h / 2), w, h);
                }
            }
        }
    }
}