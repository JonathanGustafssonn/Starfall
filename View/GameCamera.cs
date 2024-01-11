using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Starfall.GameManagment;
using Starfall.Map;
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

        public void CalculateView(Player player, Vector2 Worldsize, MapLoader map)
        {
            //Calculate appropriate Scale factor based on the ration of the game window size to the size of the world
            float ScaleX = Global.GameWindow.X / Worldsize.X;
            float ScaleY = Global.GameWindow.Y / Worldsize.Y;
            float Scale = Math.Min(ScaleX, ScaleY);

            // Calculate the position of the camera based on the position of the player
            float targetPositionX = -player.Position.X * 3 + (Global.GameWindow.X / 2);
            float targetPositionY = -player.Position.Y * 3 + (Global.GameWindow.Y / 2);

            //clamp values as to not go beyond border of the map
            float clampedTargetPositionX = MathHelper.Clamp(targetPositionX, -map.Map.Width * map.TileWidth * 3 + (Global.GameWindow.X ), 0);
            float clampedTargetPositionY = MathHelper.Clamp(targetPositionY, -map.Map.Height * map.TileHeight * 3 + (Global.GameWindow.Y ), 0);








            //gameView = Matrix.CreateScale(3, 3, 1) * Matrix.CreateTranslation(targetPositionX, targetPositionY, 0);
            gameView = Matrix.CreateScale(3, 3, 1) * Matrix.CreateTranslation(clampedTargetPositionX, clampedTargetPositionY, 0);

        }
    }
}
