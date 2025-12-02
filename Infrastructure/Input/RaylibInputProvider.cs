using Raylib_cs;
using Flappy.Application.Interfaces;

namespace Flappy.Infrastructure.Input;

public class RaylibInputProvider : IInputProvider
{
    public bool IsJumpPressed() => Raylib.IsKeyPressed(KeyboardKey.Space);
    public bool IsRestartPressed() => Raylib.IsKeyPressed(KeyboardKey.Space);
    public bool IsStartPressed() => Raylib.IsKeyPressed(KeyboardKey.Space);
    public bool IsDebugPressed() => Raylib.IsKeyPressed(KeyboardKey.F1);
}
