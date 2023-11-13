using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Starfall.GameManagment;
using Starfall.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.View
{
   
    public class GameCamera
    {
        public Matrix gameView;

        public void CalculateView(Viewport viewport,Player player, Vector2 Worldsize )
        {
            float ScaleX = viewport.Width / Worldsize.X;
            float ScaleY = viewport.Height / Worldsize.Y;   
            float Scale = Math.Min( ScaleX, ScaleY );



            // float viewX = (viewport.Width / 2) - player.Position.X * Scale;
            // float viewY = (viewport.Height / 2) - player.Position.Y * Scale;
            float viewX = -(0) / 2;
            float viewY = -(0) / 2;


            gameView = Matrix.CreateTranslation(viewX, viewY, 0f) * Matrix.CreateScale(Scale,Scale,1);

        }
    }
}
