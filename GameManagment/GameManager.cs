using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Starfall.AnimationManagment;
using Starfall.GameManagment.GameStates;
using Starfall.GameManagment.GameStates.Menu;
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
    //=================================================================
    public class GameManager
    {
        #region GameInstanceLogic

        static GameCamera gameCamera;
        static Texture2D Background;

        static public bool shouldExit = false;
        static Vector2 Worldsize;

        //Font for test purposes
        static SpriteFont font;

        #endregion

        //Used to determine currentState of game
        public enum State { Menu, Run, Quit };
        public static State currentState;

        //===========================================================
        // Initialize(), initializes content in the GameManager class
        //===========================================================
        public static void Initialize()
        {
            GameplayManager.GamePlayInitialize();
        }

        //=============================================
        // LoadContent(), loads project related content
        //=============================================
        public static void LoadContent(GameWindow window)
        {
            //Code for correctly setting size of game and loading background images
            Worldsize = new Vector2(640, 360); //480 270 640 360 320x180
            Background = Global.Content.Load<Texture2D>("BackGroundRun");

            //Code for loading Fonts
            font = Global.Content.Load<SpriteFont>("Font");

            //Code for loading gameCamera
            gameCamera = new GameCamera();

            //Load Gameplay content
            GameplayManager.GamePlayLoad(window);

            //Menu Content
            MenuManager.MenuManagerLoad(window);
        }

        //====================================
        // Update(), updates logic in the game
        //====================================
        public static void Update(GameTime gameTime)
        {
            switch (currentState)
            {
                case State.Run:
                    currentState = GameplayManager.GamePlayUpdate(gameTime, gameCamera, Worldsize);
                    break;
                case State.Menu:
                    currentState = MenuManager.MenuManagerUpdate(gameTime, gameCamera);
                    break;
                case State.Quit:
                    shouldExit = true;
                    break;
                default:
                    currentState = MenuManager.MenuManagerUpdate(gameTime, gameCamera);
                    break;
            }
        }
        
        //====================================
        // Draw(), draws content to the screen
        //====================================
        public static void Draw()
        {
            Global.SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: gameCamera.gameView);


            switch (currentState)
            {
                case State.Run:
                    GameplayManager.GamePlayDraw(font);
                    break;
                case State.Menu:
                    MenuManager.MenuManagerDraw(font);
                    break;
                case State.Quit:
                    shouldExit = true;
                    break;
            }


            Global.SpriteBatch.End();
        }
    }
}
