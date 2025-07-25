using Microsoft.Xna.Framework.Graphics;

namespace Pong;

class Score(SpriteFont font)
{
    int playerScore;
    int cpuScore;

    const int ScoreToWin = 9;

    public SpriteFont GetFont() => font;

    public void IncrementPlayerScore() => playerScore++;

    public void IncrementCpuScore() => cpuScore++;

    public int GetPlayerScore() => playerScore;

    public int GetCpuScore() => cpuScore;

    public bool IsGameFinished() => playerScore >= ScoreToWin || cpuScore >= ScoreToWin;

    public void Reset()
    {
        cpuScore = 0;
        playerScore = 0;
    }

    public bool PlayerWin() => playerScore >= ScoreToWin;

    public bool CpuWin() => cpuScore >= ScoreToWin;
}