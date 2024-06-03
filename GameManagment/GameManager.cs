using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Starfall.AnimationManagment;
using Starfall.GameManagment.GameStates;
using Starfall.GameManagment.GameStates.MenuLogic;
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
        static public bool shouldExit = false;
        static Vector2 Worldsize;
        static bool IsGameSongPlaying;
        static bool IsMenuSongPlaying;

        public static TransitionFX transition = new TransitionFX();

        //Font for test purposes
        static SpriteFont font;

        static Song song;
        static Song menuSong;

        #endregion

       

        //Used to determine currentState of game
        public enum State { Menu, Run, Quit, Options};
        public static State currentState;

        public enum OptionState { OptionMain,VolumeSound, VolumeMusic, Resolution, Mute}
        public static OptionState currentOptionState;

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
            IsGameSongPlaying = false;
            IsMenuSongPlaying = false;
            song = Global.Content.Load<Song>("MusicAndSfx/Free Copyright Music  RFM - NCM  No Copyright  Day After Day - Capturez  8 bit Music");
            menuSong = Global.Content.Load<Song>("MusicAndSfx/MenuMusic");

            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;


            //Code for correctly setting size of game and loading background images
            Worldsize = new Vector2(320, 180); //480 270 640 360 320x180 640 360

            //Code for loading Fonts
            font = Global.Content.Load<SpriteFont>("Font");

            //TransitionEffect
            transition.Load();

            //Code for loading gameCamera
            gameCamera = new GameCamera();

            //Load Gameplay content
            GameplayManager.GamePlayLoad();



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

                    if (!IsGameSongPlaying)
                    {
                        MediaPlayer.Pause();
                        MediaPlayer.Volume = Global.MusicScale;
                        MediaPlayer.Play(song);
                        IsMenuSongPlaying = false;
                        IsGameSongPlaying = true;
                    }
                       
                    transition.Update(gameTime);
                    currentState = GameplayManager.GamePlayUpdate(gameTime, gameCamera, Worldsize);
                    break;

                case State.Options:
                    MediaPlayer.Volume = Global.MusicScale;

                    if (InputManager.IsPressedOnce(Keys.Escape))
                    {
                        currentState = State.Menu;
                    }
                    currentOptionState = MenuManager.OptionsUpdate(gameTime, gameCamera);

                    switch (currentOptionState)
                    {
                        case OptionState.Resolution:
                            MenuManager.AdjustResolution(0);
                            break;
                        case OptionState.VolumeMusic:
                            MenuManager.AdjustSound(ref Global.MusicScale, 1);
                            break;
                        case OptionState.VolumeSound:

                            MenuManager.AdjustSound(ref Global.AudioScale, 2);
                            currentOptionState = OptionState.VolumeSound;

                            break;
                    }

                    
                    break;

                case State.Menu:

                    MenuManager.setResolution();

                    if (!IsMenuSongPlaying)
                    {
                        MediaPlayer.Pause();
                        MediaPlayer.Play(menuSong);
                        IsMenuSongPlaying = true;
                    }


                    if (IsGameSongPlaying)
                    {
                        MediaPlayer.Pause();
                        MediaPlayer.Play(menuSong);
                        IsGameSongPlaying = false;
                    }

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
                    transition.Draw();
                    break;
                case State.Menu:
                    MenuManager.MenuManagerDraw(font);
                    break;
                case State.Options:
                    MenuManager.OptionManagerDraw(font);
                    break;
                case State.Quit:
                    shouldExit = true;
                    break;
            }
            Global.SpriteBatch.End();



            Global.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            if (currentState == State.Run)
            {
                Global.SpriteBatch.DrawString(font, "Level " + (levelManager.currentIndex + 1) + "", new Vector2(20, 20), Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            }
            

            Global.SpriteBatch.End();

        }
    }
}
