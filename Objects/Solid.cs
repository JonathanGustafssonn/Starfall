using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Objects
{
    //------------------------------------------------------------------------------------------------------//
    // Solids are objects in the game space which are not capable of movement, think the ground for example.//
    //------------------------------------------------------------------------------------------------------//
    public class Solid
    {
        //values asociated with a Solid
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Origin;
        public Rectangle Hitbox;



        //Intializer for a Solid
        public Solid(Vector2 position, Vector2 size, Vector2 origin, Rectangle hitbox)
        {
            this.Position = position;
            this.Size = size;
            this.Origin = origin;
            this.Hitbox = hitbox;
        }


        //Drawing logic for Solids
        public void Draw(SpriteBatch spritebatch, Texture2D texture, Rectangle rect, Color color)
        {
            spritebatch.Draw(texture, rect, color);
        }
    }
}
