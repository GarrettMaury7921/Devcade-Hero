using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// MODIFIED VERSION OF Oyyou's MonoGame_Tutorials #13. All credit goes to Oyyou for the original code.
// https://github.com/Oyyou/MonoGame_Tutorials/tree/master/MonoGame_Tutorials/Tutorial013


namespace DevcadeGame
{
    /* 
    Abstract Class Component:
        @ Draw Method
        @ Update Method
    */

    public abstract class Component
	{

		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

		public abstract void Update(GameTime gameTime);

	}

}
