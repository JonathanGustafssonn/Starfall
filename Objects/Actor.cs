using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Starfall.GameManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Objects
{

    //------------------------------------------------------------------------------------------------------------------------------//
    //  Actors are objects in the game space which are capable of movement through a change in position based on current Velocity   //
    //------------------------------------------------------------------------------------------------------------------------------//
    public class Actor
    {
        //values asociated with an Actor
        public readonly Texture2D Texture;
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Center;
        public Vector2 Velocity;
        public BoundingBox Hitbox;

        //Intializer for an Actor
        public Actor(Texture2D texture,Vector2 position, Vector2 size, Vector2 velocity)
        {
            this.Texture = texture;
            this.Position = position;
            this.Size = size;   
            this.Velocity = velocity;
            this.Center = new Vector2(position.X + size.X / 2,position.Y + size.Y / 2);
        }

        //Update logic for Actors
        public void Update()
        {
            
        }

        public void UpdateHitbox()
        {
            Hitbox = new BoundingBox(Position.X, Position.Y, Size.X, Size.Y);
        }

        //Drawing logic for Actors
        public void Draw()
        {
            Global.SpriteBatch.Draw(Texture, Position, null, Color.White);
        }

    }
}
