namespace Soul_Sorter.Model;

public class Attribute
{
    public enum AttributeType
        {
            Positive,
            Negative
        }
    
        private Texture2D _texture;
        private Rectangle _sourceRectangle;
        private AttributeType _type;
        private Vector2 _relativePosition;
        private Vector2 _absolutePosition;
        private Color _color;
        private Color _glowColor;
    
        public Attribute(Texture2D texture, Rectangle sourceRectangle, AttributeType type, Vector2 relativePosition, Vector2 soulPosition)
        {
            _texture = texture;
            _sourceRectangle = sourceRectangle;
            _type = type;
            _relativePosition = relativePosition;
            _absolutePosition = soulPosition + _relativePosition; 
            _color = Color.White;
            _glowColor = Color.Transparent;
        }
    
        public void Update(Vector2 soulPosition, Vector2 mousePosition)
        {
            _absolutePosition = soulPosition + _relativePosition;
            
            Rectangle attributeRect = new Rectangle((int)_absolutePosition.X, (int)_absolutePosition.Y, _sourceRectangle.Width, _sourceRectangle.Height);
            if (attributeRect.Contains(mousePosition))
            {
                _glowColor = _type == AttributeType.Positive ? Color.Green : Color.Red;
            }
            else
            {
                _glowColor = Color.Transparent;
            }
        }
    
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_glowColor != Color.Transparent)
            {
                Rectangle glowRectangle = new Rectangle((int)_absolutePosition.X - 5, (int)_absolutePosition.Y - 5, _sourceRectangle.Width + 10, _sourceRectangle.Height + 10);
                spriteBatch.Draw(_texture, glowRectangle, _sourceRectangle, _glowColor * 0.9f);
            }
            
            spriteBatch.Draw(_texture, _absolutePosition, _sourceRectangle, _color);
        }
    
        public AttributeType Type => _type;
}