using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint_0.Interfaces;
using Sprint_0.Enemies;

namespace Sprint_0.EnemyStateMachine
{
	internal class StalfosFallState : IEnemyState
	{

        public void Start(Enemy enemy)
        {
            
            enemy.SetAnimation("Fall");
            enemy.Velocity = new Vector2(0, 150f); // pixels per second downward
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
           
            if (enemy.Position.Y >= 200) // Update Later for ground level
            {
                enemy.Position = new Vector2(enemy.Position.X, 200);
                enemy.Velocity = Vector2.Zero;

                enemy.ChangeState(new CrouchState());
            }
        }
        public void Done(Enemy enemy) { }
	}
}
