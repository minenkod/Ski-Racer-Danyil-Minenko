using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Skir_Racer
{
    class Sprite
    {
        public Vector2 position; //The possition of the sprite on the game screen. 
        public Vector2 speed;    //The X and Y speed of the sprite 
        public Texture2D texture;  //The texture that the sprite will have. 
        public Rectangle rectangle; //Take the x and y then assign and save 
        public Vector2 storedSpeed;

        public Sprite(Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, Vector2 newSpeed)
        {
            texture = newTexture;
            rectangle = newRectangle;
            position = newPosition;
            speed = newSpeed;
         
        }
   
        public virtual void Update(Game1 game)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            rectangle.X = (int)Math.Round(position.X);
            rectangle.Y = (int)Math.Round(position.Y);
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        public virtual void Save(StreamWriter sw)
        {
            sw.WriteLine( position.X + ">" + position.Y + ":" + speed.X + ":" + speed.Y); //The possition of the sprite is drawn 
            //along with a ":" symbol to split the x and y from each other
        }

        public void setPossition(int newx, int newy)   //Method used to set the posstion of the sprite. 
        {
            position.X = newx;                         //an example of use is when the skier hits a flag then they are put into a safe zone. 
            position.Y = newy;
        }

        public void setSpeed(int newx, int newy)   //Method used to set the posstion of the sprite. 
        {
            speed.X = newx;                         //an example of use is when the skier hits a flag then they are put into a safe zone. 
            speed.Y = newy;
        }
    }
}
