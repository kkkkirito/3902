using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;

namespace Sprint_0.Enemies
{
    public class WosuEnemy : Enemy
    {
        public WosuEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateWosuAnimations(spriteSheet), startPos, new EnemyConfig
                {
                    CanMove = true,
                    CanIdle = false,
                    LockFacing = true,
                    DropItemOnDeath = true,
                    BoundingBoxSize = new Rectangle(0, 0, 15, 31),
                    UseGravity = true
                })
        {
            this.SpriteSheet = spriteSheet;
            ChangeState(new MoveState());
        }
    }
}