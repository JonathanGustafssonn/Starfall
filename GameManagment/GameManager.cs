using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Starfall.InputManagment;
using Starfall.Map;
using Starfall.Objects;
using Starfall.Physics;
using Starfall.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Starfall.GameManagment
{
    public class GameManager
    {
        public Vector2 Worldsize;
        private readonly SpriteFont font;
        private readonly TmxMap map;
        private readonly GameCamera gameCamera;
        private readonly MapLoader maploader;
        private readonly Player player;
        private readonly CollisionHandler collisionHandler;
        private readonly Texture2D Background;

        private readonly List<Objects.BoundingBox> boundingBoxes;
        public GameManager()
        {

            #region Initialize
            Worldsize = new Vector2(640,360);

            font = Global.Content.Load<SpriteFont>("Font");

            player = new(Global.Content.Load<Texture2D>("CharacterCube"), new Vector2(0,0), new Vector2(32, 64),new Vector2(0,0));
            player.Hitbox = new Objects.BoundingBox(player.Position.X,player.Position.Y,player.Size.X,player.Size.Y);
            Background = Global.Content.Load<Texture2D>("BackGroundRun");
            collisionHandler = new CollisionHandler();
            gameCamera = new GameCamera();

            map = new TmxMap("Content/Levels/Actual Levels/Level-1.tmx");
            var Tileset = Global.Content.Load<Texture2D>("" + map.Tilesets[0].Name.ToString());
            var tileWidth = map.Tilesets[0].TileWidth;
            var tileHeight = map.Tilesets[0].TileHeight;
            var TileSetTilesWide = Tileset.Width / tileWidth;
            maploader = new MapLoader(map, Tileset, TileSetTilesWide, tileWidth, tileHeight);

            //initialize Map BoundingBoxes
            boundingBoxes = new List<Objects.BoundingBox>();

            for(int i = 0; i < map.ObjectGroups["Ground"].Objects.Count; i++)
            {
                TmxObject o = map.ObjectGroups["Ground"].Objects[i];

                boundingBoxes.Add(new Objects.BoundingBox((float)o.X, (float)o.Y, (float)o.Width, (float)o.Height));
            }
            #endregion

        }
        public void Update()
        {
            InputManager.Update();
            player.Update();

           

            

            

            collisionHandler.CollisionHandling(player, boundingBoxes);







            gameCamera.CalculateView(Global.graphicsDevice.Viewport,player, Worldsize);
        }

        public void Draw(Texture2D rectTexture)
        {
            Global.SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp,transformMatrix: gameCamera.gameView);

            maploader.Draw();
           // Global.SpriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
            player.Draw();
            // Global.SpriteBatch.Draw(Background, new Vector2(player.Hitbox.X, player.Hitbox.Y) ,null, Color.Green,0f, Vector2.Zero, new Vector2(player.Size.X / Background.Width,player.Size.Y / Background.Height), SpriteEffects.None, 0f);
            //Global.SpriteBatch.Draw(rectTexture, new Rectangle((int)player.Position.X, (int)(player.Position.Y + player.Size.Y), (int)player.Size.X, 6), Color.Red);
            foreach(Objects.BoundingBox box in boundingBoxes)
            {
              //  Global.SpriteBatch.Draw(rectTexture, new Rectangle((int)box.X, (int)box.Y, (int)box.Width, (int)box.Height), Color.Purple);
                Global.SpriteBatch.DrawString(font, "Are we colliding?" + player.isGrounded + "", Vector2.Zero, Color.Red);
            }
            

            Global.SpriteBatch.End();
        }
    }
}
