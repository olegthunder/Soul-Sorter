using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Soul_Sorter.Controller;

namespace Soul_Sorter.View;

public class MainMenu
{
    private enum MenuState
    {
        MainView,
        Settings
    }

    private MenuState _currentState = MenuState.MainView;

    private Texture2D _backgroundTexture;
    private Texture2D _gameTitleTexture;
    private Vector2 _gameTilePosition;
    private float _titleAnimationTime;
    private float _titleWaveAmplitude = 5.0f;

    private Texture2D _cloudsBackTexture;
    private Vector2 _cloudsBackPosition;
    private float _cloudsBackAnimationTime;
    private float _cloudsBackAmplitude = 15.0f;

    private Texture2D _cloud1Texture;
    private Vector2 _cloud1Position;
    private float _cloud1AnimationTime;
    private float _cloud1Amplitude = 10.0f;

    private Texture2D _cloud2Texture;
    private Vector2 _cloud2Position;
    private float _cloud2AnimationTime;
    private float _cloud2Amplitude = 12.0f;

    private Texture2D _cloud3Texture;
    private Vector2 _cloud3Position;
    private float _cloud3AnimationTime;
    private float _cloud3Amplitude = 15.0f;

    private Texture2D _cloudsFrontTexture;
    private Vector2 _cloudsFrontPosition;
    private float _cloudsFrontSpeed = 150f;
    private float _cloudVerticalAmplitude = 20.0f;
    private float _cloudVerticalFrequency = 5.0f;

    private Texture2D _buttonStartTexture;
    private Texture2D _buttonSettingsTexture;
    private Texture2D _buttonQuitTexture;

    private Rectangle _normalState;
    private Rectangle _hoverState;
    private Rectangle _pressedState;

    private Rectangle _currentStartButtonState;
    private Rectangle _currentSettingsButtonState;
    private Rectangle _currentQuitButtonState;

    private Vector2 _startButtonPosition;
    private Vector2 _settingsButtonPosition;
    private Vector2 _quitButtonPosition;

    private MouseState _previousMouseState;
    private Game1 _game;

    private Texture2D _settingsWindowTexture;
    private Vector2 _settingsWindowPosition;

    private Texture2D _backTexture;
    private Vector2 _backPosition;
    private Rectangle _currentBackState;
    private Rectangle _normalBackState;
    private Rectangle _hoverBackState;
    private Rectangle _pressedBackState;

    private Texture2D _soundTitleTexture;
    private Texture2D _soundDownTexture;
    private Texture2D _soundUpTexture;
    private Vector2 _soundTitlePosition;
    private Vector2 _soundDownPosition;
    private Vector2 _soundUpPosition;
    private Vector2 _soundVolumePosition;
    private Rectangle _currentSoundDownState;
    private Rectangle _currentSoundUpState;

    private Texture2D _musicTitleTexture;
    private Texture2D _musicDownTexture;
    private Texture2D _musicUpTexture;
    private Vector2 _musicTitlePosition;
    private Vector2 _musicDownPosition;
    private Vector2 _musicUpPosition;
    private Vector2 _musicVolumePosition;
    private Rectangle _currentMusicDownState;
    private Rectangle _currentMusicUpState;

    private Texture2D _fullScreenTitleTexture;
    private Texture2D _checkBoxTexture;
    private Vector2 _fullScreenTitlePosition;
    private Vector2 _checkBoxPosition;
    private Rectangle _currentCheckBoxState;
    private Rectangle _checkBoxUncheckedState;
    private Rectangle _checkBoxCheckedState;

    private SpriteFont _font;

    private Song _mainMenuMusic;
    public Song MainMenuMusic => _mainMenuMusic;

    public bool StartButtonClicked { get; set; }

    public MainMenu(Game1 game)
    {
        this._game = game;
        _previousMouseState = Mouse.GetState();
        StartButtonClicked = false;
    }

