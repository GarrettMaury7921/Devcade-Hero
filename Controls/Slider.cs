using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DevcadeGame.Controls
{
    /* 
    Class Slider:
        Slider Constructor
        Rectangle getter
        @ Slider Method
        @ Draw Method
        @ Update Method
    */
    internal class Slider : Component
    {

        #region Fields
        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private Texture2D _texture;
        private Texture2D _thumbTexture;
        private float _value;
        private bool _isDragging;
        #endregion

        #region Properties

        public Color BarColor { get; set; }
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }
        #endregion

        #region Methods
        public Slider(Texture2D texture, Texture2D thumbTexture)
        {
            _texture = texture;
            _thumbTexture = thumbTexture;
            PenColour = Color.Black;
            _value = 0.5f;
        }

        // Draw Method
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var thumbRectangle = new Rectangle((int)(Position.X + _value * (_texture.Width - _thumbTexture.Width)), (int)Position.Y, _thumbTexture.Width, _texture.Height);
            spriteBatch.Draw(_thumbTexture, thumbRectangle, Color.White);

            spriteBatch.Draw(_texture, Rectangle, Color.White);
        }

        // Update Method
        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(Rectangle))
            {
                if (_currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
                {
                    _isDragging = true;
                }
            }

            if (_isDragging)
            {
                _value = (_currentMouse.X - Position.X - _thumbTexture.Width / 2) / (_texture.Width - _thumbTexture.Width);
                _value = MathHelper.Clamp(_value, 0f, 1f);

                if (_currentMouse.LeftButton == ButtonState.Released)
                {
                    _isDragging = false;
                }
            }
        }
        #endregion
    }
}
