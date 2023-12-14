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

        public void CalculateView(Player player, Vector2 Worldsize )
        {
            /*
            float ScaleX = viewport.Width / Worldsize.X;
            float ScaleY = viewport.Height / Worldsize.Y;   
            float Scale = Math.Min( ScaleX, ScaleY );
            */

            //float transformedTargetX = player.Position.X * Scale;
            //float transformedTargetY = player.Position.Y * Scale;


             float viewX = (Global.GameWindow.X / 2) - player.Position.X;
             float viewY = (Global.GameWindow.Y / 2) - player.Position.Y;
            //  float viewX = -(0) / 2;
            //  float viewY = -(0) / 2;


            gameView = Matrix.CreateTranslation(viewX, viewY, 0); //* Matrix.CreateScale(Scale,Scale,1);

        }
    }
}
