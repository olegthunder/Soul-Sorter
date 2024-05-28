using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Soul_Sorter.Controller;

namespace Soul_Sorter.Model;

public class Soul
{
    public enum SoulType
    {
        Demon,
        Human
    }

    private Texture2D _texture;
    private Vector2 _position;
    private SoulType _type;
    private List<Attributes> _attributes;
    private bool _isSentToHeaven;
    private bool _isSentToHell;
    private int _currentFrame;
    private float _frameTime;
    private float _frameDuration;
    private Rectangle _sourceRectangle;
    private const int FrameWidth = 250;
    private const int FrameHeight = 250;
    private const float Scale = 2.4f;
    private bool _isFinished;
    private float _heavenSpeed = 1000f;
    private Texture2D _attributeTexture;
    private const float DecisionTimeLimit = 10f;
    private float _decisionTimer;
    private Action<Soul> _onSoulSent;
    private int _karma;

    public bool IsFinished => _isFinished;
    public float DecisionTimer => _decisionTimer;

    public event Action<Soul> OnSoulSent
    {
        add => _onSoulSent += value;
        remove => _onSoulSent -= value;
    }

    public Soul(Texture2D texture, SoulType type, Vector2 position)
    {
        _texture = texture;
        _type = type;
        _position = position;
        _attributes = new List<Attributes>();
        _isSentToHeaven = false;
        _isSentToHell = false;
        _currentFrame = 0;
        _frameTime = 0f;
        _frameDuration = 0.1f;
        _decisionTimer = DecisionTimeLimit;
        _isFinished = false;
        _karma = 0;
    }

    public void LoadContent(ContentManager content)
    {
        _attributeTexture = content.Load<Texture2D>("Attributes");

        Random random = new Random();
        _attributes.Clear();
        _karma = 0;
        for (int i = 0; i < 5; i++)
        {
            var typeIndex = random.Next(2);
            Attributes.AttributeType type = (Attributes.AttributeType)typeIndex;
            var attributeIndex = random.Next(5);

            var y = typeIndex * 60;
            var x = attributeIndex * 60;

            Rectangle sourceRectangle = new Rectangle(x, y, 60, 60);
            Vector2 relativePosition = new Vector2(1143 - _position.X + i * 65, 318 - _position.Y);
            Attributes attribute =
                new Attributes(_attributeTexture, sourceRectangle, type, relativePosition, _position);

            _attributes.Add(attribute);

            _karma += type == Attributes.AttributeType.Positive ? 1 : -1;
        }
    }

    public void Update(GameTime gameTime, KeyboardState keyboardState, Vector2 mousePosition)
    {
        if (_isFinished) return;

        _decisionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        _frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var attribute in _attributes)
        {
            attribute.Update(_position, mousePosition);
        }

        if (_frameTime >= _frameDuration)
        {
            _currentFrame++;
            _frameTime = 0f;

            if (_isSentToHeaven && _currentFrame >= 3)
            {
                _currentFrame = 0;
            }
            else if (_isSentToHell && _currentFrame >= 9)
            {
                _currentFrame = 8;
                _isFinished = true;
                _onSoulSent?.Invoke(this);
            }
            else if (!_isSentToHeaven && !_isSentToHell && _currentFrame >= 2)
            {
                _currentFrame = 0;
            }
        }

        if (_isSentToHeaven)
        {
            _position.Y -= _heavenSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_position.Y < -FrameHeight * Scale)
            {
                _isFinished = true;
                _onSoulSent?.Invoke(this);
            }
        }

        if (keyboardState.IsKeyDown(Keys.W) && !_isSentToHeaven && !_isSentToHell)
        {
            EvaluateSoul(true);
        }
        else if (keyboardState.IsKeyDown(Keys.S) && !_isSentToHeaven && !_isSentToHell)
        {
            EvaluateSoul(false);
        }

        if (_decisionTimer <= 0 && !_isSentToHeaven && !_isSentToHell)
        {
            EvaluateSoul(true);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var row = 0;
        if (_isSentToHeaven)
        {
            row = 1;
        }
        else if (_isSentToHell)
        {
            row = 2;
        }

        _sourceRectangle = new Rectangle(_currentFrame * FrameWidth, row * FrameHeight, FrameWidth, FrameHeight);
        spriteBatch.Draw(_texture, _position, _sourceRectangle, Color.White, 0f, Vector2.Zero, Scale,
            SpriteEffects.None, 0f);

        foreach (var attribute in _attributes)
        {
            attribute.Draw(spriteBatch);
        }
    }

    private void EvaluateSoul(bool sendToHeaven)
    {
        if (_type == SoulType.Demon)
        {
            if (sendToHeaven)
            {
                Game1.PlayerInstance.LoseLife();
                SendToHeaven();
            }
            else
            {
                Game1.PlayerInstance.DecrementSoulsToSend();
                SendToHell();
            }
        }
        else
        {
            if (_karma > 0 && sendToHeaven)
            {
                SendToHeaven();
            }
            else if (_karma <= 0 && !sendToHeaven)
            {
                Game1.PlayerInstance.DecrementSoulsToSend();
                SendToHell();
            }
            else
            {
                Game1.PlayerInstance.LoseLife();
                if (sendToHeaven)
                {
                    SendToHeaven();
                }
                else
                {
                    SendToHell();
                }
            }
        }
    }

    private void SendToHeaven()
    {
        _isSentToHeaven = true;
        _currentFrame = 0;
        Game1.PlayerInstance.PlaySendToHeavenSound();
    }

    private void SendToHell()
    {
        _isSentToHell = true;
        _currentFrame = 0;
        Game1.PlayerInstance.PlaySendToHellSound();
    }
}