    public void LoadContent(ContentManager content)
    {
        _backgroundTexture = content.Load<Texture2D>("Menu/backgroundMenu");
        _cloudsBackTexture = content.Load<Texture2D>("Menu/cloudsBack");
        _gameTitleTexture = content.Load<Texture2D>("Menu/gameTitle");
        _cloudsFrontTexture = content.Load<Texture2D>("Menu/cloudsFront");
        _cloud1Texture = content.Load<Texture2D>("Menu/cloud1");
        _cloud2Texture = content.Load<Texture2D>("Menu/cloud2");
        _cloud3Texture = content.Load<Texture2D>("Menu/cloud3");

        _settingsWindowTexture = content.Load<Texture2D>("Menu/Settings/settingsWindow");
        _backTexture = content.Load<Texture2D>("Menu/Settings/back");

        _soundTitleTexture = content.Load<Texture2D>("Menu/Settings/SoundTitle");
        _soundDownTexture = content.Load<Texture2D>("Menu/Settings/SoundDown");
        _soundUpTexture = content.Load<Texture2D>("Menu/Settings/SoundUp");

        _musicTitleTexture = content.Load<Texture2D>("Menu/Settings/MusicTitle");
        _musicDownTexture = content.Load<Texture2D>("Menu/Settings/MusicDown");
        _musicUpTexture = content.Load<Texture2D>("Menu/Settings/MusicUp");

        _fullScreenTitleTexture = content.Load<Texture2D>("Menu/Settings/FullScreenTitle");
        _checkBoxTexture = content.Load<Texture2D>("Menu/Settings/CheckBoxScreen");

        _buttonStartTexture = content.Load<Texture2D>("Menu/startGameButtons");
        _buttonSettingsTexture = content.Load<Texture2D>("Menu/settingsGameButtons");
        _buttonQuitTexture = content.Load<Texture2D>("Menu/quitGameButtons");

        _font = content.Load<SpriteFont>("Pixelony");

        _mainMenuMusic = content.Load<Song>("Menu/mainMenuMusic");

        MediaPlayer.Play(_mainMenuMusic);
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = _game.MusicVolume / 100f;

        var buttonWidth = _buttonStartTexture.Width / 3;
        var buttonHeight = _buttonStartTexture.Height;

        _normalState = new Rectangle(0, 0, buttonWidth, buttonHeight);
        _hoverState = new Rectangle(buttonWidth, 0, buttonWidth, buttonHeight);
        _pressedState = new Rectangle(2 * buttonWidth, 0, buttonWidth, buttonHeight);

        var buttonBackWidth = _backTexture.Width / 3;
        var buttonBackHeight = _backTexture.Height;

        _normalBackState = new Rectangle(0, 0, buttonBackWidth, buttonBackHeight);
        _hoverBackState = new Rectangle(buttonBackWidth, 0, buttonBackWidth, buttonBackHeight);
        _pressedBackState = new Rectangle(2 * buttonBackWidth, 0, buttonBackWidth, buttonBackHeight);

        _currentStartButtonState = _normalState;
        _currentSettingsButtonState = _normalState;
        _currentQuitButtonState = _normalState;
        _currentBackState = _normalBackState;

        _startButtonPosition = new Vector2(725, 316);
        _settingsButtonPosition = new Vector2(725, 486);
        _quitButtonPosition = new Vector2(725, 656);

        _gameTilePosition = new Vector2(560, 107);
        _cloudsBackPosition = new Vector2(0, 636);
        _cloudsFrontPosition = new Vector2(0, 950);
        _cloud1Position = new Vector2(1385, 298);
        _cloud2Position = new Vector2(42, 62);
        _cloud3Position = new Vector2(358, 316);

        _settingsWindowPosition = new Vector2(560, 279);
        _backPosition = new Vector2(835, 921);

        _soundTitlePosition = new Vector2(909, 309);
        _soundDownPosition = new Vector2(869, 390);
        _soundUpPosition = new Vector2(1001, 390);
        _soundVolumePosition = new Vector2(929, 395);

        _musicTitlePosition = new Vector2(893, 490);
        _musicDownPosition = new Vector2(869, 571);
        _musicUpPosition = new Vector2(1001, 571);
        _musicVolumePosition = new Vector2(929, 576);

        _fullScreenTitlePosition = new Vector2(753, 671);
        _checkBoxPosition = new Vector2(935, 752);

        _checkBoxUncheckedState = new Rectangle(0, 0, 50, 50);
        _checkBoxCheckedState = new Rectangle(50, 0, 50, 50);
        _currentCheckBoxState = _game.IsFullScreen ? _checkBoxCheckedState : _checkBoxUncheckedState;
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        spriteBatch.Draw(_backgroundTexture, graphicsDevice.Viewport.Bounds, Color.White);
        spriteBatch.Draw(_cloudsBackTexture, _cloudsBackPosition, Color.White);
        spriteBatch.Draw(_cloud1Texture, _cloud1Position, Color.White);
        spriteBatch.Draw(_cloud2Texture, _cloud2Position, Color.White);
        spriteBatch.Draw(_cloud3Texture, _cloud3Position, Color.White);
        spriteBatch.Draw(_gameTitleTexture, _gameTilePosition, Color.White);
        switch (_currentState)
        {
            case MenuState.MainView:
                spriteBatch.Draw(_buttonStartTexture, _startButtonPosition, _currentStartButtonState, Color.White);
                spriteBatch.Draw(_buttonSettingsTexture, _settingsButtonPosition, _currentSettingsButtonState,
                    Color.White);
                spriteBatch.Draw(_buttonQuitTexture, _quitButtonPosition, _currentQuitButtonState, Color.White);
                break;
            case MenuState.Settings:
                spriteBatch.Draw(_settingsWindowTexture, _settingsWindowPosition, Color.White);
                spriteBatch.Draw(_backTexture, _backPosition, _currentBackState, Color.White);
                DrawSettings(spriteBatch);
                break;
        }

        spriteBatch.Draw(_cloudsFrontTexture, _cloudsFrontPosition, Color.White);
        spriteBatch.Draw(_cloudsFrontTexture, _cloudsFrontPosition + new Vector2(_cloudsFrontTexture.Width, 0),
            Color.White);
    }

