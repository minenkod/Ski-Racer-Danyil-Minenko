using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace Skir_Racer
{
    class Cheese : Sprite
    {
        //The cheese derived class is kept basic with no special behaviours. 
        //The main game class determines when the cheese should be drawn. 

        public Cheese(Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, Vector2 newSpeed)
            : base(newTexture, newRectangle, newPosition, newSpeed)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
