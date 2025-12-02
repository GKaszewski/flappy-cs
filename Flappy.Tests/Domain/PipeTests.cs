using System.Numerics;
using Flappy.Domain.Entities;
using Flappy.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Flappy.Tests.Domain;

public class PipeTests
{
    [Fact]
    public void Update_ShouldMovePipeLeft()
    {
        var startX = 200f;
        var pipe = new Pipe(new Vector2(startX, 100), PipeType.Up);
        
        pipe.Update(0.1f);

        pipe.Position.X.Should().BeLessThan(startX);
    }
}
