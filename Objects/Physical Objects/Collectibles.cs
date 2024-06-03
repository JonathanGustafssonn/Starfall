using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Starfall.GameManagment;
using Starfall.GameManagment.GameStates;
using Starfall.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Objects.Physical_Objects
{
    public class Egg
    {
        public Texture2D texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public bool isCollected { get; set; }

        public float OgPosY;

        public Egg(Texture2D Texture,Vector2 Position, Vector2 Size)
        {
            this.texture = Texture;
            this.Position = Position;
            this.Size = Size;
            isCollected = false;
            OgPosY = Position.Y;
        }

        public void Update(Player player)
        {
            if (!isCollected  && player.Position.Y + player.Size.Y >= Position.Y &&
                player.Position.Y <= Position.Y + Size.Y &&
                player.Position.X <= Position.X + Size.X &&
                player.Position.X + player.Size.X >= Position.X)
            {
                isCollected = true;
            }
        }

        public void Draw()
        {
            if (!isCollected)
            {
                Global.SpriteBatch.Draw(texture,new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color.White);
            }
        }
    }
}
