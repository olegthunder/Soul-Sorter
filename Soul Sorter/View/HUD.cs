using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Soul_Sorter.View;

public class HUD
{
    private Texture2D _backgroundTexture;
    private Texture2D _timerTexture;
    private SpriteFont _font;
    private Vector2 _backgroundPosition;
    private Vector2 _livesPosition;
    private Vector2 _soulsPosition;
    private Vector2 _timerPosition;

    private int _lives;
    private int _soulsToSend;
    private float _decisionTime;
    private int _currentFrame;
    private const int FrameCount = 11;
    private const float DecisionTimeLimit = 10f;

    public HUD()
    {
        _backgroundPosition = new Vector2(26, 30);
        _livesPosition = new Vector2(144, 68);
        _soulsPosition = new Vector2(414, 68);
        _timerPosition = new Vector2(1740, 926);
    }

    public void LoadContent(ContentManager content)
    {
        _backgroundTexture = content.Load<Texture2D>("Gameplay/HUD/hud");
        _timerTexture = content.Load<Texture2D>("Gameplay/HUD/timer");
        _font = content.Load<SpriteFont>("Pixelony");
    }

    public void Update(int lives, int soulsToSend, float decisionTime)
    {
        _lives = lives;
        _soulsToSend = soulsToSend;
        _decisionTime = decisionTime;

        var elapsedTime = DecisionTimeLimit - _decisionTime;
        _currentFrame = (int)((elapsedTime / DecisionTimeLimit) * FrameCount);
        if (_currentFrame >= FrameCount)
        {
            _currentFrame = FrameCount - 1;
        }
    }

    public void ResetTimer()
    {
        _currentFrame = 0;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_backgroundTexture, _backgroundPosition, Color.White);
        spriteBatch.DrawString(_font, $" {_lives}", _livesPosition, Color.Black);
        spriteBatch.DrawString(_font, $" {_soulsToSend}", _soulsPosition, Color.Black);

        Rectangle sourceRectangle = new Rectangle(_currentFrame * (_timerTexture.Width / FrameCount), 0,
            _timerTexture.Width / FrameCount, _timerTexture.Height);
        spriteBatch.Draw(_timerTexture, _timerPosition, sourceRectangle, Color.White);
    }
}