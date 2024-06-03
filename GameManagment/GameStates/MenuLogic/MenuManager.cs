using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Starfall.InputManagment;
using Starfall.Objects;
using Starfall.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Starfall.GameManagment.GameManager;

namespace Starfall.GameManagment.GameStates.MenuLogic
{
    public class MenuManager
    {
        //SFX
        static SoundEffect uiChoice;
        static SoundEffect uiFlip;

        // Menu values
        static Menu menu;
        static Texture2D menuSprite;
        static Vector2 menuPos;


        static Menu Options;

        public static int Counter;

        //AudioTextures
        public static Texture2D Volume100;
        public static Texture2D Volume80;
        public static Texture2D Volume60;
        public static Texture2D Volume40;
        public static Texture2D Volume20;
        public static Texture2D Volume0;
        public static Texture2D AudioON;
        public static Texture2D AudioOFF;

        //ResolutionTextures
        public static Texture2D Texture1920;
        public static Texture2D Texture1280;
        public static Texture2D Texture640;
        public static Texture2D Texture320;


        public static void MenuManagerLoad(GameWindow window)
        {   
            //Code for Menu
            menuSprite = Global.Content.Load<Texture2D>("Backgrounds/MenuBackGround2");    
            menuPos.X = 0;
            menuPos.Y = 0;
            menu = new Menu((int)State.Menu);
            menu.AddItem(window, Global.Content.Load<Texture2D>("Buttons/Options"), (int)State.Options, 1, 53f);
            menu.AddItem(window, Global.Content.Load<Texture2D>("Buttons/Play"), (int)State.Run, 1,53f);
            menu.AddItem(window, Global.Content.Load<Texture2D>("Buttons/Quit"), (int)State.Quit, 1,53f);


            //Textures Volume
            Volume100 = Global.Content.Load<Texture2D>("Buttons/OptionChoices/VolumeSlide/Volume100");
            Volume80 = Global.Content.Load<Texture2D>("Buttons/OptionChoices/VolumeSlide/Volume80");
            Volume60 = Global.Content.Load<Texture2D>("Buttons/OptionChoices/VolumeSlide/Volume60");
            Volume40 = Global.Content.Load<Texture2D>("Buttons/OptionChoices/VolumeSlide/Volume40");
            Volume20 = Global.Content.Load<Texture2D>("Buttons/OptionChoices/VolumeSlide/Volume20");
            Volume0 = Global.Content.Load<Texture2D>("Buttons/OptionChoices/VolumeSlide/Volume0");

            //Textures Resolution
            Texture1920 = Global.Content.Load<Texture2D>("Buttons/OptionChoices/ScreenResolution/1920X1080");
            Texture1280 = Global.Content.Load<Texture2D>("Buttons/OptionChoices/ScreenResolution/1280X720");
            Texture640 = Global.Content.Load<Texture2D>("Buttons/OptionChoices/ScreenResolution/640X360");
            Texture320 = Global.Content.Load<Texture2D>("Buttons/OptionChoices/ScreenResolution/320X180");

            Counter = 4;



            Options = new Menu((int)OptionState.OptionMain);
            Options.AddItem(window, Global.Content.Load<Texture2D>("Buttons/OptionChoices/ScreenResolution/1920X1080"), (int)OptionState.Resolution, 2, 23f);
            Options.AddItem(window, Volume20, (int)OptionState.VolumeMusic, 2, 23f);
            Options.AddItem(window, Volume20, (int)OptionState.VolumeSound, 2, 23f);

            //Code for loading SFX
            uiChoice = Global.Content.Load<SoundEffect>("start");
            uiFlip = Global.Content.Load<SoundEffect>("switch");
        }
        //=======================================================
        // MenuUpdate(), updates values related to the menu state
        //=======================================================
        public static State MenuManagerUpdate(GameTime gameTime, GameCamera gameCamera)
        {
            gameCamera.CalculateMenuView();
            return (State)menu.Update(gameTime, uiChoice, uiFlip);

        }

