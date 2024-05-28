using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Soul_Sorter.Controller;

namespace Soul_Sorter.View;

public class Outro
{
    private Game1 _game;
    private Texture2D _buttonTexture;
    private Texture2D _badOutroTexture;
    private Texture2D _goodOutroTexture;
    private Texture2D _endTexture;
    private Rectangle _buttonRectangle;

    private int _currentStep;
    private bool _showButton;

    private Rectangle _buttonNormal;
    private Rectangle _buttonHover;
    private Rectangle _buttonPressed;
    private Rectangle _currentButtonState;

    private bool IsOutroFinished => _currentStep >= 2;
    private Song _outroSound;

    public Outro(Game1 game)
    {
        _game = game;
        _currentStep = 0;
        _showButton = true;

        _buttonRectangle = new Rectangle(1608, 56, 250, 90);
    }

    public void LoadContent(ContentManager content)
    {
        _buttonTexture = content.Load<Texture2D>("Intro/NextButton");

        int buttonStateWidth = _buttonTexture.Width / 3;
        int buttonStateHeight = _buttonTexture.Height;

        _buttonNormal = new Rectangle(0, 0, buttonStateWidth, buttonStateHeight);
        _buttonHover = new Rectangle(buttonStateWidth, 0, buttonStateWidth, buttonStateHeight);
        _buttonPressed = new Rectangle(buttonStateWidth * 2, 0, buttonStateWidth, buttonStateHeight);

        _currentButtonState = _buttonNormal;

        _badOutroTexture = content.Load<Texture2D>("Outro/badOutro");
        _goodOutroTexture = content.Load<Texture2D>("Outro/goodOutro");
        _endTexture = content.Load<Texture2D>("Outro/end");

        _outroSound = content.Load<Song>("Outro/outroSound");
    }

    public void Update(GameTime gameTime, MouseState mouseState)
    {
        if (_showButton)
        {
            if (_buttonRectangle.Contains(mouseState.Position))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    _currentButtonState = _buttonPressed;
                }
                else
                {
                    if (_currentButtonState == _buttonPressed)
                    {
                        _currentStep++;
                        _currentButtonState = _buttonNormal;

                        if (IsOutroFinished)
                        {
                            _game.ReturnToMainMenu();
                        }
                    }
                    else
                    {
                        _currentButtonState = _buttonHover;
                    }
                }
            }
            else
            {
                _currentButtonState = _buttonNormal;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.GraphicsDevice.Clear(Color.Black);

        switch (_currentStep)
        {
            case 0:
                spriteBatch.Draw(Game1.PlayerInstance.Lives <= 0 ? _badOutroTexture : _goodOutroTexture,
                    new Vector2(110, 28), Color.White);
                break;
            case 1:
                spriteBatch.Draw(_endTexture, new Vector2(755, 456), Color.White);
                break;
        }

        if (_showButton)
        {
            spriteBatch.Draw(_buttonTexture, _buttonRectangle, _currentButtonState, Color.White);
        }
    }

    public void StartOutro()
    {
        _currentStep = 0;
        _showButton = true;
        MediaPlayer.Play(_outroSound);
    }
}