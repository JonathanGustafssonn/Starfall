using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Objects
{
    public class BoundingBox
    {

        public float X;
        public float Y;
        public float Width;
        public float Height;


        public BoundingBox(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }


    }
}
