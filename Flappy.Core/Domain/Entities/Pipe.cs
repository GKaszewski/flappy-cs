using System.Numerics;
using Flappy.Domain.Enums;

namespace Flappy.Domain.Entities;

public class Pipe
{
    public Vector2 Position { get; set; } // Setter needed for resetting position
    public PipeType Type { get; private set; }

    private const float Speed = 5.0f;

    // Dimensions
    public const int Width = 26;
    public const int Height = 160; // This is the texture height, but collision might be different? 
                                   // Original code used texture width/height. 
                                   // Let's assume standard size for now.

    public Pipe(Vector2 position, PipeType type)
    {
        Position = position;
        Type = type;
    }

    public void Update(float deltaTime)
    {
        // Original: position.X -= speed * Global.deltaTime * Global.MULTIPLIER;
        var newX = Position.X - Speed * deltaTime * GameConstants.MULTIPLIER;
        Position = new Vector2(newX, Position.Y);
    }
}
