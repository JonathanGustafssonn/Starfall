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
    //=================================================================
    // GameManager is the class containg all logic related to the game
    // things like, menus, levels, gameloops, etc.
    //
    //=================================================================
    public class GameManager
    {
        #region GameInstanceLogic

        static  SpriteFont font;
        static  TmxMap map;
        static GameCamera gameCamera;
        static MapLoader maploader;
        static Player player;
        static CollisionHandler collisionHandler;
        static Texture2D Background;
        static SoundEffect effect;
        static SoundEffect dash;
        static SoundEffect uiChoice;
        static SoundEffect uiFlip;

        static Texture2D instructionJump;

        static Menu menu;
        static Texture2D menuSprite;
        static Vector2 menuPos;

        #region LevelLogic

        static Vector2 Worldsize;
        static  List<Objects.BoundingBox> boundingBoxes;
        static List<Objects.BoundingBox> Spikes;
        static List<Objects.BoundingBox> Platform;
        static List<Objects.BoundingBox> Collectibles;

        #endregion

        #endregion

        //Used to determine currentState of game
        public enum State { Menu, Run, Quit };
        public static State currentState;



        // Add seperate update and draw methods depending on state, fix states in game1 see that everything works :) 
        public static void Initialize()
        {


            //Types of different game objects
            boundingBoxes = new List<Objects.BoundingBox>();
            Spikes = new List<Objects.BoundingBox>();
            Platform = new List<Objects.BoundingBox>();
            Collectibles = new List<Objects.BoundingBox>();
        }
        public static void LoadContent(GameWindow window)
        {
            

            //Code for correctly setting size of game and loading background images
            Worldsize = new Vector2(480, 270);
            Background = Global.Content.Load<Texture2D>("BackGroundRun");

            //Code for Menu
            menuSprite = Global.Content.Load<Texture2D>("TempMenuBackground");
            menuPos.X = 0;
            menuPos.Y = 0;




            menu = new Menu((int)State.Menu);
            menu.AddItem(window, Global.Content.Load<Texture2D>("TempMenuButton"), (int)State.Menu);
            menu.AddItem(window, Global.Content.Load<Texture2D>("TempMenuButton"),(int)State.Run);
            menu.AddItem(window, Global.Content.Load<Texture2D>("TempMenuButton"), (int)State.Quit);



            //Code for loading SFX
            effect = Global.Content.Load<SoundEffect>("jump");
            dash = Global.Content.Load<SoundEffect>("dash");
            uiChoice = Global.Content.Load<SoundEffect>("start");
            uiFlip = Global.Content.Load<SoundEffect>("switch");

            //Code for loading Fonts
            font = Global.Content.Load<SpriteFont>("Font");

            //Code for loading player variables
            player = new(Global.Content.Load<Texture2D>("Player"), new Vector2(165, 150), new Vector2(15, 40), new Vector2(0, 0));
            player.Hitbox = new Objects.BoundingBox(player.Position.X, player.Position.Y, player.Size.X, player.Size.Y);

            //Code for loading gameCamera
            gameCamera = new GameCamera();

            //Code for loading Collision
            collisionHandler = new CollisionHandler();

            //Code for loading mapLoader
            instructionJump = Global.Content.Load<Texture2D>("instructionJump");

            map = new TmxMap("Content/Levels/Actual Levels/Level1-Room3.tmx");
            var Tileset = Global.Content.Load<Texture2D>("" + map.Tilesets[0].Name.ToString());
            var tileWidth = map.Tilesets[0].TileWidth;
            var tileHeight = map.Tilesets[0].TileHeight;
            var TileSetTilesWide = Tileset.Width / tileWidth;
            maploader = new MapLoader(map, Tileset, TileSetTilesWide, tileWidth, tileHeight);

            //Code for loading every instance of objects in game world
            #region MapObjectInstances
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

        public static State RunUpdate(GameTime gameTime)
        {
            gameCamera.CalculateView(player, Worldsize, maploader);
            player.Update(effect,gameTime,dash);
            collisionHandler.CollisionHandling(player, boundingBoxes, Spikes, Platform, Collectibles);

            return State.Run;
        }

        public static void RunDraw()
        {
            Global.SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: gameCamera.gameView);

            maploader.Draw();
                Global.SpriteBatch.Draw(instructionJump, new Vector2(208, 208), Color.White);
            // Global.SpriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
            player.Draw();
            // Global.SpriteBatch.Draw(Background, new Vector2(player.Hitbox.X, player.Hitbox.Y) ,null, Color.Green,0f, Vector2.Zero, new Vector2(player.Size.X / Background.Width,player.Size.Y / Background.Height), SpriteEffects.None, 0f);
            //Global.SpriteBatch.Draw(rectTexture, new Rectangle((int)player.Position.X, (int)(player.Position.Y + player.Size.Y), (int)player.Size.X, 6), Color.Red);
            foreach (Objects.BoundingBox box in boundingBoxes)
            {
                

                Global.SpriteBatch.DrawString(font, "Velocity Y" + player.Velocity.Y + "", player.Position + new Vector2(10, 10), Color.Red);

                Global.SpriteBatch.DrawString(font, "Velocity X  " + player.Velocity.X + "", player.Position + new Vector2(20, 20), Color.Yellow);

                Global.SpriteBatch.DrawString(font, "MoveDir " + player.isDashing + "", player.Position + new Vector2(0, 0), Color.Green);

                Global.SpriteBatch.DrawString(font, "WallLeft" + player.touchWallLeft + "", new Vector2(0, 80), Color.Red);

                Global.SpriteBatch.DrawString(font, "WallRight" + player.touchWallRight + "", new Vector2(0, 100), Color.Red);


            }


            Global.SpriteBatch.End();
        }

        public static State MenuUpdate(GameTime gameTime)
        {
            gameCamera.CalculateMenuView();
            return (State)menu.Update(gameTime,uiChoice,uiFlip);
        }

        public static void MenuDraw()
        {
            
            Global.SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: gameCamera.gameView); 


            Global.SpriteBatch.Draw(menuSprite,menuPos,Color.White);
            menu.Draw(font);

            Global.SpriteBatch.End();
            
        }
    }
}
