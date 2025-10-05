using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
