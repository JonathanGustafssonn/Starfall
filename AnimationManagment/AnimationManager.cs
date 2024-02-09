using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Starfall.GameManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.AnimationManagment
{
    internal class AnimationManager
    {
        int numColumns;
        int numFrames;
        Vector2 size;

        int counter;
        int activeFrame;
        int interval;

        int rowPos;
        int colPos;
        public AnimationManager(int numColumns, int numFrames, Vector2 size)
        {
            this.numColumns = numColumns;
            this.numFrames = numFrames;
            this.size = size;

            counter = 0;
            activeFrame = 0;
            interval = 5;

            rowPos = 0;
            colPos = 0;
        }
        public void Update(float intervalMulti)
        {
            counter++;
            if (counter > interval * intervalMulti)
            {
                counter = 0;
                NextFrame();
            }
        }

        private void NextFrame()
        {
            activeFrame++;
            colPos++;
            if (activeFrame >= numFrames)
            {
                ResetAnimation();
            }

            if (colPos >= numColumns)
            {
                colPos = 0;
                rowPos++;
            }
        }

        private void ResetAnimation()
        {
            activeFrame = 0;
            colPos = 0;
            rowPos = 0;
        }

        public Rectangle GetFrame()
        {
            return new Rectangle(colPos * (int)size.X, rowPos * (int)size.Y, (int)size.X, (int)size.Y );
        }

        public void Draw(Texture2D texture, Vector2 pos,int hitboxAdjW,int hitboxAdjH)
        {
            Global.SpriteBatch.Draw(texture,new Vector2(pos.X + hitboxAdjW ,pos.Y + hitboxAdjH), GetFrame(), Color.White);
        }
    }
}
