//Dillon Brigode AU25
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using Sprint_0.States.LinkStates;
using System;
using System.Collections.Generic;

namespace Sprint_0.Player_Namespace
{
    public class Player : IPlayer, ICollidable
    {
        private readonly IController _controller;
        private IPlayerState _currentState;
        private InputState state = new InputState();

        private readonly PlayerMove _movement;
        private readonly PlayerCombat _combat;
        private readonly PlayerAnimation _animation;

        public bool LivesAvailable { get; set; } = false;

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Direction FacingDirection { get; set; } = Direction.Right;
        public int MaxMagic { get; private set; } = 8;
        public int CurrentMagic { get; set; } = 8;
        public int CurrentXP { get; set; } = 0;
        public int NextLevelXP { get; private set; } = 100;
        public int Lives { get; set; } = 3;
        public int KeyCount { get; set; } = 0;

        private Vector2 _startPosition;
        private const float FALL_DEATH_Y = 500f;

        public bool IsGrounded { get; set; } = true;
        public bool IsCrouching { get; private set; } = false;

        private ICollectible _heldItem;
        public ICollectible HeldItem => _heldItem;

        public bool HasTorch { get; private set; }
        public float TorchLightRadius { get; private set; }
        public bool HasTopDownKey { get; set; } = false;

        private GameModeType _gameMode = GameModeType.Platformer;
        public bool IsDying { get; set; } = false;

        public float Speed { get; set; } = PlayerConstants.MaxHorizontalSpeed;
        public GameModeType GameMode
        {
            get => _gameMode;
            set
            {
                if (_gameMode == value) return;
                _gameMode = value;
                VerticalVelocity = 0f;

                if (_gameMode == GameModeType.TopDown)
                {
                    IsGrounded = true;
                }
                if (!(_currentState is PickupState))
                {
                    ChangeState(new IdleState());
                }
            }
        }
        public IPlayerState CurrentState
        {
            get => _currentState;
            set
            {
                if (!ReferenceEquals(_currentState, value) && value != null)
                {
                    _currentState?.Exit(this);
                    _currentState = value;
                    _currentState.Enter(this);
                }
            }
        }

        public Texture2D SpriteSheet { get; private set; }
        public int MaxHealth { get; private set; } = 100;
        public int CurrentHealth { get; set; }
        public bool IsInvulnerable { get; set; }
        public float InvulnerabilityTimer { get; set; }

        public Rectangle SourceRectangle { get; set; }
        public float AnimationTimer { get; set; }
        public int CurrentFrame { get; set; }

        public Rectangle BoundingBox { get; set; }
        private PlayerAttackHitbox attackHitBox;

        public float VerticalVelocity { get; set; }

        public Player(Texture2D spriteSheet, Vector2 startPosition, IController controller)
        {
            AnimationTimer = 0;
            SpriteSheet = spriteSheet;
            Position = startPosition;
            _startPosition = startPosition;
            CurrentHealth = MaxHealth;
            _controller = controller;
            attackHitBox = new PlayerAttackHitbox(this);

            // Start in idle state
            ChangeState(new IdleState());

            _movement = new PlayerMove(this);
            _combat = new PlayerCombat(this);
            _animation = new PlayerAnimation(this);
        }

        public void AddXP(int amount)
        {
            if (amount <= 0) return;
            CurrentXP += amount;
            while (CurrentXP >= NextLevelXP)
            {
                CurrentXP -= NextLevelXP;
                NextLevelXP = (int)(NextLevelXP * 1.5f);

                MaxMagic += 1;
                CurrentMagic = Math.Max(CurrentMagic + 1, MaxMagic);
                Lives += 1;
            }
        }

        public bool TrySpendMagic(int amount)
        {
            if (amount <= 0) return true;
            if (CurrentMagic < amount) return false;
            CurrentMagic -= amount;
            return true;
        }

