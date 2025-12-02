using System.Numerics;
using Flappy.Domain.Enums;

namespace Flappy.Domain.Entities;

public class Bird
{
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }
    public BirdState State { get; private set; }

    private const float Gravity = 10.0f;
    private const float JumpForce = -5.0f;

    // Dimensions for collision (approximated from usage)
    public const int Width = 17;
    public const int Height = 12;

    public Bird(Vector2 startPosition)
    {
        Position = startPosition;
        State = BirdState.Idle;
        Velocity = Vector2.Zero;
    }

    public void Jump()
    {
        Velocity = new Vector2(0, JumpForce * GameConstants.MULTIPLIER);
        State = BirdState.Flying;
    }

    public void Update(float deltaTime)
    {
        if (Velocity.Y > 0)
        {
            State = BirdState.Falling;
        }

        if (Velocity == Vector2.Zero)
        {
            State = BirdState.Idle;
        }

        // Apply gravity
        // Note: In original code: velocity.Y += gravity * Global.MULTIPLIER * Global.deltaTime;
        // position += velocity * Global.deltaTime;

        var newVelocityY = Velocity.Y + Gravity * GameConstants.MULTIPLIER * deltaTime;
        Velocity = new Vector2(Velocity.X, newVelocityY);

        Position += Velocity * deltaTime;
    }

    public void Stop()
    {
        Velocity = Vector2.Zero;
    }
}
