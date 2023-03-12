using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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

        // Thanks ChatGPT

        #region Fields
        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private readonly Texture2D _texture;
        private readonly Texture2D _thumbTexture;
        private float _value;
        private bool _isDragging;
        private readonly string _type;
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
        public Slider(Texture2D texture, Texture2D thumbTexture, string type)
        {
            PenColour = Color.Black;
            _texture = texture;
            _thumbTexture = thumbTexture;
            _value = MediaPlayer.Volume;
            this._type = type;
        }

        // Draw Method
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var thumbRectangle = new Rectangle((int)(Position.X + _value * (_texture.Width - _thumbTexture.Width)), (int)Position.Y, _thumbTexture.Width, _texture.Height);
            // Draw thumb texture last
            spriteBatch.Draw(_texture, Rectangle, Color.White);
            spriteBatch.Draw(_thumbTexture, thumbRectangle, Color.White);
        }

        // Update Method
        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            // If the mouse goes over the slider
            if (mouseRectangle.Intersects(Rectangle))
            {
                if (_currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
                {
                    _isDragging = true;
                }
            }

            // If you're dragging with the mouse
            if (_isDragging)
            {
                _value = (_currentMouse.X - Position.X - _thumbTexture.Width / 2) / (_texture.Width - _thumbTexture.Width);
                _value = MathHelper.Clamp(_value, 0f, 1f);

                // When you release the button
                if (_currentMouse.LeftButton == ButtonState.Released)
                {
                    _isDragging = false;
                }

                // Change the sound
                if (_type == "music")
                {
                    MediaPlayer.Volume = _value;
                }
                else
                {
                    SoundEffect.MasterVolume = _value;
                }

            } // _isDragging 
        } // Update method

        #endregion

    } // Slider.cs
} // name space
