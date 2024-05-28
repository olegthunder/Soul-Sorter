using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Soul_Sorter.Model;

public class Player
{
    private Texture2D _idleTexture;
        private Texture2D _sendToHellTexture;
        private Texture2D _sendToHeavenTexture;
        private Texture2D _hurtTexture;
        private Texture2D _godsFistTexture;
        private Vector2 _position;
        private Vector2 _godsFistPosition;
        private Vector2 _godsFistTargetPosition;
        private int _lives;
        private int _soulsToSend;

        private int _idleFrameCount = 13;
        private int _sendFrameCount = 7;
        private int _hurtFrameCount = 6;
        private int _currentFrame;
        private float _frameTime;
        private float _frameDuration = 0.05f; 
        private bool _isSendingToHell;
        private bool _isSendingToHeaven;
        private bool _isHurt;

        private const int FrameHeight = 220;
        private const int FrameWidthIdle = 120;
        private const int FrameWidthSend = 120;
        private const int FrameWidthHurt = 120;
        private const int TargetHeight = 700;
        private const float Scale = (float)TargetHeight / FrameHeight;
        private const float FistSpeed = 2000f;

        private SoundEffect _sendToHeavenSound;
        private SoundEffect _sendToHellSound;
        private SoundEffect _hurtSound;

        public Song BackgroundMusic { get; private set; }

        public Player()
        {
            _position = new Vector2(361, 274);
            _godsFistPosition = new Vector2(351, -420);
            _godsFistTargetPosition = new Vector2(351, -40);
            _lives = 5;
            _soulsToSend = 100;
        }

        public void LoadContent(ContentManager content)
        {
            _idleTexture = content.Load<Texture2D>("Gameplay/Player/idle");
            _sendToHellTexture = content.Load<Texture2D>("Gameplay/Player/action");
            _sendToHeavenTexture = content.Load<Texture2D>("Gameplay/Player/action");
            _hurtTexture = content.Load<Texture2D>("Gameplay/Player/hurt");
            _godsFistTexture = content.Load<Texture2D>("Gameplay/Player/godsfist");
            BackgroundMusic = content.Load<Song>("Gameplay/Background/backgroundGameplay");

            _sendToHeavenSound = content.Load<SoundEffect>("Gameplay/Sounds/hurp");
            _sendToHellSound = content.Load<SoundEffect>("Gameplay/Sounds/fire");
            _hurtSound = content.Load<SoundEffect>("Gameplay/Sounds/hit");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(BackgroundMusic);
            MediaPlayer.Volume = 0.5f;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            _frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_frameTime >= _frameDuration)
            {
                _currentFrame++;
                _frameTime = 0f;

                if (_isHurt && _currentFrame >= _hurtFrameCount)
                {
                    _isHurt = false;
                    _currentFrame = 0;
                }
                if (_isSendingToHell && _currentFrame >= _sendFrameCount)
                {
                    _isSendingToHell = false;
                    _currentFrame = 0;
                }
                if (_isSendingToHeaven && _currentFrame >= _sendFrameCount)
                {
                    _isSendingToHeaven = false;
                    _currentFrame = 0;
                }
                if (!_isSendingToHell && !_isSendingToHeaven && !_isHurt && _currentFrame >= _idleFrameCount)
                {
                    _currentFrame = 0;
                }
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                SendToHeaven();
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                SendToHell();
            }

            UpdateGodsFist(gameTime);
        }

        private void UpdateGodsFist(GameTime gameTime)
        {
            if (_isHurt)
            {
                _godsFistPosition.Y += FistSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_godsFistPosition.Y >= _godsFistTargetPosition.Y)
                {
                    _godsFistPosition.Y = _godsFistTargetPosition.Y;
                }
            }
            else
            {
                _godsFistPosition.Y -= FistSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_godsFistPosition.Y <= -420)
                {
                    _godsFistPosition.Y = -420;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D currentTexture = _idleTexture;
            int frameWidth = FrameWidthIdle;

            if (_isHurt)
            {
                currentTexture = _hurtTexture;
                frameWidth = FrameWidthHurt;
            }
            else if (_isSendingToHell)
            {
                currentTexture = _sendToHellTexture;
                frameWidth = FrameWidthSend;
            }
            else if (_isSendingToHeaven)
            {
                currentTexture = _sendToHeavenTexture;
                frameWidth = FrameWidthSend;
            }

            Rectangle sourceRectangle = new Rectangle(_currentFrame * frameWidth, 0, frameWidth, FrameHeight);
            spriteBatch.Draw(currentTexture, _position, sourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            
            spriteBatch.Draw(_godsFistTexture, _godsFistPosition, Color.White);
        }

        public void LoseLife()
        {
            _lives--;
            _isHurt = true;
            _currentFrame = 0;
            _godsFistPosition.Y = -420;
            _hurtSound.Play();
        }

        public void PlaySendToHeavenSound()
        {
            _sendToHeavenSound.Play();
        }

        public void PlaySendToHellSound()
        {
            _sendToHellSound.Play();
        }

        private void SendToHell()
        {
            _isSendingToHell = true;
            _currentFrame = 0;
            PlaySendToHellSound();
        }

        private void SendToHeaven()
        {
            _isSendingToHeaven = true;
            _currentFrame = 0;
            PlaySendToHeavenSound();
        }

        public void DecrementSoulsToSend()
        {
            _soulsToSend--;
        }

        public void Reset(int initialLives, int initialSoulsToSend)
        {
            _lives = initialLives;
            _soulsToSend = initialSoulsToSend;
        }

        public int Lives => _lives;
        public int SoulsToSend => _soulsToSend;
}