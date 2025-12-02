using System.Numerics;
using Flappy.Domain.Entities;
using Flappy.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Flappy.Tests.Domain;

public class BirdTests
{
    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var startPos = new Vector2(100, 100);
        var bird = new Bird(startPos);

        bird.Position.Should().Be(startPos);
        bird.State.Should().Be(BirdState.Idle);
        bird.Velocity.Should().Be(Vector2.Zero);
    }

    [Fact]
    public void Jump_ShouldSetStateToFlying_AndSetUpwardVelocity()
    {
        var bird = new Bird(new Vector2(100, 100));
        bird.Jump();

        bird.State.Should().Be(BirdState.Flying);
        bird.Velocity.Y.Should().BeLessThan(0); // Up is negative Y in this coordinate system
    }

    [Fact]
    public void Update_ShouldApplyGravity()
    {
        var bird = new Bird(new Vector2(100, 100));
        bird.Update(0.1f);

        // Gravity is positive, so velocity.Y should increase (become less negative or more positive)
        bird.Velocity.Y.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Update_ShouldTransitionToFalling_WhenVelocityIsPositive()
    {
        var bird = new Bird(new Vector2(100, 100));
        bird.Jump(); // Velocity becomes negative

        // Simulate enough time for gravity to overcome jump force
        for (int i = 0; i < 100; i++) bird.Update(0.1f);

        bird.State.Should().Be(BirdState.Falling);
    }
}
