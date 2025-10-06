using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint_0
{
    public static class SpriteFactory
    {
        #region EnemySprites
        internal static Dictionary<string, Animation> CreateStalfosAnimations(Texture2D spriteSheet)
        {
            var idleFrames = new List<Rectangle>
            {
                new Rectangle(499, 10, 17, 33)
            };

            var walkingFrames = new List<Rectangle>
            {
                new Rectangle(499, 11, 15, 32),
                new Rectangle(516, 11, 15, 32)
            };

            var idleAttackFrames = new List<Rectangle>
            {
                new Rectangle(499, 11, 15, 32),
                new Rectangle(533, 11, 15, 32),
                new Rectangle(567, 11, 30, 32)
            };

            var walkingAttackFrames = new List<Rectangle>
            {
                new Rectangle(499, 11, 15, 31),
                new Rectangle(550, 11, 15, 31),
                new Rectangle(567, 11, 30, 32)
            };
            var attackOffsets = new List<Vector2>
            {
                Vector2.Zero,
                Vector2.Zero,
                new Vector2(-15, 0),
            };
            var fallingFrames = new List<Rectangle>
            {
                new Rectangle(649, 10, 17, 33)
            };

            var crouchingFrames = new List<Rectangle>
            {
                new Rectangle(633, 11, 15, 26)
            };
            var crouchOffsets = new List<Vector2>
            {
                new Vector2(0, 6)
            };


            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(spriteSheet, idleFrames, 0.25f, true) },
                { "Move", new Animation(spriteSheet, walkingFrames, 0.15f, true) },
                { "IdleAttack", new Animation(spriteSheet, idleAttackFrames, 0.2f, false, attackOffsets) },
                { "MoveAttack", new Animation(spriteSheet, walkingAttackFrames, 0.2f, false, attackOffsets) },
                { "Fall", new Animation(spriteSheet, fallingFrames, 0.2f, false) },
                { "Crouch", new Animation(spriteSheet, crouchingFrames, 0.2f, false, crouchOffsets) }
            };

        }
        internal static Dictionary<string, Animation> CreateBotAnimations(Texture2D spriteSheet)
        {
            var idleFrames = new List<Rectangle>
            {
                new Rectangle(176, 10, 17, 17),
                new Rectangle(210, 10, 15, 17)

            };

            var movingFrames = new List<Rectangle>
            {
                new Rectangle(176, 10, 17, 17),
                new Rectangle(194, 10, 15, 17)
            };

            var stunnedFrames = new List<Rectangle>
            {
                new Rectangle(176, 10, 17, 17),
                new Rectangle(210, 10, 15, 17)
            };

            var jumpFrames = new List<Rectangle>
            {
                new Rectangle(194, 10, 15, 17)
            };

            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(spriteSheet, idleFrames, 0.25f, true) },
                { "Move", new Animation(spriteSheet, movingFrames, 0.15f, true) },
                { "Stunned", new Animation(spriteSheet, stunnedFrames, 0.2f, false) },
                { "Jump", new Animation(spriteSheet, jumpFrames, 0.2f, false) }
            };
        }
        internal static Dictionary<string, Animation> CreateOctorokAnimations(Texture2D spriteSheet)
        {
            var idleFrames = new List<Rectangle>
            {
                new Rectangle(291, 182, 16, 16),
                new Rectangle(308, 182, 16, 16)
            };

            var attackingFrames = new List<Rectangle>
            {
                new Rectangle(291, 182, 16, 16),
                new Rectangle(308, 182, 16, 16)
            };

            var projectileFrames = new List<Rectangle>
            {
                new Rectangle(292, 200, 7, 15),
                new Rectangle(301, 200, 7, 15)
            };

            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(spriteSheet, idleFrames, 0.2f, true) },
                { "Attack", new Animation(spriteSheet, attackingFrames, 0.1f, false) },
                { "Projectile", new Animation(spriteSheet, projectileFrames, 0.2f, true) },
            };
        }
        internal static Dictionary<string, Animation> CreateOverworldBotAnimations(Texture2D spriteSheet)
        {
            var idleFrames = new List<Rectangle>
            {
                new Rectangle(468, 426, 18, 15)
            };

            var movingFrames = new List<Rectangle>
            {
                new Rectangle(448, 426, 18, 15),
                new Rectangle(468, 426, 18, 15)
            };

            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(spriteSheet, idleFrames, 0.25f, true) },
                { "MoveLeft", new Animation(spriteSheet, movingFrames, 0.15f, true) },
                { "MoveRight", new Animation(spriteSheet, movingFrames, 0.15f, true) },
                { "MoveUp", new Animation(spriteSheet, movingFrames, 0.15f, true) },
                { "MoveDown", new Animation(spriteSheet, movingFrames, 0.15f, true) },
            };
        }

        internal static Dictionary<string, Animation> CreateOverworldManAnimations(Texture2D spriteSheet)
        {
            var idleFrames = new List<Rectangle>
            {
                new Rectangle(468, 399, 18, 24)
            };

            var movingFrames = new List<Rectangle>
            {
                new Rectangle(448, 399, 18, 24),
                new Rectangle(468, 399, 18, 24)
            };

            return new Dictionary<string, Animation>
            {
                { "Idle", new Animation(spriteSheet, idleFrames, 0.25f, true) },
                { "MoveLeft", new Animation(spriteSheet, movingFrames, 0.15f, true) },
                { "MoveRight", new Animation(spriteSheet, movingFrames, 0.15f, true) },
                { "MoveUp", new Animation(spriteSheet, movingFrames, 0.15f, true) },
                { "MoveDown", new Animation(spriteSheet, movingFrames, 0.15f, true) },
            };
        }

        #endregion
        #region PlayerSprites
        internal static Rectangle GetIdleSprite(Direction facingDirection, int currentFrame)
        {
            Rectangle idleFrames = new Rectangle(0, 10, 18, 42);
            return idleFrames;
        }
        internal static Rectangle GetWalkingSprite(Direction facingDirection, int currentFrame)
        {
            var movingFrames = new List<Rectangle>
            {
                new Rectangle(29, 11, 15, 31),
                new Rectangle(47, 12, 14, 30),
                new Rectangle(63, 11, 15, 31)
            };
            int frameIndex = movingFrames.Count == 0 ? 0 : currentFrame % movingFrames.Count;
            return movingFrames[frameIndex];
        }

        internal static Rectangle GetAttackingSprite(Direction facingDirection, int currentFrame)
        {
            var attackFrames = new List<Rectangle>
            {
                //two frame for stab attack.
                new Rectangle(1, 73, 22, 31),

                new Rectangle(27, 75, 30, 29),

                //Ducking attack.
                //new Rectangle(67, 79, 31, 25),
                //upward attack
                //new Rectangle(127, 75, 15, 29),
                //downward attack
                //new Rectangle(189, 73, 16, 31)
            };
            int frameIndex = attackFrames.Count == 0 ? 0 : currentFrame % attackFrames.Count;
            return attackFrames[frameIndex];
        }
        internal static Rectangle GetJumpSprite(Direction facingDirection, int currentFrame)
        {
            var jumpFrames = new List<Rectangle>
            {
                // Jumping pose 1 (ascending)
                new Rectangle(117, 16, 16, 27),
                // Jumping pose 2 (at peak)
                new Rectangle(134, 11, 16, 32)
            };
            int frameIndex = jumpFrames.Count == 0 ? 0 : currentFrame % jumpFrames.Count;
            return jumpFrames[frameIndex];
        }
        internal static Rectangle GetBeamSprite(Direction facingDirection, int currentFrame)
        {
            var beamFrames = new List<Rectangle>
            {
                new Rectangle(238, 68, 8, 3),
                new Rectangle(247, 68, 7, 3)
            };
            int frameIndex = beamFrames.Count == 0 ? 0 : currentFrame % beamFrames.Count;
            return beamFrames[frameIndex];
        }
        internal static Rectangle GetFireballSprite(Direction facingDirection, int currentFrame)
        {
            var fireFrames = new List<Rectangle>
            {
                new Rectangle(238,94,8,10),
                new Rectangle(247, 92, 8, 10)
            };
            int frameIndex = fireFrames.Count == 0 ? 0 : currentFrame % fireFrames.Count;
            return fireFrames[frameIndex];
        }

        internal static Rectangle GetCrouchSprite(Direction facingDirection, int currentFrame)
        {
            Rectangle crouchFrame = new Rectangle(84, 16, 16, 27);
            return crouchFrame;
        }
        internal static Rectangle GetHurtSprite(Direction facingDirection, int currentFrame)
        {
            Rectangle hurtFrame = new Rectangle(217, 130, 15, 28);
            //new Rectangle(217, 130, 15, 28);
            return hurtFrame;
        }

        internal static Rectangle GetCrouchAttackSprite(Direction facingDirection, int currentFrame)
        {
            var crouchAttackFrames = new List<Rectangle>
            {
                new Rectangle(67, 79, 32, 26)
            };
            int frameIndex = crouchAttackFrames.Count == 0 ? 0 : currentFrame % crouchAttackFrames.Count;
            return crouchAttackFrames[frameIndex];
        }

        internal static Rectangle GetUpThrustSprite(Direction facingDirection, int currentFrame)
        {
            var upThrustFrames = new List<Rectangle>
            {
                new Rectangle(190, 73, 16, 32)
            };
            int frameIndex = upThrustFrames.Count == 0 ? 0 : currentFrame % upThrustFrames.Count;
            return upThrustFrames[frameIndex];
        }

        internal static Rectangle GetDownThrustSprite(Direction facingDirection, int currentFrame)
        {
            var downThrustFrames = new List<Rectangle>
            {
                new Rectangle(127, 75, 16, 30)
            };
            int frameIndex = downThrustFrames.Count == 0 ? 0 : currentFrame % downThrustFrames.Count;
            return downThrustFrames[frameIndex];
        }
        internal static Rectangle GetPickupSprite(Direction facingDirection, int currentFrame)
        {
            var pickupFrames = new List<Rectangle>
            {
                new Rectangle(188, 11, 16, 32),
                new Rectangle(205, 11, 16, 32),
                new Rectangle(264, 11, 16, 32),
                new Rectangle(281, 11, 16, 32)
            };
            int frameIndex = Math.Clamp(currentFrame, 0, pickupFrames.Count - 1);
            return pickupFrames[frameIndex];
        }
        #endregion
        #region ItemSprites
        internal static Dictionary<string, Animation> CreateItemAnimations(Texture2D spriteSheet)
        {
            var dict = new Dictionary<string, Animation>();

            // Treasure Bag
            dict["TreasureBag"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(1, 12, 8, 15)
            }, 0.25f, false);

            // Small Magic Jar (3 colors)
            dict["SmallMagicJar_Blue"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(12, 12, 8, 15)
            }, 0.25f, false);

            dict["SmallMagicJar_LightBlue"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(21, 12, 8, 15)
            }, 0.25f, false);

            dict["SmallMagicJar_Teal"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(30, 12, 8, 15)
            }, 0.25f, false);

            // Large Magic Jar (3 colors)
            dict["LargeMagicJar_DarkRed"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(41, 12, 8, 15)
            }, 0.25f, false);
            dict["LargeMagicJar_Red"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(50, 12, 8, 15)
            }, 0.25f, false);
            dict["LargeMagicJar_LightRed"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(59, 12, 8, 15)
            }, 0.25f, false);

            // Doll
            dict["Doll"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(71, 12, 7, 15)
            }, 0.25f, false);

            // Fairy (animated, 2 frames)
            dict["Fairy"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(81, 12, 8, 15),
                new Rectangle(90, 12, 8, 15)
            }, 0.25f, true);

            // Key
            dict["Key"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(101, 12, 8, 15)
            }, 0.25f, false);

            // Candle
            dict["Candle"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(129, 11, 8, 15)
            }, 0.25f, false);

            // Hammer
            dict["Hammer"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(138, 13, 7, 14)
            }, 0.25f, false);

            // Glove
            dict["Glove"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(147, 12, 8, 15)
            }, 0.25f, false);

            // Raft
            dict["Raft"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(156, 12, 8, 14)
            }, 0.25f, false);

            // Boots
            dict["Boots"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(166, 13, 7, 13)
            }, 0.25f, false);

            // Whistle
            dict["Whistle"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(175, 12, 6, 14)
            }, 0.25f, false);

            // Cross
            dict["Cross"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(183, 13, 8, 12)
            }, 0.25f, false);

            // Magic Key
            dict["MagicKey"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(192, 11, 8, 15)
            }, 0.25f, false);

            // Heart
            dict["Heart"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(203, 12, 16, 15)
            }, 0.25f, false);

            // Container
            dict["Container"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(222, 11, 12, 16)
            }, 0.25f, false);

            // Trophy
            dict["Trophy"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(240, 12, 14, 15)
            }, 0.25f, false);

            // Water of Life
            dict["WaterOfLife"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(259, 11, 10, 16)
            }, 0.25f, false);

            // Child
            dict["Child"] = new Animation(spriteSheet, new List<Rectangle>
            {
                new Rectangle(276, 11, 11, 16)
            }, 0.25f, false);

            return dict;
        }
        #endregion
    }
}
