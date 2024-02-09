using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Starfall.AnimationManagment;
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
        public SoundEffect effect;
        private readonly List<Objects.BoundingBox> boundingBoxes;
        private readonly List<Objects.BoundingBox> Spikes;
        private readonly List<Objects.BoundingBox> Platform;
        private readonly List<Objects.BoundingBox> Collectibles;

        
        


        public GameManager()
        {

            #region Initialize
            Worldsize = new Vector2(640,360);
            effect = Global.Content.Load<SoundEffect>("jump");
            font = Global.Content.Load<SpriteFont>("Font");
            // 15 41
            player = new(Global.Content.Load<Texture2D>("Player"), new Vector2(165,150), new Vector2(15, 40),new Vector2(0,0));
            player.Hitbox = new Objects.BoundingBox(player.Position.X,player.Position.Y,player.Size.X,player.Size.Y);
            Background = Global.Content.Load<Texture2D>("BackGroundRun");
            collisionHandler = new CollisionHandler();
            gameCamera = new GameCamera();


            

            map = new TmxMap("Content/Levels/Actual Levels/Level2.tmx");
            var Tileset = Global.Content.Load<Texture2D>("" + map.Tilesets[0].Name.ToString());
            var tileWidth = map.Tilesets[0].TileWidth;
            var tileHeight = map.Tilesets[0].TileHeight;
            var TileSetTilesWide = Tileset.Width / tileWidth;
            maploader = new MapLoader(map, Tileset, TileSetTilesWide, tileWidth, tileHeight);

            //initialize Map BoundingBoxes
            boundingBoxes = new List<Objects.BoundingBox>();
            Spikes = new List<Objects.BoundingBox>();
            Platform = new List<Objects.BoundingBox>();
            Collectibles = new List<Objects.BoundingBox>();

            for (int i = 0; i < map.ObjectGroups["Ground"].Objects.Count; i++)
            {
                TmxObject o = map.ObjectGroups["Ground"].Objects[i];

                boundingBoxes.Add(new Objects.BoundingBox((float)o.X, (float)o.Y, (float)o.Width, (float)o.Height));
            }

            //Initialize Map Spike Boxes
            for (int i = 0; i < map.ObjectGroups["Spikes"].Objects.Count; i++)
            {
                TmxObject spike = map.ObjectGroups["Spikes"].Objects[i];

                Spikes.Add(new Objects.BoundingBox((float)spike.X, (float)spike.Y, (float)spike.Width, (float)spike.Height));
            }

            //Initialize Platforms
            for (int i = 0; i < map.ObjectGroups["Platform"].Objects.Count; i++)
            {
                TmxObject platform = map.ObjectGroups["Platform"].Objects[i];

                Platform.Add(new Objects.BoundingBox((float)platform.X, (float)platform.Y, (float)platform.Width, (float)platform.Height));
            }

            //Initialize Collectibles
            for (int i = 0; i < map.ObjectGroups["Collectibles"].Objects.Count; i++)
            {
                TmxObject platform = map.ObjectGroups["Collectibles"].Objects[i];

                Collectibles.Add(new Objects.BoundingBox((float)Collectibles.X, (float)Collectibles.Y, (float)Collectibles.Width, (float)Collectibles.Height));
            }
            #endregion

        }
        public void Update()
        {
            
            gameCamera.CalculateView(player, Worldsize, maploader);
            InputManager.Update(player, effect);
            player.Update(effect);                    
            collisionHandler.CollisionHandling(player, boundingBoxes, Spikes, Platform);         
        }

        public void Draw(Texture2D rectTexture, GameTime gameTime)
        {
            Global.SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp,transformMatrix: gameCamera.gameView);

            maploader.Draw();

           // Global.SpriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
            player.Draw();
            // Global.SpriteBatch.Draw(Background, new Vector2(player.Hitbox.X, player.Hitbox.Y) ,null, Color.Green,0f, Vector2.Zero, new Vector2(player.Size.X / Background.Width,player.Size.Y / Background.Height), SpriteEffects.None, 0f);
            //Global.SpriteBatch.Draw(rectTexture, new Rectangle((int)player.Position.X, (int)(player.Position.Y + player.Size.Y), (int)player.Size.X, 6), Color.Red);
            foreach(Objects.BoundingBox box in boundingBoxes)
            {
                Global.SpriteBatch.DrawString(font, "Velocity Y" + player.Velocity.Y+ "", player.Position + new Vector2(10,10), Color.Red);

               Global.SpriteBatch.DrawString(font, "Velocity X  " + player.Velocity.X + "", player.Position + new Vector2(20, 20), Color.Yellow);

                Global.SpriteBatch.DrawString(font, "WallLeft" + player.touchWallLeft + "", new Vector2(0, 80), Color.Red);

                Global.SpriteBatch.DrawString(font, "WallRight" + player.touchWallRight + "", new Vector2(0, 100), Color.Red);


            }


            Global.SpriteBatch.End();
        }
    }
}
