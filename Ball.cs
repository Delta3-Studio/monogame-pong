using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Pong;

class Ball(Texture2D texture, int xPosition, int worldWidth, int worldHeight, SoundEffect soundEffect)
{
    const int BarHeight = 80;
    const int BarWidth = 10;
    const int TopAndBottomPadding = 20;

    int xPosition = xPosition;
    int yPosition = worldHeight / 2;

    int verticalMovement;
    int horizontalMovement = 5;

    int previousPosX = xPosition;

    public void IncreaseHorizontalMovement()
    {
        if (Math.Abs(horizontalMovement) > 10)
        {
            return;
        }

        if (horizontalMovement > 0)
        {
            horizontalMovement += 1;
        }
        else
        {
            horizontalMovement -= 1;
        }
    }

    public void ResetHorizontalMovement() => horizontalMovement = 5;

    public Texture2D GetTexture() => texture;

    public int GetPositionX() => xPosition;

    public int GetPositionY() => yPosition;

    public void Move()
    {
        previousPosX = xPosition;
        MoveVertically();
        MoveHorizontally();
    }

    public void MoveToCenter()
    {
        xPosition = yPosition = worldHeight / 2;
        BounceHorizontal();
        verticalMovement = 0;
    }

    void MoveHorizontally()
    {
        if (xPosition > worldWidth || xPosition < 0)
        {
            BounceHorizontal();
        }

        xPosition += horizontalMovement;
    }

    void MoveVertically()
    {
        if (yPosition > worldHeight - TopAndBottomPadding || yPosition < 0 + TopAndBottomPadding)
        {
            BounceVertical();
            PlayEdgeSound();
        }


        yPosition += verticalMovement;
    }

    public void CheckHit(Bar playerBar, Bar cpuBar)
    {
        if (IsCollision(playerBar))
        {
            Swing(playerBar);
            BounceHorizontal();
            playerBar.PlaySound();

            playerBar.IncreaseMovementSpeed();
        }

        if (IsCollision(cpuBar))
        {
            Swing(cpuBar);
            BounceHorizontal();
            cpuBar.PlaySound();

            cpuBar.IncreaseMovementSpeed();
        }
    }

    void Swing(Bar bar)
    {
        verticalMovement = OffCollision(bar);
        IncreaseHorizontalMovement();
    }

    void PlayEdgeSound()
    {
        soundEffect.Play();
    }

    bool IsCollision(Bar bar) => IsCollisionX(bar) && IsCollisionY(bar);

    bool IsCollisionX(Bar bar) =>
        xPosition <= bar.GetPositionX() + BarWidth && xPosition >= bar.GetPositionX();

    int OffCollision(Bar bar)
    {
        // That's some interesting bit, how should the ball bounce when colliding with the bar??
        // Let's make it simple: the further away from the middle of the bar, the least horizontally the ball will go 
        var diff = yPosition - bar.YCenterOfBar();
        var off = (diff) / 2;
        return off;
    }

    public bool IsCollisionY(Bar bar) =>
        yPosition < bar.GetPositionY() + BarHeight && yPosition > bar.GetPositionY();


    void BounceVertical() => verticalMovement *= -1;

    void BounceHorizontal() => horizontalMovement *= -1;

    public bool IsGoingRight() => previousPosX < xPosition;
}