        public void Update(GameTime gameTime)
        {
            // auto-uncrouch if the hold command didn't run this frame
            IsCrouching = IsCrouching && IsGrounded;

            _combat.UpdateInvulnerability(gameTime);
            _currentState?.Update(this, gameTime);

            // Only switch between moving/idle/crouch when NOT in non-interruptible states (Hurt/Attack/Pickup)
            if (IsGrounded && !(_currentState is HurtState) && !(_currentState is AttackState) && !(_currentState is PickupState))
            {
                if (IsCrouching)
                {
                    if (!(_currentState is CrouchState)) ChangeState(new CrouchState());
                }
                else if (
                    (GameMode == GameModeType.TopDown && (Math.Abs(Velocity.X) > 0.1f || Math.Abs(Velocity.Y) > 0.1f)) ||
                    (GameMode == GameModeType.Platformer && Math.Abs(Velocity.X) > 0.1f)
                )
                {
                    if (!(_currentState is MovingState)) ChangeState(new MovingState());
                }
                else
                {
                    if (!(_currentState is IdleState))
                        ChangeState(new IdleState());
                }
            }
            _movement.ApplyMovement(gameTime);
            _animation.Update(gameTime);
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, 16, 32);

            CheckFallDeath();
        }
        private void CheckFallDeath()
        {
            if (Position.Y > FALL_DEATH_Y && !IsDying)
            {
                TakeDamage(20); 
                if (CurrentHealth > 0)
                {
                    Position = _startPosition;
                    VerticalVelocity = 0f;
                    Velocity = Vector2.Zero;
                    IsGrounded = false; 
                }
            }
        }

        public void ChangeState(IPlayerState newState) => CurrentState = newState;

        public void Move(Vector2 direction)
        {
            // Disable while hurt/attacking/crouching/picking up
            if (_currentState is HurtState || _currentState is AttackState || _currentState is PickupState || IsCrouching) return;

            // Use only X for platforming-style movement
            if (GameMode == GameModeType.Platformer)
            {
                // X only
                Velocity = new Vector2(direction.X * Speed, 0f);
                UpdateFacingDirection(direction);
            }
            else
            {
                // 2D movement
                Velocity = direction * 100f;
                if (Math.Abs(direction.X) >= Math.Abs(direction.Y))
                    UpdateFacingDirection(direction);
                else
                    FacingDirection = direction.Y < 0 ? Direction.Up : Direction.Down;
            }
        }

        private void UpdateFacingDirection(Vector2 direction)
        {
            if (direction.X > 0) FacingDirection = Direction.Right;
            else if (direction.X < 0) FacingDirection = Direction.Left;
        }

        public void SetCrouch(bool crouch)
        {
            if (GameMode != GameModeType.Platformer) { IsCrouching = false; return; }
            if (!IsGrounded) { IsCrouching = false; return; }
            IsCrouching = crouch;
            if (IsCrouching && !(_currentState is CrouchState) && !(_currentState is AttackState))
                ChangeState(new CrouchState());
            if (!IsCrouching && _currentState is CrouchState)
                ChangeState(new IdleState());
        }

        public void Pickup(ICollectible item)
        {
            if (_currentState is HurtState || _currentState is AttackState || _currentState is PickupState) return; // can't pick up while hurt/attacking/picking up
            _heldItem = item;
            Velocity = Vector2.Zero;
            ChangeState(new PickupState());
        }

        public IEnumerable<ICollidable> GetCollidables()
        {
            var list = new List<ICollidable> { this };

            if (CurrentState is AttackState)
            {
                var hb = attackHitBox.BoundingBox;
                if (!hb.IsEmpty) list.Add(attackHitBox);
            }

            return list;
        }
        public void Jump() => _movement.Jump();
        public void Draw(SpriteBatch spriteBatch) => _animation.Draw(spriteBatch);
        public void TakeDamage(int damage) => _combat.TakeDamage(damage);
        public bool CanMove(Vector2 direction) => true;
        public void Attack(Direction direction, AttackMode mode = AttackMode.Normal) => _combat.Attack(direction, mode);

        public void EnableTorch(float radius)
        {
            if (radius <= 0f) radius = 1f;
            TorchLightRadius = Math.Max(TorchLightRadius, radius);
            HasTorch = true;
        }
        public void ResetToStart(Vector2 startPos, bool keepMagic, bool keepXP)
        {
            Position = startPos;
            _startPosition = startPos;
            CurrentHealth = MaxHealth;
            IsInvulnerable = false;
            Velocity = Vector2.Zero;
            VerticalVelocity = 0f;
            HasTopDownKey = false;
            IsGrounded = false; 

            if (!keepMagic) CurrentMagic = MaxMagic;
            if (!keepXP) CurrentXP = 0;

            if (CurrentState != null)
                ChangeState(new IdleState());
        }
    }
}

