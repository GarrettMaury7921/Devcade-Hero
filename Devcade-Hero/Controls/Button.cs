﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
// MODIFIED VERSION OF Oyyou's MonoGame_Tutorials #13. All credit goes to Oyyou for the original code.
// https://github.com/Oyyou/MonoGame_Tutorials/tree/master/MonoGame_Tutorials/Tutorial013


namespace DevcadeHero.Controls
{
    /* 
    Class Button:
        Button Constructor
        Rectangle getter
        @ Button Method
        @ Draw Method
        @ Update Method
    */
    public class Button : Component
    {
        #region Fields
        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private SpriteFont _font;
        private Texture2D _texture;
        private bool _isHovering;
        #endregion

        #region Properties
        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }
        public bool _isSlider { get; set; }
        // Set the default text offset to zero
        public Vector2 textOffset { get; set; } = Vector2.Zero;
        public int _scale { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width * _scale, _texture.Height * _scale);
            }
        }

        public string Text { get; set; }
        #endregion

        #region Methods
        public Button(Texture2D texture, SpriteFont font, int scale)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.White;
            _scale = scale;
        }

        public void EnterButtonHit()
        {
            Click?.Invoke(this, new EventArgs());
        }

        // Draw Method
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            // When the cursor is hovering
            if (_isHovering && !_isSlider)
                colour = Color.Gray;

            // When the cursor is on a slider
            if (_isSlider)
                colour = Color.Red;

            spriteBatch.Draw(_texture, Rectangle, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2) + textOffset.X;
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2) + textOffset.Y;

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }

        }

        // Update Method
        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }

        } // Update class

        #endregion
    } // Button class

} // Name space