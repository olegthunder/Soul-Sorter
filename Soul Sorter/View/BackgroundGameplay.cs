using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Soul_Sorter.Controller;

namespace Soul_Sorter.View;

public class BackgroundGameplay
{
    private Texture2D _backgroundTexture;
    private Texture2D _backgroundClouds;
    private Texture2D _backgroundCloud1;
    private Texture2D _backgroundCloud2;
    private Texture2D _backgroundCloud3;
    private Vector2 _backgroundCloudsPosition;
    private Vector2 _backgroundCloud1Position;
    private Vector2 _backgroundCloud2Position;
    private Vector2 _backgroundCloud3Position;
    private float _backgroundCloudsAnimationTime;
    private float _cloudsBackAmplitude = 10.0f;

    private Game1 _game;

    public BackgroundGameplay(Game1 game)
    {
        this._game = game;
    }

    public void LoadContent(ContentManager content)
    {
        _backgroundTexture = content.Load<Texture2D>("Menu/backgroundMenu");
        _backgroundClouds = content.Load<Texture2D>("Gameplay/Background/backgroundClouds");
        _backgroundCloud1 = content.Load<Texture2D>("Gameplay/Background/backgroundCloud1");
        _backgroundCloud2 = content.Load<Texture2D>("Gameplay/Background/backgroundCloud2");
        _backgroundCloud3 = content.Load<Texture2D>("Gameplay/Background/backgroundCloud3");
        _backgroundCloudsPosition = new Vector2(0, 590);
        _backgroundCloud1Position = new Vector2(34, 391);
        ;
        _backgroundCloud2Position = new Vector2(711, 80);
        ;
        _backgroundCloud3Position = new Vector2(1544, 309);
        ;
    }

    public void Update(GameTime gameTime)
    {
        _backgroundCloudsAnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        _backgroundCloudsPosition.Y =
            590 + _cloudsBackAmplitude * (float)Math.Sin(_backgroundCloudsAnimationTime * 0.5);
        _backgroundCloud1Position.Y = 391 + _cloudsBackAmplitude * (float)Math.Sin(_backgroundCloudsAnimationTime * 1);
        _backgroundCloud2Position.Y = 80 + _cloudsBackAmplitude * (float)Math.Sin(_backgroundCloudsAnimationTime * 1);
        _backgroundCloud3Position.Y = 309 + _cloudsBackAmplitude * (float)Math.Sin(_backgroundCloudsAnimationTime * 1);
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        spriteBatch.Draw(_backgroundTexture, graphicsDevice.Viewport.Bounds, Color.White);
        spriteBatch.Draw(_backgroundClouds, _backgroundCloudsPosition, Color.White);
        spriteBatch.Draw(_backgroundCloud1, _backgroundCloud1Position, Color.White);
        spriteBatch.Draw(_backgroundCloud2, _backgroundCloud2Position, Color.White);
        spriteBatch.Draw(_backgroundCloud3, _backgroundCloud3Position, Color.White);
    }
}