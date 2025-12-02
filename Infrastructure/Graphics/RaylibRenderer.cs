using System.Numerics;
using Raylib_cs;
using Flappy.Application.Interfaces;
using Flappy.Domain.Entities;
using Flappy.Domain.Enums;

namespace Flappy.Infrastructure.Graphics;

public class RaylibRenderer : IRenderer
{
    private readonly RaylibAssetManager _assets;
    private RenderTexture2D _renderTexture;

    public RaylibRenderer(RaylibAssetManager assets)
    {
        _assets = assets;
        // Note: Raylib.InitWindow must be called before this constructor is used
        _renderTexture = Raylib.LoadRenderTexture(GameConstants.OG_WIDTH, GameConstants.OG_HEIGHT);
        Raylib.SetTextureFilter(_renderTexture.Texture, TextureFilter.Point);
    }

    public void BeginDrawing()
    {
        Raylib.BeginDrawing();
        Raylib.BeginTextureMode(_renderTexture);
        Raylib.ClearBackground(Color.Black);
    }

    public void EndDrawing()
    {
        Raylib.EndTextureMode();

        Raylib.DrawTexturePro(_renderTexture.Texture,
            new Rectangle(0, 0, GameConstants.OG_WIDTH, -GameConstants.OG_HEIGHT),
            new Rectangle(0, 0, GameConstants.OG_WIDTH * GameConstants.SCALE, GameConstants.OG_HEIGHT * GameConstants.SCALE),
            new Vector2(0, 0), 0,
            Color.White);

        Raylib.EndDrawing();
    }

    public void DrawBackground()
    {
        Raylib.DrawTexture(_assets.Background, 0, 0, Color.White);
        Raylib.DrawTexture(_assets.Ground, 0, 256, Color.White);
    }

    public void DrawBird(Bird bird)
    {
        Texture2D texture = bird.State switch
        {
            BirdState.Idle => _assets.IdleBird,
            BirdState.Flying => _assets.FlyBird,
            BirdState.Falling => _assets.FallBird,
            _ => _assets.IdleBird
        };

        Raylib.DrawTexture(texture, (int)bird.Position.X, (int)bird.Position.Y, Color.White);
    }

    public void DrawPipe(Pipe pipe)
    {
        Texture2D texture = pipe.Type == PipeType.Up ? _assets.PipeUp : _assets.PipeDown;
        Raylib.DrawTexture(texture, (int)pipe.Position.X, (int)pipe.Position.Y, Color.White);
    }

    public void DrawTitleScreen()
    {
        Raylib.DrawTexture(_assets.Title, GameConstants.OG_WIDTH / 2 - _assets.Title.Width / 2, 50, Color.White);
        Raylib.DrawTexture(_assets.Ready, GameConstants.OG_WIDTH / 2 - _assets.Ready.Width / 2, 75, Color.White);
        Raylib.DrawText("Press SPACE to start", GameConstants.OG_WIDTH / 2 - 50, 110, 10, Color.White);
    }

    public void DrawGameOver(int score)
    {
        Raylib.DrawTexture(_assets.GameOver, GameConstants.OG_WIDTH / 2 - _assets.GameOver.Width / 2, 50, Color.White);
        Raylib.DrawTexture(_assets.ScoreBoard, GameConstants.OG_WIDTH / 2 - _assets.ScoreBoard.Width / 2, 100, Color.White);
        Raylib.DrawTexture(score < 100 ? _assets.SilverMedal : _assets.GoldMedal, 30, 122, Color.White);
        Raylib.DrawText(score.ToString(), 98, 116, 10, Color.White);
    }

    public void DrawScore(int score)
    {
        Raylib.DrawText(score.ToString(), GameConstants.OG_WIDTH / 2 - 10, 10, 20, Color.White);
    }

    public void DrawDebug(Bird bird, List<Pipe> pipes)
    {
        Raylib.DrawRectangleLines((int)bird.Position.X, (int)bird.Position.Y, Bird.Width, Bird.Height, Color.Red);
        foreach (var pipe in pipes)
        {
            Raylib.DrawRectangleLines((int)pipe.Position.X, (int)pipe.Position.Y, Pipe.Width, Pipe.Height, Color.Red);
        }
    }

    public bool ShouldClose() => Raylib.WindowShouldClose();
    public float GetDeltaTime() => Raylib.GetFrameTime();
}
