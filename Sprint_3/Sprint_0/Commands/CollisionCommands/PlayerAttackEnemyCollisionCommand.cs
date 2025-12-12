using Microsoft.Xna.Framework;
using Sprint_0.Collision_System;
using Sprint_0.Enemies;
using Sprint_0.EnemyStateMachine;
using Sprint_0.Interfaces;
using Sprint_0.Player_Namespace;
using Sprint_0.States.LinkStates;

namespace Sprint_0.Commands.CollisionCommands
{
    public sealed class PlayerAttackEnemyCollisionCommand : ICollisionCommand
    {
        public void Execute(CollisionInfo info)
        {
            var hitbox = info.A as PlayerAttackHitbox ?? info.B as PlayerAttackHitbox;
            var enemy  = info.A as Enemy              ?? info.B as Enemy;
            if (hitbox == null || enemy == null) return;

            // Damage once per overlap (enemy handles its own i-frames)
            int damage = 10;

            // Slight positional nudge and knockback from MTV
            //enemy.Position += info.MinimumTranslationVector;
            Vector2 n = info.MinimumTranslationVector;
            if (n != Vector2.Zero) n.Normalize();
            if (!(enemy is BubbleEnemy))
            {
                enemy.Velocity = n * -10f;
            }
            else
            {

                 enemy.Velocity = new Vector2(0, 0);
                 enemy.ChangeState(new BubbleState());
            }


                enemy.TakeDamage(damage);
        }
    }
}