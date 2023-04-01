using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Note
{
    public Texture2D Texture { get; set; } // the texture for the note
    public float Tick { get; set; }
    public float Tick_Length { get; set; }
    public Vector2 fretLinePos { get; set; }
    public float fretLineRotationAngle { get; set; }

    // Attributes
    private Rectangle _position; // private field to store the position

    public Rectangle Position
    {
        get => _position; // Getter returns the private field
        set => _position = new Rectangle(value.X, value.Y, value.Width, value.Height); // Setter creates a new Rectangle with the updated position
    }

    public Note(Texture2D texture, float tick, float tick_length, int lane, int PreferredBackBufferWidth,
        int PreferredBackBufferHeight, int note_width, int note_height)
    {
        Texture = texture;
        Tick = tick;
        Tick_Length = tick_length;
    }

    public void CalculatePosition(Rectangle fretLineRect, float fretLineRotationAngle, int note_width, int note_height)
    {
        // Calculate the position of the note, taking into account the fret line's position and rotation
        Vector2 notePos = new(_position.X + (note_width / 2), fretLineRect.Center.Y);
        Matrix rotationMatrix = Matrix.CreateRotationZ(fretLineRotationAngle);
        notePos = Vector2.Transform(notePos - fretLineRect.Center.ToVector2(), rotationMatrix) + fretLineRect.Center.ToVector2();

        // Set the position of the note
        Position = new Rectangle((int)notePos.X - (note_width / 2), (int)notePos.Y - (note_height / 2) + 1, note_width, note_height);
    }


}
