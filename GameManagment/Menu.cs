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
using System.Text;
using System.Threading.Tasks;

namespace Starfall.GameManagment
{
    class MenuItem
    {
        Texture2D texture;
        Vector2 position;
        int currentState;

        //MenuItem() constructor responsible for different values for different menu choices
        public MenuItem(Texture2D texture, Vector2 position, int currentState)
        {
            this.texture = texture;
            this.position = position;
            this.currentState = currentState;
        }
        //Get values for MenuItem

        public Texture2D Texture { get { return texture; } }
        public Vector2 Position { get { return position;} }
        public int State { get { return currentState; } }

    }
    public class Menu
    {
        List<MenuItem> menu;
        int selected = 0;

        float currentHeight;

        int defaultMenuState;


        //====================================================
        //Menu(), is a constructor for the list with MenuItems
        //====================================================
        public Menu(int defaultMenuState)
        {
            menu = new List<MenuItem>();
            this.defaultMenuState = defaultMenuState;
        }

        //================================================================
        // AddItem(), a function which adds new MenuItems to the menu list 
        //================================================================
        public void AddItem(GameWindow window, Texture2D itemTexture, int State)
        {
            //Define height of item

            float X = ((Global.GameWindow.X / 2 / 12) - (itemTexture.Width / 2 )) ;

            float Y = 0 + currentHeight / 12;
            





            // Change currentHeight every item by a set amount of pixels (20px)
            currentHeight += itemTexture.Height + (29.2f * 12);

            //Create temporary object to add to list
            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), State);
            menu.Add(temp);
            
        }




        //========================================================
        // Update checks for key presses, currently set for W S or 
        // Arrow keys, also checks for confirmation with Enter,
        // Will Add for press with mouse later
        //========================================================

        public int Update(GameTime gameTime,SoundEffect choice, SoundEffect flip)
        {
            
            float currentTime = Global.Time;
            InputManager.GetState();
            KeyboardState keyBoardState = Keyboard.GetState();

            if (InputManager.IsPressedOnce(Keys.S) || InputManager.IsPressedOnce(Keys.Down))
            {
                selected++;
                flip.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);

                if (selected > menu.Count - 1)
                {
                    selected = 0;
                }
            }
            else if (InputManager.IsPressedOnce(Keys.W) || InputManager.IsPressedOnce(Keys.Up))
            {
                selected--;
                flip.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);

                if (selected < 0)
                {
                    selected = menu.Count - 1;
                }
            }





            if (InputManager.IsPressed(Keys.Enter))
            {
                choice.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);
                return menu[selected].State;
            }
            else
            {
                return defaultMenuState;
            }

        }

        //=======================
        // Draw(), draws the menu
        //=======================


        public void Draw(SpriteFont font)
        {
            for (int i = 0; i < menu.Count; i++)
            {

                if (i == selected)
                {
                    Global.SpriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.Black);
                }
                else
                {
                    Global.SpriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.White);
                }
            }
        }
        

    }
}