        public static OptionState OptionsUpdate(GameTime gameTime, GameCamera gameCamera)
        {
            gameCamera.CalculateMenuView();
            return (OptionState)Options.UpdateOptions(gameTime, uiChoice, uiFlip);

        }

        public static void AdjustSound(ref float Scale, int index)
        {
            if (InputManager.IsPressedOnce(Keys.Right) || InputManager.IsPressedOnce(Keys.D))
            {
                uiFlip.Play(volume: Scale, pitch: 0.0f, pan: 0.0f);
                if (Scale == 1 || Scale > 1)
                {
                    Scale = 1;
                }
                else
                {
                    Scale += 0.2f;
                }
                
                AdjustSoundTexture(ref Scale, index);
            }
            else if (InputManager.IsPressedOnce(Keys.Left) || InputManager.IsPressedOnce(Keys.A))
            {
                uiFlip.Play(volume: Scale,   pitch: 0.0f, pan: 0.0f);
                if (Scale == 0 || Scale < 0)
                {
                    Scale = 0;
                }
                else
                {
                    Scale -= 0.2f;
                }
                AdjustSoundTexture(ref Scale,index);
            }
        }

        public static void AdjustResolution(int index)
        {

            if (InputManager.IsPressedOnce(Keys.Right) || InputManager.IsPressedOnce(Keys.D))
            {
                Counter++;

            
            }
            else if (InputManager.IsPressedOnce(Keys.Left) || InputManager.IsPressedOnce(Keys.A))
            {
                Counter--;
            }

            if (Counter >= 4)
            {
                Counter = 4;
                
            }
            else if (Counter <= 1)
            {
                Counter = 1;
            }

            AdjustResolutionTexture(index);

        }

        public static void setResolution()
        {
            if (Counter >= 4)
            {
                Counter = 4;
                Global.Resolution = new Vector2(1920,1080);
            }
            else if (Counter == 3)
            {
                Counter = 3;
                Global.Resolution = new Vector2(1280,730);
            }
            else if (Counter == 2)
            {
                Counter = 2;
                Global.Resolution = new Vector2(640,360);
            }
            else if (Counter <= 1)
            {
                Counter = 1;
                Global.Resolution = new Vector2(320,180);

            }

            Global.GameWindow = new Point((int)Global.Resolution.X, (int)Global.Resolution.Y);
        }
        public static void AdjustSoundTexture(ref float Scale,int index)
        {
            Scale = (float)Math.Round(Scale, 1);

            if (Scale == 1)
            {
                Options.setButtonTexture(index, Volume100);
            }
            else if (Scale == 0.8f)
            {
                Options.setButtonTexture(index, Volume80);
            }
            else if (Scale == 0.6f)
            {
                Options.setButtonTexture(index, Volume60);
            }
            else if (Scale == 0.4f)
            {
                Options.setButtonTexture(index, Volume40);
            }
            else if (Scale == 0.2f)
            {
                Options.setButtonTexture(index, Volume20);
            }
            else if (Scale == 0f)
            {
                Options.setButtonTexture(index, Volume0);
            }
        }

        public static void AdjustResolutionTexture(int index)
        {
            


            if (Counter >= 4)
            {
                Options.setButtonTexture(index, Texture1920);
            }
            else if (Counter == 3)
            {
                Options.setButtonTexture(index, Texture1280);
            }
            else if (Counter == 2)
            {
                Options.setButtonTexture(index, Texture640);
            }
            else if (Counter <= 1)
            {
                Options.setButtonTexture(index, Texture320);
            }

        }
        //===========================================================================
        // MenuManagerDraw(), handles drawing objects to the screen in the menu State
        //===========================================================================
        public static void MenuManagerDraw(SpriteFont font)
        {
            Global.SpriteBatch.Draw(menuSprite, menuPos, Color.White);
            menu.Draw(font);
            Global.SpriteBatch.DrawString(font, "Audio " + Global.AudioScale+ "", new Vector2(10, 10), Color.Red);
        }

        public static void OptionManagerDraw(SpriteFont font)
        {
            Global.SpriteBatch.Draw(menuSprite, menuPos, Color.White);
            Options.Draw(font);
        }
    }
}
