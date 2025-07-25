namespace Pong;

class CpuController(Bar cpuBar, Ball ball, int worldHeight)
{
    const int MovementLength = 5;

    const int BarHeight = 80;

    const int ProximityTolerance = 3;

    public void UpdatePosition()
    {
        MoveCloseToBall();
    }

    public void MoveCloseToBall()
    {
        if (Math.Abs(ball.GetPositionY() - cpuBar.YCenterOfBar()) > ProximityTolerance)
        {
            MoveCloserTo(ball.GetPositionY());
        }
    }

    void AdjustToCenter()
    {
        if (!ball.IsGoingRight())
        {
            MoveCloserTo(worldHeight / 2);
        }
    }

    void MoveCloserTo(int point)
    {
        var middleOfBar = cpuBar.GetPositionY() + BarHeight / 2;
        if (middleOfBar < point)
        {
            cpuBar.MoveDown();
        }
        if (middleOfBar > point)
        {
            cpuBar.MoveUp(); ;
        }
    }
}