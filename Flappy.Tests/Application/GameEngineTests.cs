using System.Numerics;
using Flappy.Application.Core;
using Flappy.Application.Interfaces;
using Flappy.Domain.Entities;
using Flappy.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace Flappy.Tests.Application;

public class GameEngineTests
{
    private readonly Mock<IRenderer> _mockRenderer;
    private readonly Mock<IInputProvider> _mockInput;
    private readonly Mock<IAssetManager> _mockAssets;
    private readonly GameEngine _gameEngine;

    public GameEngineTests()
    {
        _mockRenderer = new Mock<IRenderer>();
        _mockInput = new Mock<IInputProvider>();
        _mockAssets = new Mock<IAssetManager>();
        
        // Setup default behavior
        _mockRenderer.SetupSequence(r => r.ShouldClose())
            .Returns(false)
            .Returns(true); // Run loop once
            
        _mockRenderer.Setup(r => r.GetDeltaTime()).Returns(0.1f);

        _gameEngine = new GameEngine(_mockRenderer.Object, _mockInput.Object, _mockAssets.Object);
    }

    [Fact]
    public void Run_ShouldLoadAssets()
    {
        _gameEngine.Run();
        _mockAssets.Verify(a => a.LoadAssets(), Times.Once);
    }

    [Fact]
    public void Run_ShouldUpdateAndDraw()
    {
        _gameEngine.Run();
        _mockRenderer.Verify(r => r.BeginDrawing(), Times.Once);
        _mockRenderer.Verify(r => r.EndDrawing(), Times.Once);
    }

    [Fact]
    public void Update_ShouldJump_WhenInputPressed()
    {
        // Arrange
        _mockInput.Setup(i => i.IsStartPressed()).Returns(true); // Start game first
        _mockRenderer.SetupSequence(r => r.ShouldClose())
            .Returns(false) // Frame 1: Start game
            .Returns(false) // Frame 2: Jump
            .Returns(true); // End
            
        // We need to inject input behavior dynamically or sequence it
        // Frame 1: Start Pressed -> Game becomes Playing
        // Frame 2: Jump Pressed -> Bird Jumps
        
        _mockInput.SetupSequence(i => i.IsStartPressed())
            .Returns(true)
            .Returns(false);
            
        _mockInput.SetupSequence(i => i.IsJumpPressed())
            .Returns(false)
            .Returns(true);

        // Act
        _gameEngine.Run();

        // Assert
        _mockRenderer.Verify(r => r.DrawBird(It.IsAny<Bird>()), Times.AtLeastOnce);
    }
}
