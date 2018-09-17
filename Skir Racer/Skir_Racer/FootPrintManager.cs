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
    public class FootPrintManager 
    {
        //Class is a modification of tutorial code from http://rbwhitaker.wikidot.com
        //I have changed alot of behaviours from the class I copied but will include this just incase. 


        public Vector2 EmitterLocation { get; set; } //This takes the values of the skier X and Y position coordinates. 
        private List<FootPrint> particles;         //Will store all the particles created then draw + update them. 
        private Texture2D texture;
        public bool paused;
        public int spawnTime = 10; ////Spawn a foot print every x seconds. eg 30 =  0.5 of a second.  

        public FootPrintManager(Texture2D newTextures, Vector2 location)   
        {
            EmitterLocation = location;
                texture = newTextures;
            particles = new List<FootPrint>();
  
        }
        int timer = 0;
        public void Update()
        {
            timer += 1;
            if (timer == spawnTime)  //The timer matches the spawn time and a footprint is spawned. 
            {
                particles.Add(GenerateNewParticle()); //A particle stream is generated
                timer = 0;
            }

            for (int particle = 0; particle < particles.Count; particle++) //Loop throug all footprint in the list. 
            {
                if(paused == false)
                particles[particle].Update();//Update each unique footprint in the list. 

                if (particles[particle].leftToLive <= 0) //The footprint has run out of time therefore it should be removed. 
                {
                    particles.RemoveAt(particle); //Removed from the list. 
                    particle--;  
                }
            }
        }

        private FootPrint GenerateNewParticle()
        {
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(0,-1);//The direction of the footprint emited.
            return new FootPrint(texture, position, velocity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}

