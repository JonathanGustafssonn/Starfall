using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Starfall.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Starfall.GameManagment.GameManager;

namespace Starfall.GameManagment.GameStates.Menu
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


        public static void MenuManagerLoad(GameWindow window)
        {   
            //Code for Menu
            menuSprite = Global.Content.Load<Texture2D>("TempMenuBackground");
            menuPos.X = 0;
            menuPos.Y = 0;

            menu = new Menu((int)State.Menu);
            menu.AddItem(window, Global.Content.Load<Texture2D>("TempMenuButton"), (int)State.Menu);
            menu.AddItem(window, Global.Content.Load<Texture2D>("TempMenuButton"), (int)State.Run);
            menu.AddItem(window, Global.Content.Load<Texture2D>("TempMenuButton"), (int)State.Quit);

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

        //===========================================================================
        // MenuManagerDraw(), handles drawing objects to the screen in the menu State
        //===========================================================================
        public static void MenuManagerDraw(SpriteFont font)
        {
            Global.SpriteBatch.Draw(menuSprite, menuPos, Color.White);
            menu.Draw(font);
        }
    }
}
