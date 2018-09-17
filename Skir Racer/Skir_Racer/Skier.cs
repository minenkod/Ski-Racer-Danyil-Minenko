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

namespace Skir_Racer
{
    class Skier : Sprite
    {
        protected int Timer = 0; //stores the amount of time has passed.   
        Game1 gm = new Game1();
        public Vector2 storedSkierSpeed; 
        public Skier(Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, Vector2 newSpeed)
            : base(newTexture, newRectangle, newPosition, newSpeed)
        {
        
        }
        enum skierDirection    //An enum to set which direction the skier is going. 
        {
            left, right, up, down
        }
        skierDirection directionSprite; 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">The current game object.</param>
        /// <param name="direction">The direction the skier should go.</param>
        /// <param name="screenSize">The size of the game screen.</param>
        /// <param name="directionTexture">The texture which will be drawn with the sprite. </param>
        public  void Update(Game1 game, string direction, int screenSize, Texture2D directionTexture)
        {
        //    Console.WriteLine(storeSpeed);

            base.Update(game);
            switch (direction)
            {
                case "up":
                    position.Y -= speed.Y;  
                    texture = directionTexture;
             
                    if (position.Y < 0)    //Check if the sprite has left the screen from the top
                    {
                        position.Y += speed.Y;
                        directionSprite = skierDirection.up;
                    }
                    break;
            
                case "down":
                    position.Y += speed.Y;
                    texture = directionTexture;

                    if (position.Y > screenSize - 25)//Check if the sprite has left the screen from the bottom
                    {
                        position.Y -= speed.Y;
                        directionSprite = skierDirection.down;

                    }
                    break;
                
                case "left":
                    position.X -= speed.X;
                    texture = directionTexture;

                    if (position.X < 0)//Check if the sprite has left the screen from the left
                    {
                        position.X += speed.X;
                        directionSprite = skierDirection.left;

                    }
                    break;
                case "right":
                    texture = directionTexture;

                    position.X += speed.X;
                    if (position.X > screenSize - 25)//Check if the sprite has left the screen from the right
                    {
                        position.X -= speed.X;
                        directionSprite = skierDirection.right;
                    }
                    break;
            }
        }
      
        public override void Update(Game1 game)
        {
            base.Update(game);
        }
    }
}
