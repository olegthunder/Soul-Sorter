using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Soul_Sorter.Model;
using Soul_Sorter.View;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;

namespace Soul_Sorter.Controller;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Soul _currentSoul;
    private List<Texture2D> _demonTextures;
    private List<Texture2D> _humanTextures;

    private MainMenu _mainMenu;
    private BackgroundGameplay _backgroundGameplay;
    private Player _player;
    private HUD _hud;
    private Intro _intro;
    private Outro _outro;
    private Random _random;

    private Song _backgroundMusic;

    private const int InitialLives = 5;
    private const int InitialSoulsToSend = 100;

    private enum GameState
    {
        MainMenu,
        Intro,
        Gameplay,
        Outro
    }

    private GameState _currentState;

    public static Player PlayerInstance { get; private set; }

    public float SoundVolume { get; set; }
    public float MusicVolume { get; set; }
    public bool IsFullScreen { get; set; }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.HardwareModeSwitch = false;
        Window.AllowUserResizing = true;

        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.ApplyChanges();

        _random = new Random();
    }

    protected override void Initialize()
    {
        LoadSettings();

        _mainMenu = new MainMenu(this);
        _backgroundGameplay = new BackgroundGameplay(this);
        _player = new Player();
        PlayerInstance = _player;
        _hud = new HUD();
        _intro = new Intro(this);
        _outro = new Outro(this);
        _demonTextures = new List<Texture2D>();
        _humanTextures = new List<Texture2D>();
        _currentState = GameState.MainMenu;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _mainMenu.LoadContent(Content);
        _backgroundGameplay.LoadContent(Content);
        _player.LoadContent(Content);
        _hud.LoadContent(Content);
        _intro.LoadContent(Content);
        _outro.LoadContent(Content);

        _backgroundMusic = Content.Load<Song>("Gameplay/Background/backgroundGameplay");

        for (var i = 1; i <= 6; i++)
        {
            _demonTextures.Add(Content.Load<Texture2D>($"Gameplay/Demons/DemonSoul{i}"));
            _humanTextures.Add(Content.Load<Texture2D>($"Gameplay/Humans/HumanSoul{i}"));
        }

        CreateNewSoul();

        ApplyVolumeSettings();
    }

    private void CreateNewSoul()
    {
        Soul.SoulType soulType = (Soul.SoulType)_random.Next(2);
        Texture2D texture = soulType == Soul.SoulType.Demon
            ? GetRandomTexture(_demonTextures)
            : GetRandomTexture(_humanTextures);
        _currentSoul = new Soul(texture, soulType, new Vector2(1000, 378));
        _currentSoul.LoadContent(Content);
        _currentSoul.OnSoulSent += OnSoulSent;

        _hud.ResetTimer();
    }

    private void OnSoulSent(Soul soul)
    {
        if (_player.Lives <= 0 || _player.SoulsToSend <= 0)
        {
            _currentState = GameState.Outro;
            _outro.StartOutro();
        }
        else
        {
            CreateNewSoul();
        }
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        var mouseState = Mouse.GetState();

        switch (_currentState)
        {
            case GameState.MainMenu:
                _mainMenu.Update(gameTime);
                if (_mainMenu.StartButtonClicked)
                {
                    _currentState = GameState.Intro;
                    _intro.StartIntro();
                }

                break;
            case GameState.Intro:
                _intro.Update(gameTime, mouseState);
                if (_intro.IsIntroFinished)
                {
                    StartGameplay();
                }

                break;
            case GameState.Gameplay:
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);

                _backgroundGameplay.Update(gameTime);
                _player.Update(gameTime, keyboardState);

                _hud.Update(_player.Lives, _player.SoulsToSend, _currentSoul.DecisionTimer);

                _currentSoul.Update(gameTime, keyboardState, mousePosition);
                break;
            case GameState.Outro:
                _outro.Update(gameTime, mouseState);
                break;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        switch (_currentState)
        {
            case GameState.MainMenu:
                _mainMenu.Draw(_spriteBatch, GraphicsDevice);
                break;
            case GameState.Intro:
                _intro.Draw(_spriteBatch);
                break;
            case GameState.Gameplay:
                _backgroundGameplay.Draw(_spriteBatch, GraphicsDevice);
                _player.Draw(_spriteBatch);
                _hud.Draw(_spriteBatch);
                _currentSoul.Draw(_spriteBatch);
                break;
            case GameState.Outro:
                _outro.Draw(_spriteBatch);
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void ReturnToMainMenu()
    {
        _currentState = GameState.MainMenu;
        _mainMenu.StartButtonClicked = false;
        _player.Reset(InitialLives, InitialSoulsToSend);
        CreateNewSoul();
        MediaPlayer.Play(_mainMenu.MainMenuMusic);
    }

    public void StartGameplay()
    {
        _currentState = GameState.Gameplay;
        MediaPlayer.Play(_player.BackgroundMusic);
    }

    private Texture2D GetRandomTexture(List<Texture2D> textures)
    {
        int index = _random.Next(textures.Count);
        return textures[index];
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _graphics.IsFullScreen = isFullScreen;
        _graphics.ApplyChanges();
    }

    public void SaveSettings()
    {
        var settings = new { SoundVolume, MusicVolume, IsFullScreen };

        var json = JsonSerializer.Serialize(settings);
        File.WriteAllText("settings.json", json);
    }

    public void LoadSettings()
    {
        if (File.Exists("settings.json"))
        {
            var json = File.ReadAllText("settings.json");
            var settings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            SoundVolume = settings["SoundVolume"].GetSingle();
            MusicVolume = settings["MusicVolume"].GetSingle();
            IsFullScreen = settings["IsFullScreen"].GetBoolean();
            SetFullScreen(IsFullScreen);
            MediaPlayer.Volume = MusicVolume / 100f;
        }
        else
        {
            SoundVolume = 100;
            MusicVolume = 100;
            IsFullScreen = false;
        }
    }

    private void ApplyVolumeSettings()
    {
        MediaPlayer.Volume = MusicVolume / 100f;
        SoundEffect.MasterVolume = SoundVolume / 100f;
    }
}