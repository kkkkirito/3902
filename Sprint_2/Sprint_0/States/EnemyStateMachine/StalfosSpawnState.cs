using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0.EnemyStateMachine
{
	internal class StalfosSpawnState : IEnemyState
	{

        public void Start(Enemy enemy)
        {
            
            enemy.SetAnimation("Falling");
            enemy.Velocity = new Vector2(0, 150f); // pixels per second downward
        }
        public void Update(Enemy enemy, GameTime gameTime)
        {
           
            if (enemy.Position.Y >= 200) // Update Later for ground level
            {
                enemy.Position = new Vector2(enemy.Position.X, 200);
                enemy.Velocity = Vector2.Zero;

                enemy.ChangeState(new IdleState());
            }
        }
        public void Done(Enemy enemy) { }
	}
}
