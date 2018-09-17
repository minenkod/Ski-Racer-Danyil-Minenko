using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Skir_Racer
{
    public class FootPrint 
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle = 0;       
        public float AngularVelocity = 0f;
        public int leftToLive = 200; //The print will be alive for just over 3 seconds 60 means 1 second. 
        public float Size = 1;  //The footprint will be a size 1. 
        public bool paused = false;
        public FootPrint(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
        }
       
        public void Update()
        {
            if(paused == false)
            {
                leftToLive--; //Decrease the amount of time each footstep has left to persist. 
                Position += Velocity;    //Add the posstion with the velocity. This is the direction the print is facing. 
                Angle += AngularVelocity; // Similarly add the angular velocity
            }
        }
       
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height); 
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color.Black,Angle, origin, Size, SpriteEffects.None, 0f);
        }

    }
}