    private void DrawSettings(SpriteBatch spriteBatch)
    {
        var scaleFont = 0.6f;
        spriteBatch.Draw(_soundTitleTexture, _soundTitlePosition, Color.White);
        spriteBatch.Draw(_soundDownTexture, _soundDownPosition, _currentSoundDownState, Color.White);
        spriteBatch.Draw(_soundUpTexture, _soundUpPosition, _currentSoundUpState, Color.White);
        spriteBatch.DrawString(_font, $"{_game.SoundVolume}", _soundVolumePosition, Color.Black, 0f, Vector2.Zero,
            scaleFont, SpriteEffects.None, 0f);

        spriteBatch.Draw(_musicTitleTexture, _musicTitlePosition, Color.White);
        spriteBatch.Draw(_musicDownTexture, _musicDownPosition, _currentMusicDownState, Color.White);
        spriteBatch.Draw(_musicUpTexture, _musicUpPosition, _currentMusicUpState, Color.White);
        spriteBatch.DrawString(_font, $"{_game.MusicVolume}", _musicVolumePosition, Color.Black, 0f, Vector2.Zero,
            scaleFont, SpriteEffects.None, 0f);

        spriteBatch.Draw(_fullScreenTitleTexture, _fullScreenTitlePosition, Color.White);
        spriteBatch.Draw(_checkBoxTexture, _checkBoxPosition, _currentCheckBoxState, Color.White);
    }

    public void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        switch (_currentState)
        {
            case MenuState.MainView:
                UpdateButtonState(mouseState, _startButtonPosition, _buttonStartTexture, ref _currentStartButtonState,
                    _buttonStartTexture.Width / 3, _buttonStartTexture.Height);
                UpdateButtonState(mouseState, _settingsButtonPosition, _buttonSettingsTexture,
                    ref _currentSettingsButtonState, _buttonSettingsTexture.Width / 3, _buttonSettingsTexture.Height);
                UpdateButtonState(mouseState, _quitButtonPosition, _buttonQuitTexture, ref _currentQuitButtonState,
                    _buttonQuitTexture.Width / 3, _buttonQuitTexture.Height);

                if (IsButtonClicked(_startButtonPosition, _buttonStartTexture, mouseState))
                {
                    StartButtonClicked = true;
                    MediaPlayer.Stop(); // Остановка музыки главного меню
                }

                if (IsButtonClicked(_settingsButtonPosition, _buttonSettingsTexture, mouseState))
                {
                    _currentState = MenuState.Settings;
                }

                if (IsButtonClicked(_quitButtonPosition, _buttonQuitTexture, mouseState))
                {
                    _game.Exit();
                }

                break;
            case MenuState.Settings:
                UpdateButtonBackState(mouseState, _backPosition, _backTexture, ref _currentBackState,
                    _backTexture.Width / 3, _backTexture.Height);
                UpdateSoundButtons(mouseState);
                UpdateMusicButtons(mouseState);
                UpdateFullScreenCheckBox(mouseState);

                if (IsButtonClicked(_backPosition, _backTexture, mouseState))
                {
                    _currentState = MenuState.MainView;
                    _game.SaveSettings();
                }

                break;
        }

        _titleAnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        _gameTilePosition.Y = 107 + _titleWaveAmplitude * (float)Math.Sin(_titleAnimationTime * 2.0);

        _cloudsBackAnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        _cloudsBackPosition.Y = 636 + _cloudsBackAmplitude * (float)Math.Sin(_cloudsBackAnimationTime * 1.0);

        _cloud1AnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        _cloud1Position.Y = 298 + _cloud1Amplitude * (float)Math.Sin(_cloud1AnimationTime * 1.3);

        _cloud2AnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        _cloud2Position.Y = 62 + _cloud2Amplitude * (float)Math.Sin(_cloud2AnimationTime * 1.3);

        _cloud3AnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        _cloud3Position.Y = 316 + _cloud3Amplitude * (float)Math.Sin(_cloud3AnimationTime * 2.0);

        var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _cloudsFrontPosition.X -= _cloudsFrontSpeed * elapsed;

        if (_cloudsFrontPosition.X <= -_cloudsFrontTexture.Width)
        {
            _cloudsFrontPosition.X = 0;
        }

