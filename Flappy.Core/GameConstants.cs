namespace Flappy;

public static class GameConstants
{
    public const int OG_WIDTH = 144;
    public const int OG_HEIGHT = 312;
    public const int SCALE = 2;
    public const float MULTIPLIER = 10.0f;
    public const float GRAVITY = 10.0f;

    // Texture dimensions (hardcoded for now as they were implicitly used)
    public const int BIRD_WIDTH = 17; // Approximate, need to check assets or original code logic
    public const int BIRD_HEIGHT = 12;
    public const int PIPE_WIDTH = 26;
    public const int PIPE_HEIGHT = 160; // Just a guess, need to check. 
    // Actually, let's not hardcode dimensions here yet if we can avoid it, 
    // but the original code used texture width/height directly.
    // For pure domain, we might need these. 
    // Let's stick to what was in Global for now.
}
