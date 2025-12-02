namespace Flappy.Application.Interfaces;

public interface IInputProvider
{
    bool IsJumpPressed();
    bool IsRestartPressed();
    bool IsStartPressed();
    bool IsDebugPressed();
}