        _cloudsFrontPosition.Y = 850 + _cloudVerticalAmplitude *
            (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * _cloudVerticalFrequency);

        _previousMouseState = mouseState;
    }

    private void UpdateButtonState(MouseState mouseState, Vector2 buttonPosition, Texture2D buttonTexture,
        ref Rectangle currentButtonState, int buttonWidth, int buttonHeight)
    {
        var buttonRect = new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, buttonWidth, buttonHeight);
        if (buttonRect.Contains(mouseState.X, mouseState.Y))
        {
            currentButtonState = mouseState.LeftButton == ButtonState.Pressed ? _pressedState : _hoverState;
        }
        else
        {
            currentButtonState = _normalState;
        }
    }

    private void UpdateButtonBackState(MouseState mouseState, Vector2 buttonPosition, Texture2D buttonTexture,
        ref Rectangle currentButtonState, int buttonWidth, int buttonHeight)
    {
        var buttonRect = new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, buttonWidth, buttonHeight);
        if (buttonRect.Contains(mouseState.X, mouseState.Y))
        {
            currentButtonState = mouseState.LeftButton == ButtonState.Pressed ? _pressedBackState : _hoverBackState;
        }
        else
        {
            currentButtonState = _normalBackState;
        }
    }

    private bool IsButtonClicked(Vector2 buttonPosition, Texture2D buttonTexture, MouseState mouseState)
    {
        var buttonWidth = buttonTexture.Width / 3;
        var buttonHeight = buttonTexture.Height;
        var buttonRect = new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, buttonWidth, buttonHeight);

        return buttonRect.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed &&
               _previousMouseState.LeftButton == ButtonState.Released;
    }

    private void UpdateSoundButtons(MouseState mouseState)
    {
        var soundDownRect = new Rectangle((int)_soundDownPosition.X, (int)_soundDownPosition.Y, 50, 50);
        var soundUpRect = new Rectangle((int)_soundUpPosition.X, (int)_soundUpPosition.Y, 50, 50);

        if (soundDownRect.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed &&
            _previousMouseState.LeftButton == ButtonState.Released)
        {
            _game.SoundVolume = Math.Max(0, _game.SoundVolume - 10);
            SoundEffect.MasterVolume = _game.SoundVolume / 100f;
        }

        if (soundUpRect.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed &&
            _previousMouseState.LeftButton == ButtonState.Released)
        {
            _game.SoundVolume = Math.Min(100, _game.SoundVolume + 10);
            SoundEffect.MasterVolume = _game.SoundVolume / 100f;
        }

        _currentSoundDownState = soundDownRect.Contains(mouseState.Position)
            ? new Rectangle(50, 0, 50, 50)
            : new Rectangle(0, 0, 50, 50);
        _currentSoundUpState = soundUpRect.Contains(mouseState.Position)
            ? new Rectangle(50, 0, 50, 50)
            : new Rectangle(0, 0, 50, 50);
    }

    private void UpdateMusicButtons(MouseState mouseState)
    {
        var musicDownRect = new Rectangle((int)_musicDownPosition.X, (int)_musicDownPosition.Y, 50, 50);
        var musicUpRect = new Rectangle((int)_musicUpPosition.X, (int)_musicUpPosition.Y, 50, 50);

        if (musicDownRect.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed &&
            _previousMouseState.LeftButton == ButtonState.Released)
        {
            _game.MusicVolume = Math.Max(0, _game.MusicVolume - 10);
            MediaPlayer.Volume = _game.MusicVolume / 100f;
        }

        if (musicUpRect.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed &&
            _previousMouseState.LeftButton == ButtonState.Released)
        {
            _game.MusicVolume = Math.Min(100, _game.MusicVolume + 10);
            MediaPlayer.Volume = _game.MusicVolume / 100f;
        }

        _currentMusicDownState = musicDownRect.Contains(mouseState.Position)
            ? new Rectangle(50, 0, 50, 50)
            : new Rectangle(0, 0, 50, 50);
        _currentMusicUpState = musicUpRect.Contains(mouseState.Position)
            ? new Rectangle(50, 0, 50, 50)
            : new Rectangle(0, 0, 50, 50);
    }

    private void UpdateFullScreenCheckBox(MouseState mouseState)
    {
        var checkBoxRect = new Rectangle((int)_checkBoxPosition.X, (int)_checkBoxPosition.Y, 50, 50);

        if (checkBoxRect.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed &&
            _previousMouseState.LeftButton == ButtonState.Released)
        {
            _game.IsFullScreen = !_game.IsFullScreen;
            _game.SetFullScreen(_game.IsFullScreen);
            _currentCheckBoxState = _game.IsFullScreen ? _checkBoxCheckedState : _checkBoxUncheckedState;
        }
    }
}