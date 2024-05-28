using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Soul_Sorter.Controller;

namespace Soul_Sorter.View;

public class Intro
{
    private Game1 _game;
    private Texture2D _buttonTexture;
    private Texture2D[] _introTextures;
    private Rectangle _buttonRectangle;
    private Vector2[] _introPositions;

    private int _currentStep;
    private bool _showButton;

    private Rectangle _buttonNormal;
    private Rectangle _buttonHover;
    private Rectangle _buttonPressed;
    private Rectangle _currentButtonState;

    public bool IsIntroFinished => _currentStep >= 6;
    private Song _introSound;

    public Intro(Game1 game)
    {
        _game = game;
        _currentStep = 0;
        _showButton = true;

        _introTextures = new Texture2D[6];

        _introPositions = new Vector2[]
        {
            new Vector2(8, 140), new Vector2(880, 40), new Vector2(40, 40), new Vector2(86, 140),
            new Vector2(110, 42), new Vector2(326, 56)
        };

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

        for (int i = 0; i < 6; i++)
        {
            _introTextures[i] = content.Load<Texture2D>($"Intro/intro{i + 1}");
        }

        _introSound = content.Load<Song>("Intro/introSound");
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

                        if (IsIntroFinished)
                        {
                            _game.StartGameplay();
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
                spriteBatch.Draw(_introTextures[0], _introPositions[0], Color.White);
                break;
            case 1:
                spriteBatch.Draw(_introTextures[0], _introPositions[0], Color.White);
                spriteBatch.Draw(_introTextures[1], _introPositions[1], Color.White);
                break;
            case 2:
                spriteBatch.Draw(_introTextures[2], _introPositions[2], Color.White);
                break;
            case 3:
                spriteBatch.Draw(_introTextures[2], _introPositions[2], Color.White);
                spriteBatch.Draw(_introTextures[3], _introPositions[3], Color.White);
                break;
            case 4:
                spriteBatch.Draw(_introTextures[4], _introPositions[4], Color.White);
                break;
            case 5:
                spriteBatch.Draw(_introTextures[5], _introPositions[5], Color.White);
                break;
        }

        if (_showButton)
        {
            spriteBatch.Draw(_buttonTexture, _buttonRectangle, _currentButtonState, Color.White);
        }
    }

    public void StartIntro()
    {
        _currentStep = 0;
        _showButton = true;
        MediaPlayer.Play(_introSound);
    }
}