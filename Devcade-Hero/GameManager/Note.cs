﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DevcadeGame.GameManager
{
    // NOTE VALUES
    //  N 4 = blue1
    //  N 5 = blue2
    //  N 6 = blue3
    //  N 7 = blue4
    //  N 0 = red
    //  N 1 = blue5
    //  N 2 = green
    //  N 3 = white

    public class Note
    {
        public Texture2D Texture { get; set; } // the texture for the note
        public float Tick { get; set; }
        public float Tick_Length { get; set; }
        public Vector2 fretLinePos { get; set; }
        public float fretLineRotationAngle { get; set; }
        public int Lane { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double NoteTime { get; set; }
        public float NoteSpeed { get; set; }

        // Attributes
        private Rectangle _position; // private field to store the position

        public Rectangle Position
        {
            get => _position; // Getter returns the private field
            set => _position = new Rectangle(value.X, value.Y, value.Width, value.Height); // Setter creates a new Rectangle with the updated position
        }

        public Note(Texture2D texture, float tick, float tick_length, int lane, int PreferredBackBufferWidth,
            int PreferredBackBufferHeight, int note_width, int note_height, double time_between_notes)
        {
            Texture = texture;
            Tick = tick;
            Tick_Length = tick_length;
            Lane = lane;
            Width = PreferredBackBufferWidth;
            Height = PreferredBackBufferHeight;
            NoteTime = time_between_notes;
            NoteSpeed = 3.999969399f;
        }

        int count = 0;
        public void CalculatePosition(Rectangle noteRect, float fretLineRotationAngle, int note_width, int note_height)
        {
            // Calculate the position of the note, taking into account the fret line's position and rotation
            Vector2 notePos = new(_position.X + (note_width / 2), noteRect.Center.Y);
            Matrix rotationMatrix = Matrix.CreateRotationZ(fretLineRotationAngle);
            notePos = Vector2.Transform(notePos - noteRect.Center.ToVector2(), rotationMatrix) + noteRect.Center.ToVector2();

            // Set the position of the note
            float xOffset = 0;
            float yOffset = NoteSpeed;

            count++;
            switch (Lane)
            {
                case 4:
                    // Calculate offset for lane 1
                    if (count % 10 == 0)
                    {
                        xOffset = -3.21f;
                    }
                    break;
                case 5:
                    // Calculate offset for lane 2
                    if (count % 10 == 0)
                    {
                        xOffset = -3.0000048f;
                    }
                    break;
                case 6:
                    // Calculate offset for lane 3
                    if (count % 10 == 0)
                    {
                        xOffset = -3.000007f;
                    }
                    break;
                case 7:
                    // Calculate offset for lane 4
                    if (count % 20 == 0)
                    {
                        xOffset = -2.2f;
                    }
                    break;
                case 0:
                    // Calculate offset for lane 5
                    if (count % 10 == 0)
                    {
                        xOffset = 1.7f;
                    }
                    break;
                case 1:
                    // Calculate offset for lane 6
                    if (count % 10 == 0)
                    {
                        xOffset = 2.3f;
                    }
                    break;
                case 2:
                    // Calculate offset for lane 7
                    if (count % 10 == 0)
                    {
                        xOffset = 3.001f;
                    }
                    break;
                case 3:
                    // Calculate offset for lane 8
                    if (count % 10 == 0)
                    {
                        xOffset = 3.21f;
                    }
                    break;
            }

            notePos.X += xOffset;
            notePos.Y += yOffset;
            Position = new Rectangle((int)(notePos.X - note_width / 2), (int)(notePos.Y - note_height / 2), note_width, note_height);
        }
    }
}