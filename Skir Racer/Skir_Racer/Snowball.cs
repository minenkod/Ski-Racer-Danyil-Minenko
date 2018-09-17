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
using System.Diagnostics;

namespace Skir_Racer
{
    class Snowball : Sprite
    {
        Random r = new Random();
        public bool roll = true;

        public Snowball(Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, Vector2 newSpeed)
            : base(newTexture, newRectangle, newPosition, newSpeed)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use when drawing the snowball.</param>
        ///   <param name="game">The game object that will be updated.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //1.Draw the snowball on the middle of the playing area
            //2.The snowball has to spawn and random intervals. 
            //3.Move the snowball down the field. 
            int chance = r.Next(1, 100);
            base.Draw(spriteBatch);
        }
     
        public override void Update(Game1 game)
        {
            if(roll == true)        //Boolean value used to determine if the snowball is allowed to be rolled again.  
           {
               int randomY = r.Next(1, 430); 
               base.Update(game);
               position += speed;
               if (position.X > 800) 
               {
                   roll = false;   //The snowball has left the screen, possition is reset and put onto a random part of the screen. 
                   position.X = -70;
                   position.Y = randomY;
               }
           }
           }
        }

    }

