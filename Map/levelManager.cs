using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Starfall.GameManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Starfall.Map
{


    public class levelManager
    {
        //Map data
        static TmxMap map;
        public static MapLoader maploader;

        //Gameplay instruction image for jumping (will be changed)
        static Texture2D instructionJump;

        //Object lists for Levels
        #region LevelLogic

        public static List<Objects.BoundingBox> boundingBoxes;
        public static List<Objects.BoundingBox> Spikes;
        public static List<Objects.BoundingBox> Platform;
        public static List<Objects.BoundingBox> Collectibles;

        #endregion

        public static void Initialize()
        {
            //Types of different game objects
            boundingBoxes = new List<Objects.BoundingBox>();
            Spikes = new List<Objects.BoundingBox>();
            Platform = new List<Objects.BoundingBox>();
            Collectibles = new List<Objects.BoundingBox>();
        }
        public static void Load()
        {
            //Code for loading mapLoader
            instructionJump = Global.Content.Load<Texture2D>("instructionJump");

            map = new TmxMap("Content/Levels/Actual Levels/Test.tmx");
            var Tileset = Global.Content.Load<Texture2D>("" + map.Tilesets[0].Name.ToString());
            var tileWidth = map.Tilesets[0].TileWidth;
            var tileHeight = map.Tilesets[0].TileHeight;
            var TileSetTilesWide = Tileset.Width / tileWidth;
            maploader = new MapLoader(map, Tileset, TileSetTilesWide, tileWidth, tileHeight);

            //Code for loading every instance of objects in game world
            #region MapObjects
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
                TmxObject Collectible = map.ObjectGroups["Collectibles"].Objects[i];
                Collectibles.Add(new Objects.BoundingBox((float)Collectible.X, (float)Collectible.Y, (float)Collectible.Width, (float)Collectible.Height));
            }
            #endregion
        }

        public static void Update()
        {
      
        }

        public static void Draw()
        {
            maploader.Draw();
            Global.SpriteBatch.Draw(instructionJump, new Vector2(208, 208), Color.White);
        }
    }
}
