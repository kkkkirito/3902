using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;

namespace Sprint_0.Enemies
{
    public class BotEnemy : Enemy
    {

        public BotEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateBotAnimations(spriteSheet), startPos)
        {
            this.SpriteSheet = spriteSheet;
            this.CanMove = true;
            this.CanJump = true;
            this.CanAttack = false;
            this.CanCrouch = false;
            this.Position = startPos;
            this.BoundingBox = new Rectangle((int)startPos.X, (int)startPos.Y, 14, 13);

            XPReward = 2;

            ChangeState(new IdleState());
        }
    }
}
