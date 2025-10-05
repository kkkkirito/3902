using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.EnemyStateMachine;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0.Enemies
{
    public class BotEnemy : Enemy
    {

        public BotEnemy(Texture2D spriteSheet, Vector2 startPos)
            : base(SpriteFactory.CreateBotAnimations(spriteSheet), startPos)
        {
            this.CanMove = true;
            this.CanJump = true;
            this.CanAttack = false;
            this.CanCrouch = false;
            this.Position = startPos;

            ChangeState(new IdleState());
        }
    }
}
