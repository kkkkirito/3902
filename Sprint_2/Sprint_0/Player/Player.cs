//Dillon Brigode
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Sprint_0.States.LinkStates;
using Sprint_0.Interfaces;
using Sprint_0;

//Link class for managing Link.

public class Player : IPlayer
{
    
    private readonly IController _controller;
    private IPlayerState _currentState;

    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Speed { get; private set; } = 100f;
    public Direction FacingDirection { get; set; } = Direction.Down;
    public IPlayerState CurrentState { get; set; } 
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
    //public int AnimationTimer { get; set; }

    //constructor for player.
    public Player(Texture2D spriteSheet, Vector2 startPosition, IController controller)
    {
        AnimationTimer = 0;
        SpriteSheet = spriteSheet;
        Position = startPosition;
        CurrentHealth = MaxHealth;
        _controller = controller;

        // Start in idle state
        ChangeState(new IdleState());
    }

    public void Update(GameTime gameTime)
    {
        // Handle invulnerability timer
        if (IsInvulnerable)
        {
            InvulnerabilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (InvulnerabilityTimer <= 0)
            {
                IsInvulnerable = false;
            }
        }

        // Process input through controller
        //TO -DO: change this if state pattern uses different signature
        var inputState = new InputState();

        // Update current state


        // Apply physics
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //TO-DO: This might need to be changed based on how drawing is handled in states/sprite factory
        var color = IsInvulnerable ? Color.Red * 0.5f : Color.White;
        _currentState.Draw(this, spriteBatch, color);
    }

    public void ChangeState(IPlayerState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }

    public void Move(Vector2 direction)
    {
        if (CanMove(direction))
        {
            Velocity = direction * Speed;
            UpdateFacingDirection(direction);
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
        if (!IsInvulnerable)
        {
            CurrentHealth = Math.Max(0, CurrentHealth - damage);
            IsInvulnerable = true;
            InvulnerabilityTimer = 0.5f; // 1.5 seconds of invulnerability

            if (CurrentHealth <= 0)
            {
                ChangeState(new DeadState());
            }
            else
            {
                ChangeState(new HurtState());
            }
        }
    }

    private void UpdateFacingDirection(Vector2 direction)
    {
        if (direction.X > 0) FacingDirection = Direction.Right;
        else if (direction.X < 0) FacingDirection = Direction.Left;
        else if (direction.Y > 0) FacingDirection = Direction.Down;
        else if (direction.Y < 0) FacingDirection = Direction.Up;
    }
    
    public void Attack()
    {
        Velocity = Vector2.Zero;
        ChangeState(new AttackState());
    }
}

