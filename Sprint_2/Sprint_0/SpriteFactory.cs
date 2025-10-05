using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0
{
    public static class SpriteFactory
    {
        internal static Dictionary<string, Animation> CreateStalfosAnimations(Texture2D spriteSheet)
        {
            var idleFrames = new List<Rectangle>
            {
                new Rectangle(498, 10, 17, 33)
            };

            var walkingFrames = new List<Rectangle>
            {
                new Rectangle(498, 10, 17, 33),
                new Rectangle(515, 10, 17, 33)
            };

            var idleAttackFrames = new List<Rectangle>
            {
                new Rectangle(498, 10, 17, 33),
                new Rectangle(532, 10, 17, 33),
                new Rectangle(566, 10, 32, 33)
            };

            var walkingAttackFrames = new List<Rectangle>
            {
                new Rectangle (498, 10, 17, 33),
                new Rectangle(549, 10, 17, 33),
                new Rectangle(566, 10, 32, 33)
            };

            var fallingFrames = new List<Rectangle>
            {
                new Rectangle(649, 10, 17, 33)
            };

            var crouchingFrames = new List<Rectangle>
            {
                new Rectangle(632, 10, 17, 33)
            };

            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(spriteSheet, idleFrames, 0.25f, true) },
                { "Walking", new Animation(spriteSheet, walkingFrames, 0.15f, false) },
                { "IdleAttack", new Animation(spriteSheet, idleAttackFrames, 0.2f, false) },
                { "WalkingAttack", new Animation(spriteSheet, walkingAttackFrames, 0.2f, false) },
                { "Falling", new Animation(spriteSheet, fallingFrames, 0.2f, false) },
                { "Crouching", new Animation(spriteSheet, crouchingFrames, 0.2f, false) }
            };

        }
        internal static Dictionary<string, Animation> CreateBotAnimations(Texture2D spriteSheet)
        {
            var idleFrames = new List<Rectangle>
            {
                new Rectangle(176, 10, 17, 17)
            };

            var movingFrames = new List<Rectangle>
            {
                new Rectangle(176, 10, 17, 17),
                new Rectangle(193, 10, 17, 17)
            };

            var stunnedFrames = new List<Rectangle>
            {
                new Rectangle(176, 10, 17, 17),
                new Rectangle(210, 10, 15, 17)
            };

            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(spriteSheet, idleFrames, 0.25f, true) },
                { "Moving", new Animation(spriteSheet, movingFrames, 0.15f, false) },
                { "Stunned", new Animation(spriteSheet, stunnedFrames, 0.2f, false) },
            };
        }
        internal static Dictionary<string, Animation> CreateOctorokAnimations(Texture2D spriteSheet)
        {
            var idleFrames = new List<Rectangle>
            {
                new Rectangle(290, 181, 17, 17)
            };

            var attackingFrames = new List<Rectangle>
            {
                new Rectangle(290, 181, 17, 17),
                new Rectangle(193, 10, 17, 17)
            };

            var projectileFrames = new List<Rectangle>
            {
                new Rectangle(290, 198, 9, 17),
                new Rectangle(299, 198, 9, 17)
            };

            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(spriteSheet, idleFrames, 0.25f, true) },
                { "Attack", new Animation(spriteSheet, attackingFrames, 0.15f, false) },
                { "Projectile", new Animation(spriteSheet, projectileFrames, 0.2f, false) },
            };
        }
    }
}
