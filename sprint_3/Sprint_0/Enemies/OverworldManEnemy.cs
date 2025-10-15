using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;

namespace Sprint_0.Enemies
{   // Don't know what the enemy is called, can not find it in the sprites but its the exact same movement as Overworld Bot 
    public class OverworldManEnemy : Enemy
    {
        public OverworldManEnemy(Texture2D spriteSheet, Vector2 startPos)
             : base(SpriteFactory.CreateOverworldManAnimations(spriteSheet), startPos)
        {
            this.Position = startPos;
            ChangeState(new OverworldIdleState());
        }
    }
}
