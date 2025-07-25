using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Pong;

class Bar(Texture2D texture, int xPosition, int worldHeight, SoundEffect soundEffect)
{
    readonly int xPosition = xPosition - Width / 2;
    int yPosition = worldHeight / 2 - Height / 2;

    public const int Height = 80;
    public const int Width = 10;

    const int InitialMovementLength = 3;
    const int MaxMovementLength = 10;
    int movementLength = InitialMovementLength;

    public void IncreaseMovementSpeed()
    {
        if (movementLength > MaxMovementLength)
        {
            // Set some limit
            return;
        }

        movementLength += InitialMovementLength;
    }

    public void ResetHorizontalMovementSpeed() => movementLength = InitialMovementLength;

    public int GetPositionX() => xPosition;

    public int GetPositionY() => yPosition;

    public void MoveUp()
    {
        if (yPosition > 0)
        {
            yPosition -= movementLength;
        }
    }

    public void MoveDown()
    {
        if (yPosition < worldHeight - Height)
        {
            yPosition += movementLength;
        }
    }

    public Texture2D GetTexture() => texture;

    public int YCenterOfBar() => yPosition + Height / 2;

    public void PlaySound() => soundEffect.Play();
}