using Flappy.Domain.Entities;

namespace Flappy.Application.Interfaces;

public interface IRenderer
{
    void BeginDrawing();
    void EndDrawing();
    void DrawBackground();
    void DrawBird(Bird bird);
    void DrawPipe(Pipe pipe);
    void DrawTitleScreen();
    void DrawGameOver(int score);
    void DrawScore(int score);
    void DrawDebug(Bird bird, List<Pipe> pipes);
    bool ShouldClose();
    float GetDeltaTime();
}
