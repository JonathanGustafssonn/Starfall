using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Starfall.GameManagment;
using Starfall.Objects;
using Starfall.Objects.Physical_Objects;
using Starfall.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Starfall.Map
{
    public class levelManager
    {

        public static int currentIndex = 0;
        public static int levelindex = 0;
        static Texture2D eggTexture;
        static Texture2D BackgroundTexture;

        private static float delayTime;
        private static bool isDelayed;

        private const float StartDelayDuration = 0.4f; // 0.4 seconds delay
        private const float LevelChangeDelayDuration = 0.4f; // 0.4 seconds delay

        //Map data
        static TmxMap map;
        public static MapLoader maploader;

        //Gameplay instruction image for jumping (will be changed)
        static Texture2D instructionJump;



        //Object lists for Levels
        #region LevelLogic
        public static List<Level> levels;

        public static List<Objects.BoundingBox> boundingBoxes;
        public static List<Objects.BoundingBox> Spikes;
        public static List<Objects.BoundingBox> Platform;
        //public static List<Objects.BoundingBox> Collectibles;

        #endregion

        public static void Initialize()
        {

            BackgroundTexture = Global.Content.Load<Texture2D>("Backgrounds/Background1");
            eggTexture = Global.Content.Load<Texture2D>("Collectibles/EGG");
            levels = new List<Level>();


            //Tutorial 1
            Level Tut1 = new Level();
            Tut1.Rooms.Add(new Room(320, 180, new Vector2(8, 0))); // Room 1
            Tut1.levelName = "Tutorial1.tmx";
            Tut1.eggs.Add(new Egg(eggTexture, new Vector2(300, 108), new Vector2(14, 16)));
            Tut1.spawnPosition = new Vector2(86, 157);
            levels.Add(Tut1);
            //Tutorial 2
            Level Tut2 = new Level();
            Tut2.Rooms.Add(new Room(320, 180, new Vector2(8, 0))); // Room 1
            Tut2.levelName = "Tutorial2.tmx";
            Tut2.eggs.Add(new Egg(eggTexture, new Vector2(304, 100), new Vector2(14, 16)));
            Tut2.spawnPosition = new Vector2(43, 47);
            levels.Add(Tut2);

            //Tutorial 3
            Level Tut3 = new Level();
            Tut3.Rooms.Add(new Room(320, 180, new Vector2(8, 0))); // Room 1
            Tut3.levelName = "Tutorial3.tmx";
            Tut3.eggs.Add(new Egg(eggTexture, new Vector2(304, 100), new Vector2(14, 16)));
            Tut3.spawnPosition = new Vector2(43, 47);
            levels.Add(Tut3);

            //Level 1
            Level level1 = new Level();
            level1.Rooms.Add(new Room(320, 180, new Vector2(8, 0))); // Room 1
            level1.levelName = "Test.tmx";
            level1.eggs.Add(new Egg(eggTexture ,new Vector2(65, 28), new Vector2(14, 16)));
            level1.spawnPosition = new Vector2(40, 152);
            levels.Add(level1);

            //Level 2
            Level level2 = new Level();
            level2.Rooms.Add(new Room(320, 180, new Vector2(8, 0))); // Room 1
            level2.spawnPosition = new Vector2(30, 135);
            level2.eggs.Add(new Egg(eggTexture, new Vector2(300, 100), new Vector2(14, 16)));
            level2.levelName = "Level-1.tmx";
            levels.Add(level2);

            //Level 3
            Level level3 = new Level();
            level3.Rooms.Add(new Room(320, 180, new Vector2(8, 0))); // Room 1
            level3.eggs.Add(new Egg(eggTexture, new Vector2(300, 110), new Vector2(14, 16)));
            level3.levelName = "Level3.tmx";
            level3.spawnPosition = new Vector2(16, 119);
            levels.Add(level3);

            // level 4

            // level 5

            // level 6

            // level 7

            // level 8

            // level 9

            // level 10

            //Types of different game objects
            boundingBoxes = new List<Objects.BoundingBox>();
            Spikes = new List<Objects.BoundingBox>();
            Platform = new List<Objects.BoundingBox>();
            //Collectibles = new List<Objects.BoundingBox>();
        }
        public static void Load(Player player)
        {
            #region Temp

            player.canMove = false;
            isDelayed = true;
            delayTime = StartDelayDuration;

            //Code for loading mapLoader

            instructionJump = Global.Content.Load<Texture2D>("instructionJump");

            map = new TmxMap("Content/Levels/Actual Levels/" + levels[currentIndex].levelName + "");
            var Tileset = Global.Content.Load<Texture2D>("" + map.Tilesets[0].Name.ToString());
            var tileWidth = map.Tilesets[0].TileWidth;
            var tileHeight = map.Tilesets[0].TileHeight;
            var TileSetTilesWide = Tileset.Width / tileWidth;
            maploader = new MapLoader(map, Tileset, TileSetTilesWide, tileWidth, tileHeight);

            boundingBoxes.Clear();
            Spikes.Clear();
            Platform.Clear();


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


            #endregion
            #endregion
        }
        public static void Draw()
        {
            Global.SpriteBatch.Draw(BackgroundTexture,new Vector2(8,0), Color.White);
            maploader.Draw();
            foreach (Egg egg in levels[currentIndex].eggs)
            {
                egg.Draw();
            }
            
        }

        public static void Update(Player player, GameTime gameTime)
        {

            if (isDelayed)
            {
                delayTime -= Global.Time;
                if (delayTime <= 0)
                {
                    isDelayed = false;
                    player.canMove = true;
                }
                return;
            }

            if (levels[currentIndex].eggs.Count > 0)
            {
                foreach (Egg egg in levels[currentIndex].eggs)
                {
                    egg.Update(player);
                }


                bool allEggsTaken = true;
                foreach (Egg egg in levels[currentIndex].eggs)
                {
                    if (!egg.isCollected)
                    {
                        allEggsTaken = false;
                        break;
                    }
                }

                if (allEggsTaken)
                {
                    ChangeLevel(player,gameTime);
                }
            }

           


        }
        public static void ChangeLevel(Player player, GameTime gameTime)
        {

            if (currentIndex < levels.Count - 1)
            {
                GameManager.transition.Load();
                currentIndex++;
                player.spawnPoint = levels[currentIndex].spawnPosition;
                player.Score++;
                player.Velocity = new Vector2(0, 0);
                player.Position = player.spawnPoint;

                isDelayed = true;
                delayTime = LevelChangeDelayDuration;
                player.canMove = false;

                Load(player);
                
                
            }
            else
            {
                GameManager.shouldExit = true;
            }
        }
    }

    public class Level
    {
        public string levelName { get; set; }
        public List<Room> Rooms { get; set; }
        public Vector2 spawnPosition { get; set; }

        public List<Egg> eggs { get; set; }

        public Level()
        {
            Rooms = new List<Room>();
            eggs = new List<Egg>();
        }
        
    }

    public class Room
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Pos { get; set; }


        public Room(int width, int height, Vector2 pos)
        {
            Width = width;
            Height = height;
            Pos = pos;
        }
    }




}
