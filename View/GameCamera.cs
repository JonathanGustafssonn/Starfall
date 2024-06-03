using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Starfall.GameManagment;
using Starfall.Map;
using Starfall.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace Starfall.View
{
   
    public class GameCamera
    {
        public Matrix gameView;
        public  static float cameraWidth = 320; //640
        public static float cameraHeight = 180; //360
        public static float cameraPosX = 0;
        public static float cameraPosY = 0;
        public void Update(Vector2 Worldsize, MapLoader map,int levelindex, List<Level> levels, Player player)
        {
            foreach (Room room in levels[levelManager.currentIndex].Rooms)
            {
                if (player.Position.X > room.Pos.X 
                    && player.Position.X + player.Size.X < room.Pos.X + room.Width 
                    && player.Position.Y > room.Pos.Y 
                    && player.Position.Y + player.Size.Y< room.Pos.Y + room.Height)
                {
                    cameraPosX = room.Pos.X;
                    cameraPosY = room.Pos.Y;
                    cameraWidth = room.Pos.X + room.Width; 
                    cameraHeight= room.Height;

                    CalculateView(player,Worldsize,map,cameraWidth,cameraHeight);
                }
                
            }
            CalculateView(player, Worldsize, map, cameraWidth, cameraHeight);
        }
        public void CalculateView(Player player, Vector2 Worldsize, MapLoader map, float cameraWidth, float cameraHeight)
        {
            float scale = Global.Resolution.X / 320;
 
            Vector2 position = new Vector2(player.Position.X - (Global.GameWindow.X /2 / scale),player.Position.Y - (Global.GameWindow.Y /2 / scale));


            position = Vector2.Clamp(position, new Vector2(cameraPosX,cameraPosY), new Vector2(cameraWidth - (Global.GameWindow.X / scale), cameraHeight - (Global.GameWindow.Y / scale)));
            // can just do Wordlview instead  of / sca le to save space and  logicality i figure
           //Second problem should be related to Global.GameWindow in the position creation


            gameView = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) * Matrix.CreateScale(new Vector3(scale, scale, 1)); 
        }

        public  void CalculateMenuView()
        {
            float Scale = Global.GameWindow.X / 160;
            gameView = Matrix.CreateScale(Scale, Scale, 0) * Matrix.CreateTranslation(0, 0, 0);
        }
    }
}
