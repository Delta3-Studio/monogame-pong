using Microsoft.Xna.Framework.Audio;

namespace Pong;

class BallOutManager(Ball ball, int worldWidth, SoundEffect soundEffect, Score score)
{
    const int ResetBallLatencyInFrames = 60;
    const int LeftAndRightPadding = 10;

    int latchDelay;

    public void Move() => ball.Move();

    public void ResetBallAfterLatency()
    {
        if (latchDelay == 1)
        {
            ball.MoveToCenter();
        }
        else
        {
            PlayBallOutSound();
            InitDelay();
        }

        latchDelay--;
    }

    void InitDelay()
    {
        if (latchDelay == 0)
        {
            latchDelay = ResetBallLatencyInFrames;
        }
    }

    public bool IsBallOut()
    {
        if (IsOutLeft())
        {
            IncrementCpuScore();
            return true;

        }
        if (IsOutRight())
        {
            IncrementPlayerSCore();
            return true;
        }
        return false;
    }

    void IncrementCpuScore()
    {
        if (latchDelay == 0)
        {
            score.IncrementCpuScore();
        }
    }

    void IncrementPlayerSCore()
    {
        if (latchDelay == 0)
        {
            score.IncrementPlayerScore();
        }
    }

    bool IsOutRight()
    {
        return ball.GetPositionX() > worldWidth - LeftAndRightPadding;
    }

    bool IsOutLeft()
    {
        return ball.GetPositionX() < LeftAndRightPadding;
    }

    void PlayBallOutSound()
    {
        if (latchDelay == 60 - 1)
        {
            soundEffect.Play();
        }
    }

    public Ball GetBall() => ball;
}