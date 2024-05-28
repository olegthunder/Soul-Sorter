using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soul_Sorter.Content;

public class Player
{
        private Texture2D _idleTexture;
        private Texture2D _sendToHellTexture;
        private Texture2D _sendToHeavenTexture;
        private Texture2D _loseLifeTexture;
        private Vector2 _position;
        private int _lives;
        private int _soulsToSend;
        private float _decisionTimer;
        private const float DecisionTimeLimit = 10f; // 10 секунд на принятие решения

        public Player()
        {
            _position = new Vector2(100, 100); // Начальная позиция Иисуса
            _lives = 3;
            _soulsToSend = 10; // Примерное значение, изменить по необходимости
            _decisionTimer = DecisionTimeLimit;
        }

        public void LoadContent(ContentManager content)
        {
            _idleTexture = content.Load<Texture2D>("Player/idle");
            _sendToHellTexture = content.Load<Texture2D>("Player/sendToHell");
            _sendToHeavenTexture = content.Load<Texture2D>("Player/sendToHeaven");
            _loseLifeTexture = content.Load<Texture2D>("Player/loseLife");
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            _decisionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                SendToHeaven();
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                SendToHell();
            }

            if (_decisionTimer <= 0)
            {
                LoseLife();
                _decisionTimer = DecisionTimeLimit; // Сбросить таймер
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_idleTexture, _position, Color.White); // Пример отрисовки
        }

        private void SendToHell()
        {
            _soulsToSend--;
            // Добавить логику отправки в ад
        }

        private void SendToHeaven()
        {
            _soulsToSend--;
            // Добавить логику отправки в рай
        }

        private void LoseLife()
        {
            _lives--;
            // Добавить логику потери жизни
        }

        public int Lives => _lives;
        public int SoulsToSend => _soulsToSend;
        public float DecisionTimer => _decisionTimer;
}