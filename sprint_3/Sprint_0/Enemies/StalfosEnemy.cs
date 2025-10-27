using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;

namespace Sprint_0.Enemies
{
    public class StalfosEnemy : Enemy
    {
        public StalfosEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateStalfosAnimations(spriteSheet), startPos)

        {
            this.CanMove = true;
            this.CanJump = false;
            this.CanAttack = true;
            this.CanCrouch = true;
            this.Position = startPos;
            this.BoundingBox = new Rectangle((int)startPos.X, (int)startPos.Y, 13, 30);
            this.IsGrounded = false; 
            ChangeState(new StalfosFallState());
        }
    }
}
