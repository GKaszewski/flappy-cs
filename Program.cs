using Raylib_cs;
using Flappy.Application.Core;
using Flappy.Infrastructure.Graphics;
using Flappy.Infrastructure.Input;

namespace Flappy;

class Program
{
    static void Main(string[] args)
    {
        Raylib.InitWindow(GameConstants.OG_WIDTH * GameConstants.SCALE, GameConstants.OG_HEIGHT * GameConstants.SCALE, "Flappy");

        var assetManager = new RaylibAssetManager();
        var inputProvider = new RaylibInputProvider();
        var renderer = new RaylibRenderer(assetManager);

        var game = new GameEngine(renderer, inputProvider, assetManager);
        game.Run();

        Raylib.CloseWindow();
    }
}