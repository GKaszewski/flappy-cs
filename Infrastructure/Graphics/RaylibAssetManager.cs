using Raylib_cs;
using System.Reflection;
using Flappy.Application.Interfaces;

namespace Flappy.Infrastructure.Graphics;

public class RaylibAssetManager : IAssetManager
{
    public Texture2D Background { get; private set; }
    public Texture2D Ground { get; private set; }
    public Texture2D Title { get; private set; }
    public Texture2D Ready { get; private set; }
    public Texture2D IdleBird { get; private set; }
    public Texture2D FlyBird { get; private set; }
    public Texture2D FallBird { get; private set; }
    public Texture2D PipeUp { get; private set; }
    public Texture2D PipeDown { get; private set; }
    public Texture2D GameOver { get; private set; }
    public Texture2D SilverMedal { get; private set; }
    public Texture2D GoldMedal { get; private set; }
    public Texture2D ScoreBoard { get; private set; }

    public void LoadAssets()
    {
        Background = LoadTexture("Flappy.assets.bg.png");
        Ground = LoadTexture("Flappy.assets.ground.png");
        Title = LoadTexture("Flappy.assets.title.png");
        Ready = LoadTexture("Flappy.assets.ready.png");
        IdleBird = LoadTexture("Flappy.assets.idle.png");
        FlyBird = LoadTexture("Flappy.assets.jump.png");
        FallBird = LoadTexture("Flappy.assets.fall.png");
        PipeUp = LoadTexture("Flappy.assets.pipe_up.png");
        PipeDown = LoadTexture("Flappy.assets.pipe_down.png");
        GameOver = LoadTexture("Flappy.assets.game_over.png");
        SilverMedal = LoadTexture("Flappy.assets.silver_medal.png");
        GoldMedal = LoadTexture("Flappy.assets.gold_medal.png");
        ScoreBoard = LoadTexture("Flappy.assets.scoreboard_dead.png");
    }

    private Texture2D LoadTexture(string path)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(path);
        if (stream == null)
        {
            Console.WriteLine($"Could not find resource: {path}");
            return new Texture2D();
        }
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        var data = ms.ToArray();
        if (data.Length == 0) return new Texture2D();

        var image = Raylib.LoadImageFromMemory(".png", data);
        var texture = Raylib.LoadTextureFromImage(image);
        Raylib.UnloadImage(image);
        return texture;
    }
}
