using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.GameManagment
{
    public static class Global
    {
        public static float Time { get; set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static GraphicsDevice graphicsDevice { get; set; }
        public static  Point GameWindow { get; set; }

        public static float AudioScale = 0.2f;
        public static float MusicScale = 0.2f;
        public static Vector2 Resolution = new Vector2(1920,1080);



        public static void Update(GameTime gameTime)
        {
            Time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
