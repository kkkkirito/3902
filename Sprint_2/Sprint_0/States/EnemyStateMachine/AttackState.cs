using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0.EnemyStateMachine
{
    internal class AttackState : IEnemyState
    {
        private float attackDuration = 0.5f; // half a second attack
        private float timer;

        public void Start(Enemy enemy)
        {
            timer = 0f;
            enemy.Velocity = Vector2.Zero;
            enemy.SetAnimation("Attack");    
            // Hit Box Later                              
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= attackDuration)
            {
                enemy.ChangeState(new IdleState());
            }
        }

        public void Done(Enemy enemy) { }

        
    }
}
