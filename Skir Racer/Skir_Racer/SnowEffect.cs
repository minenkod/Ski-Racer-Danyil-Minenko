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
    class SnowEffect:Sprite
    {
        protected int graphicsWidth;
        protected int graphicsHeight; 

        public SnowEffect(Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, Vector2 newSpeed)
            : base(newTexture, newRectangle, newPosition, newSpeed)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen. </param>
        public void setGraphics(int width, int height)
        {
            graphicsWidth = width;
            graphicsHeight = height;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw the snow effect with. </param>
        public void DrawSnow(SpriteBatch spriteBatch)
        {
           spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null); //the texture is drawn with a wrap. 
         spriteBatch.Draw(texture, new Vector2(1, 1), new Rectangle(0, (int)position.Y, graphicsWidth, graphicsHeight), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0); 
           spriteBatch.End();
        }
     
        public override void Update(Game1 game)
        {
            position -= speed; //The snow posstion is taken away from the speed which makes it look like its falling down. 
            base.Update(game);
        }
    }
}
