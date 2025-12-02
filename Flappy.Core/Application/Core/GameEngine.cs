using System.Numerics;
using Flappy.Application.Interfaces;
using Flappy.Domain.Entities;
using Flappy.Domain.Enums;

namespace Flappy.Application.Core;

public class GameEngine
{
    private readonly IRenderer _renderer;
    private readonly IInputProvider _input;
    private readonly IAssetManager _assetManager;

    private Bird _bird;
    private List<Pipe> _pipes;
    private GameState _gameState;
    private int _score;
    private float _scoreTime;
    private const float ScoreRate = 0.75f;
    private bool _drawDebug;

    public GameEngine(IRenderer renderer, IInputProvider input, IAssetManager assetManager)
    {
        _renderer = renderer;
        _input = input;
        _assetManager = assetManager;
        _pipes = new List<Pipe>();
        _gameState = GameState.TitleScreen;
        _bird = new Bird(new Vector2(GameConstants.OG_WIDTH / 2 - Bird.Width / 2,
            GameConstants.OG_HEIGHT / 2 - Bird.Height / 2));
    }

    public void Run()
    {
        _assetManager.LoadAssets();

        while (!_renderer.ShouldClose())
        {
            var deltaTime = _renderer.GetDeltaTime();
            Update(deltaTime);
            Draw();
        }
    }

    private void Update(float deltaTime)
    {
        if (_input.IsDebugPressed())
        {
            _drawDebug = !_drawDebug;
        }

        switch (_gameState)
        {
            case GameState.TitleScreen:
                UpdateTitleScreen();
                break;
            case GameState.Playing:
                UpdateGame(deltaTime);
                break;
            case GameState.GameOver:
                UpdateGameOver();
                break;
        }
    }

    private void UpdateTitleScreen()
    {
        if (_input.IsStartPressed())
        {
            GeneratePipes();
            _gameState = GameState.Playing;
        }
    }

    private void UpdateGame(float deltaTime)
    {
        if (_input.IsJumpPressed())
        {
            _bird.Jump();
        }

        _bird.Update(deltaTime);

        foreach (var pipe in _pipes)
        {
            pipe.Update(deltaTime);
        }

        SetPipesPosition();
        CheckCollisions();
        CheckScore(deltaTime);



        if (_bird.Position.Y >= 256)
        {
            _bird.Stop(); // Stop velocity
            _gameState = GameState.GameOver;
        }
    }

    private void UpdateGameOver()
    {
        if (_input.IsRestartPressed())
        {
            _gameState = GameState.TitleScreen;
            _score = 0;
            _bird = new Bird(new Vector2(GameConstants.OG_WIDTH / 2 - Bird.Width / 2,
                GameConstants.OG_HEIGHT / 2 - Bird.Height / 2));
            _pipes.Clear();
        }
    }

    private void GeneratePipes()
    {
        var random = new Random();
        for (var i = 0; i < 5; i++)
        {
            var offset = random.Next(150, 317);
            var downY = random.Next(180, 201);
            var upY = random.Next(-100, 1);
            var x = GameConstants.OG_WIDTH + (i * 100) + offset;
            _pipes.Add(new Pipe(new Vector2(x, upY), PipeType.Up));
            _pipes.Add(new Pipe(new Vector2(x, downY), PipeType.Down));
        }
    }

    private void SetPipesPosition()
    {
        var offPipes = _pipes.Where(pipe => pipe.Position.X < 0).ToList();
        if (offPipes.Count == 0) return;

        var random = new Random();
        for (var i = 0; i < offPipes.Count; i += 2)
        {
            var offset = random.Next(150, 317);
            var downY = random.Next(180, 201);
            var upY = random.Next(-100, 1);

            // We need to find the rightmost pipe to append after?
            // Original logic: var x = Global.OG_WIDTH + (i * 100) + offset; 
            // Wait, original logic was weird. It reset X based on loop index?
            // "var x = Global.OG_WIDTH + (i * 100) + offset;" inside the loop over offPipes.
            // This seems to reset them relative to screen, not relative to last pipe.
            // But since they go off screen in pairs, maybe it works out.
            // Let's stick to original logic but be careful.
            // Actually, original code:
            // var offPipes = Global.pipes.Where(pipe => pipe.position.X < 0).ToList();
            // for (var i = 0; i < offPipes.Count; i += 2) { ... }
            // It seems it just recycles them to the right.

            var x = GameConstants.OG_WIDTH + (i * 100) + offset;
            // This X is relative to screen width, so it might overlap if we are not careful?
            // If i=0, x = 144 + 0 + 150 = 294.
            // If the last pipe is at 200, this puts it at 294.
            // Seems fine.

            if (i < offPipes.Count) offPipes[i].Position = new Vector2(x, upY);
            if (i + 1 < offPipes.Count) offPipes[i + 1].Position = new Vector2(x, downY);
        }
    }

    private void CheckCollisions()
    {
        // AABB Collision
        var birdRect = new { X = _bird.Position.X, Y = _bird.Position.Y, W = Bird.Width, H = Bird.Height };

        foreach (var pipe in _pipes)
        {
            var pipeRect = new { X = pipe.Position.X, Y = pipe.Position.Y, W = Pipe.Width, H = Pipe.Height };

            if (CheckCollision(birdRect.X, birdRect.Y, birdRect.W, birdRect.H,
                               pipeRect.X, pipeRect.Y, pipeRect.W, pipeRect.H))
            {
                _gameState = GameState.GameOver;
            }
        }
    }

    private bool CheckCollision(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
    {
        return x1 < x2 + w2 && x1 + w1 > x2 && y1 < y2 + h2 && y1 + h1 > y2;
    }

    private void CheckScore(float deltaTime)
    {
        foreach (var pipe in _pipes)
        {
            // Original logic: if (position.X > pipe.position.X && position.X < pipe.position.X + Global.pipeUpTexture.Width)
            if (_bird.Position.X > pipe.Position.X && _bird.Position.X < pipe.Position.X + Pipe.Width)
            {
                if (_scoreTime > ScoreRate)
                {
                    _score++;
                    _scoreTime = 0;
                }
                _scoreTime += deltaTime;
            }
        }
    }

    private void Draw()
    {
        _renderer.BeginDrawing();
        _renderer.DrawBackground();

        switch (_gameState)
        {
            case GameState.TitleScreen:
                _renderer.DrawTitleScreen();
                break;
            case GameState.Playing:
                foreach (var pipe in _pipes) _renderer.DrawPipe(pipe);
                _renderer.DrawBird(_bird);
                _renderer.DrawScore(_score);
                if (_drawDebug) _renderer.DrawDebug(_bird, _pipes);
                break;
            case GameState.GameOver:
                _renderer.DrawGameOver(_score);
                break;
        }

        _renderer.EndDrawing();
    }
}
