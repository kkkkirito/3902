using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using Sprint_0.Managers;

namespace Sprint_0.Enemies
{
    public class WosuEnemy : Enemy
    {
        public WosuEnemy(Texture2D spriteSheet, Vector2 startPos, IAudioManager audio)
            : base(SpriteFactory.CreateWosuAnimations(spriteSheet), startPos, new EnemyConfig
                {
                    CanMove = true,
                    CanIdle = false,
                    DropItemOnDeath = true,
                    BoundingBoxSize = new Rectangle(0, 0, 15, 31),
                    UseGravity = true
                }, audio)
        {
            this.SetSpriteSheet(spriteSheet);
            ChangeState(new MoveState(audio));
        }
    }
}