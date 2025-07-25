using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;

public class Game1 : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    const int Height = 600;
    const int Width = 800;

    const int BarToEdgePadding = 40;

    Bar playerBar;
    Bar cpuBar;
    BallOutManager ballOutManager;

    CpuController cpuController;
    Score score;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        graphics.PreferredBackBufferHeight = Height;
        graphics.PreferredBackBufferWidth = Width;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);

        Content.RootDirectory = "Content";
        var barTexture = Content.Load<Texture2D>("PongBar");
        var ballTexture = Content.Load<Texture2D>("PongBall");

        var ballOutSound = Content.Load<SoundEffect>("ballOutSound");
        var ballCollideWithBarSound = Content.Load<SoundEffect>("ballBarCollision");
        var ballCollideWithEdgeSound = Content.Load<SoundEffect>("ballEdgeCollision");

        var music = Content.Load<SoundEffect>("music");
        var soundEffectInstance = music.CreateInstance();
        soundEffectInstance.IsLooped = true;
        soundEffectInstance.Play();

        var font = Content.Load<SpriteFont>("Score");

        playerBar = new Bar(barTexture, BarToEdgePadding, Height, ballCollideWithBarSound);
        cpuBar = new Bar(barTexture, Width - BarToEdgePadding, Height, ballCollideWithBarSound);

        var ball = new Ball(ballTexture, Width / 2, Width, Height, ballCollideWithEdgeSound);

        score = new Score(font);
        ballOutManager = new BallOutManager(ball, Width, ballOutSound, score);

        cpuController = new CpuController(cpuBar, ball, Height);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();


        if (score.IsGameFinished())
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                score.Reset();
                playerBar.ResetHorizontalMovementSpeed();
                cpuBar.ResetHorizontalMovementSpeed();
            }
            return;
        }

        var keyboardState = Keyboard.GetState();
        var gamePadState = GamePad.GetState(PlayerIndex.One);
        if (keyboardState.IsKeyDown(Keys.Up) || gamePadState.DPad.Up == ButtonState.Pressed)
        {
            playerBar.MoveUp();
        }

        if (keyboardState.IsKeyDown(Keys.Down) || gamePadState.DPad.Down == ButtonState.Pressed)
        {
            playerBar.MoveDown();
        }

        if (ballOutManager.IsBallOut())
        {
            ballOutManager.ResetBallAfterLatency();
            ballOutManager.GetBall().ResetHorizontalMovement();
            playerBar.ResetHorizontalMovementSpeed();
            cpuBar.ResetHorizontalMovementSpeed();
        }
        else
        {
            ballOutManager.Move();
            ballOutManager.GetBall().CheckHit(playerBar, cpuBar);
            cpuController.UpdatePosition();
        }
        base.Update(gameTime);
    }

    void GameOver()
    {
        DisplayEndMessage("Game Over");
    }

    void Win()
    {
        DisplayEndMessage("You  Win");
    }

    void DisplayEndMessage(string message)
    {
        spriteBatch.DrawString(score.GetFont(), message, new Vector2(40, Height / 2), Color.White, 0f, new Vector2(0, 0), new Vector2(3, 3), SpriteEffects.None, 0f);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin();

        // Draw player and cpu bars
        spriteBatch.Draw(playerBar.GetTexture(), new Rectangle(playerBar.GetPositionX(), playerBar.GetPositionY(), Bar.Width, Bar.Height), Color.White);
        spriteBatch.Draw(cpuBar.GetTexture(), new Rectangle(cpuBar.GetPositionX(), cpuBar.GetPositionY(), Bar.Width, Bar.Height), Color.White);

        // Draw the ball
        spriteBatch.Draw(ballOutManager.GetBall().GetTexture(), new Rectangle(ballOutManager.GetBall().GetPositionX(), ballOutManager.GetBall().GetPositionY(), 8, 8), Color.White);

        // Draw the Score values
        spriteBatch.DrawString(score.GetFont(), score.GetPlayerScore().ToString(CultureInfo.InvariantCulture), new Vector2(Width / 2 - 40 - 15, 20), Color.White);
        spriteBatch.DrawString(score.GetFont(), score.GetCpuScore().ToString(CultureInfo.InvariantCulture), new Vector2(Width / 2 + 40, 20), Color.White);

        DrawNet();

        if (score.PlayerWin())
        {
            Win();
        }

        if (score.CpuWin())
        {
            GameOver();
        }

        spriteBatch.End();


        base.Draw(gameTime);
    }

    void DrawNet()
    {
        for (var yPosition = 20; yPosition < Height - BarToEdgePadding; yPosition += 35)
        {
            spriteBatch.Draw(playerBar.GetTexture(), new Rectangle(Width / 2, yPosition, 5, 30), Color.White);
        }
    }
}