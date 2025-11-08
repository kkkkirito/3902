using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;

namespace Sprint_0.Enemies
{
    public class WosuEnemy : Enemy
    {

        public WosuEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateWosuAnimations(spriteSheet), startPos)
        {
            this.CanMove = true;
            this.CanJump = false;
            this.CanAttack = false;
            this.CanCrouch = false;
            this.Position = startPos;
            this.BoundingBox = new Rectangle((int)startPos.X, (int)startPos.Y, 15, 31);

            ChangeState(new MoveState());
        }
    }
}