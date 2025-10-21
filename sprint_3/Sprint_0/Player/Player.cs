//Dillon Brigode AU25
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;
using Sprint_0.States.LinkStates;
using System;
using System.Collections.Generic;

//Link class for managing Link.
namespace Sprint_0.Player_Namespace;

public class Player : IPlayer, ICollidable
{

    private readonly IController _controller;
    private IPlayerState _currentState;
    private InputState state = new InputState();

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Speed { get; private set; } = 100f;
    public Direction FacingDirection { get; set; } = Direction.Down;

    private const float Gravity = 1200f;
    private const float JumpStrength = -350f;
    private float VerticalVelocity = 0f;

    private float groundY;
    public bool IsGrounded { get; set; } = true;
    public bool IsCrouching { get; private set; } = false;

    private ICollectible _heldItem;
    public ICollectible HeldItem => _heldItem;

    private GameModeType _gameMode = GameModeType.Platformer;
    public GameModeType GameMode
    {
        get => _gameMode;
        set
        {
            if (_gameMode == value) return;
            _gameMode = value;

            // When entering Platformer, snap the "floor" to the player's current feet Y
            if (_gameMode == GameModeType.Platformer)
            {
                groundY = Position.Y;
            }

            // normalize state when modes change
            IsCrouching = false;
            VerticalVelocity = 0f;
            IsGrounded = true;

            // don't interrupt pickup when changing modes
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

    // Health and game-specific properties
    public int MaxHealth { get; private set; } = 100;
    public int CurrentHealth { get; set; }
    public bool IsInvulnerable { get; set; }
    public float InvulnerabilityTimer { get; set; }

    // Animation properties not sure if needed here or in states/sprite factory
    public Rectangle SourceRectangle { get; set; }
    public float AnimationTimer { get; set; }
    public int CurrentFrame { get; set; }

    public Rectangle BoundingBox { get; set; }
    private PlayerAttackHitbox attackHitBox;

    //constructor for player.
    public Player(Texture2D spriteSheet, Vector2 startPosition, IController controller)
    {
        AnimationTimer = 0;
        SpriteSheet = spriteSheet;
        Position = startPosition;
        groundY = startPosition.Y;
        CurrentHealth = MaxHealth;
        _controller = controller;
        attackHitBox = new PlayerAttackHitbox(this);

        // Start in idle state
        ChangeState(new IdleState());
    }

    public void Update(GameTime gameTime)
    {
        // auto-uncrouch if the hold command didn't run this frame
        IsCrouching = IsCrouching && IsGrounded;

        // Handle invulnerability timer
        if (IsInvulnerable)
        {
            InvulnerabilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (InvulnerabilityTimer <= 0)
            {
                IsInvulnerable = false;
            }
        }
        // If we were hurt and invulnerability ended, return to idle
        if (_currentState is HurtState && !IsInvulnerable)
        {
            ChangeState(new IdleState());
        }
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


        // Apply movement for this frame
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (GameMode == GameModeType.Platformer)
        {
            Position = new Vector2(Position.X + Velocity.X * dt, Position.Y);
            if (!IsGrounded)
            {
                VerticalVelocity += Gravity * dt;
            }
            Position = new Vector2(Position.X, Position.Y + VerticalVelocity * dt);
            if (Position.Y >= groundY)
            {
                Position = new Vector2(Position.X, groundY);
                VerticalVelocity = 0f;
                if (!IsGrounded)
                {
                    IsGrounded = true;
                    // land -> return to idle/moving
                    if (_currentState is JumpState) ChangeState(new IdleState());
                }
            }
            else
            {
                IsGrounded = false;
            }
        }
        else
        {
            Position += Velocity * dt;
            VerticalVelocity = 0f;
            IsGrounded = true;
        }

        // Reset horizontal speed only (keep vertical physics alive)
        Velocity = new Vector2(0f, 0f);

        // Time-based animation for walking (matches AnimatedFromRects behavior)
        if (_currentState is MovingState)
        {
            const float frameTime = 0.15f; // seconds per frame for walking
            AnimationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (AnimationTimer >= frameTime)
            {
                // There are 3 walking frames defined in SpriteFactory.GetWalkingSprite
                CurrentFrame = (CurrentFrame + 1) % 3;
                AnimationTimer -= frameTime;
            }
        }
        else if (!(_currentState is AttackState) && !(_currentState is JumpState) && !(_currentState is PickupState))
        {
            // Reset when not walking
            CurrentFrame = 0;
            AnimationTimer = 0f;
        }
        BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, 16, 32);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //Updated color logic to white since we implemented the hurt sprite (red)
        var color = IsInvulnerable ? Color.White : Color.White;
        _currentState.Draw(this, spriteBatch, color);
    }

    public void ChangeState(IPlayerState newState)
    {
        CurrentState = newState;
    }

    public void Move(Vector2 direction)
    {
        // Disable while hurt/attacking/crouching/picking up
        if (_currentState is HurtState || _currentState is AttackState || _currentState is PickupState || IsCrouching) return;

        // Use only X for platforming-style movement
        if (CanMove(direction))
        {
            Velocity = new Vector2(direction.X * Speed, 0f);
            UpdateFacingDirection(direction);
        }
        if (GameMode == GameModeType.Platformer)
        {
            // X only
            Velocity = new Vector2(direction.X * Speed, 0f);
            UpdateFacingDirection(direction);
        }
        else
        {
            // 2D movement
            Velocity = direction * Speed;
            if (Math.Abs(direction.X) >= Math.Abs(direction.Y))
                UpdateFacingDirection(direction);
            else
                FacingDirection = direction.Y < 0 ? Direction.Up : Direction.Down;
        }
    }

    public void Jump()
    {
        if (GameMode != GameModeType.Platformer) return; // disable in TopDown
        if (IsGrounded && !IsCrouching && !(_currentState is HurtState) && !(_currentState is AttackState))
        {
            VerticalVelocity = JumpStrength;
            IsGrounded = false;
            ChangeState(new JumpState());
        }
    }

    public bool CanMove(Vector2 direction)
    {
        // Add collision detection logic here ex: walls or obstacles.
        // For now, always return true
        return true;
    }

    public void TakeDamage(int damage)
    {
        // Ignore if currently invulnerable
        if (IsInvulnerable) return;

        CurrentHealth = Math.Max(0, CurrentHealth - damage);
        IsInvulnerable = true;
        InvulnerabilityTimer = 0.5f; // Match HurtState visual duration

        if (CurrentHealth <= 0)
        {
            ChangeState(new DeadState());
            return;
        }
        if (Velocity.X > 0) FacingDirection = Direction.Right;
        else if (Velocity.X < 0) FacingDirection = Direction.Left;
        ChangeState(new HurtState());
    }

    private void UpdateFacingDirection(Vector2 direction)
    {
        if (direction.X > 0) FacingDirection = Direction.Right;
        else if (direction.X < 0) FacingDirection = Direction.Left;
    }
    public void Attack(Direction direction, AttackMode mode = AttackMode.Normal)
    {
        if (_currentState is HurtState || _currentState is AttackState) return;
        FacingDirection = direction;
        Velocity = Vector2.Zero;
        if (!IsGrounded && mode == AttackMode.DownThrust)
        {
            //stab downwards.
            VerticalVelocity = Math.Max(VerticalVelocity, 400f);
        }
        else if (!IsGrounded && mode == AttackMode.UpThrust)
        {
            //stab upwards, not a second jump just a boost.
            VerticalVelocity = Math.Min(VerticalVelocity, -250f);
        }
        ChangeState(new AttackState(mode));
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

        // Add the attack hitbox while attacking
        if (CurrentState is AttackState)
        {
            // Empty hitbox means inactive; only add if non-empty
            var hb = attackHitBox.BoundingBox;
            if (!hb.IsEmpty) list.Add(attackHitBox);
        }

        return list;
    }
}

