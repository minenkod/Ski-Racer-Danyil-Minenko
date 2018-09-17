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
    class Flag : Sprite
    {

         protected const int NARROW_LIMIT = 200; //The flags wont become narrower then the values stored in this variable. 
         public int timer; 
         public int space = 400 ;
         public bool pause = false; 
         public Rectangle rightRectangleC;
         public Rectangle leftRectangle;//detecting when to play sound when next to left flag
         public Rectangle rightRectangle;//detecting when to play sound when next to right flag
         public bool active =  true;
         public Vector2 storedFlagSpeed; 
        public Flag(Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, Vector2 newSpeed): base(newTexture, newRectangle, newPosition, newSpeed)
        {
             // flag2= new Rectangle()
        }
       
       /// <summary>
       /// 
       /// </summary>
          /// <param name="game">The current game object. </param>
        public override void Update(Game1 game)
        {
            base.Update(game);
            position -= speed;
            if(position.Y < 0)
              {
               position.Y = 480;
                //the flag is back up so it is activated. 
               active = true;
              }
         if(!pause)
         {
             timer += 1;
         }
           
            
            if(timer > 500)
            {
                if (speed.Y > 3 )
                {
                    speed.Y = 3;
                }

                if(space < 200) //The space between the flag is becoming to narrow. 
                {
                    space = NARROW_LIMIT;   //Limit the flag narrowness
                }

                space -= 20;
                speed.Y += 1;
                timer = 0;
            
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use when drawing the flags.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            leftRectangle = rectangle;
            leftRectangle.X = leftRectangle.X + 20; //sound will play when 10 points into the playfield field. 
            rightRectangle = rightRectangleC;
            rightRectangle.X = rightRectangle.X - 20;
            rightRectangleC = rectangle;
            rightRectangleC.X += space;
            spriteBatch.Draw(texture, rightRectangleC, Color.DeepSkyBlue); //The flag opposite the first flag is drawn. 
            base.Draw(spriteBatch);
        }

        public Rectangle lineRectangle()
        {
            Vector2 begin = Vector2.Zero; 
            Vector2 end =  Vector2.Zero;
            if(active == true)   //The flag hasnt been gone through already
            {
            begin = new Vector2(rectangle.X, rectangle.Y);//flag 1 pos 
            end = new Vector2(rightRectangleC.X, rightRectangleC.Y); //    flag 2 pos 
            }
            else
            {
                begin = Vector2.Zero; //The flag has already been gone through move the rectangle of the screen.
                end = Vector2.Zero;
            }
            Rectangle lineRect = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + 1, 1);
            return lineRect;
        }
    }

}
