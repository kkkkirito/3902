using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;

namespace Sprint_0.Enemies
{
    public class OverworldBotEnemy : Enemy
    {
        public OverworldBotEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateOverworldBotAnimations(spriteSheet), startPos)
        {
            this.Position = startPos;
            ChangeState(new OverworldIdleState());
        }
    }

}
