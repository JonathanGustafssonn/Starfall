using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Starfall.GameManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.View
{
    public class TransitionFX
    {
        private static Texture2D texture;
        List<FadeItem> items = new List<FadeItem>();

        public void Initialize()
        {
            
        }
        public void Load()
        {
            for (int xPixel = 16; xPixel - 16 < Global.GameWindow.X; xPixel += 32)
            {

                for (int yPixel = 16; yPixel - 16 < Global.GameWindow.Y; yPixel += 32)
                {
                    items.Add(new FadeItem()
                    {
                        pos = new Vector2(xPixel, yPixel),
                        Delay = xPixel
                    });
                }
            }

            texture = Global.Content.Load<Texture2D>("Fade");
        }

        public void Update(GameTime gameTime)
        {
            foreach (var item in items)
            {
                item.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }

        public void Draw()
        {

            foreach (var item in items)
            {
                Global.SpriteBatch.Draw(texture, new Vector2(item.pos.X, item.pos.Y), null, Color.White, 0f, new Vector2(16, 16), item.Scale, SpriteEffects.None, 0);   
            }
        }
    }

    class FadeItem
    {
        public Vector2 pos { get; set; }
        public float Scale
        {
            get
            {
                if (Delay > 0)
                {
                    return 2.0f;
                }
                else if (Radians > MathHelper.Pi)
                {
                    return 0f;
                }

                return (float)Math.Cos(Radians) + 1;
            }
        }

        public float Delay { get; set; }
        public float Radians { get; set; }

        public void Update(float DeltaTimeMilliSeconds)
        {
            Delay -= DeltaTimeMilliSeconds;

            if (Delay < 0)
            {
                Radians += DeltaTimeMilliSeconds / 200.0f;
            }
        }

    }
}
