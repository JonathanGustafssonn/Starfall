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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Starfall.GameManagment.GameManager;

namespace Starfall.GameManagment.GameStates.MenuLogic
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
        public Vector2 Position { get { return position; } }
        public int State { get { return currentState; } }

        public void SetTexture(Texture2D newTexture)
        {
            texture = newTexture;
        }

    }
    public class Menu
    {
        List<MenuItem> menu;
        int selected = 0;

        float currentHeight;

        float currentWidth;

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
        public void AddItem(GameWindow window, Texture2D itemTexture, int State, int type, float Size)
        {
            float X = 0;
            float Y = 0;
            //Define height of item

            //float X = Global.GameWindow.X / 2 / 12 - itemTexture.Width / 2;

            //float Y = 0 + currentHeight / 12;


            if (type == 1)
            {
                Y = Global.GameWindow.Y / 12 - itemTexture.Height;

                X = 0 + currentWidth / 12;

                currentWidth += itemTexture.Width + Size * 12;
            }
            else if (type == 2)
            {
                Y = 0 + currentHeight / 12;

                X = 0 + currentWidth / 12;

                currentHeight += itemTexture.Height + Size * 12;
            }

            //Create temporary object to add to list
            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), State);
            menu.Add(temp);

        }




        //========================================================
        // Update checks for key presses, currently set for W S or 
        // Arrow keys, also checks for confirmation with Enter,
        // Will Add for press with mouse later
        //========================================================
        public int Update(GameTime gameTime, SoundEffect choice, SoundEffect flip)
        {

            float currentTime = Global.Time;
            InputManager.GetState();
            KeyboardState keyBoardState = Keyboard.GetState();
            if (menu.Count > 1)
            {
                if (InputManager.IsPressedOnce(Keys.D) || InputManager.IsPressedOnce(Keys.Right))
                {
                    selected++;
                    flip.Play(volume: Global.AudioScale, pitch: 0.0f, pan: 0.0f);

                    if (selected > menu.Count - 1)
                    {
                        selected = 0;
                    }
                }
                else if (InputManager.IsPressedOnce(Keys.A) || InputManager.IsPressedOnce(Keys.Left))
                {
                    selected--;
                    flip.Play(volume: Global.AudioScale, pitch: 0.0f, pan: 0.0f);

                    if (selected < 0)
                    {
                        selected = menu.Count - 1;

                    }
                }
            }
            





            if (InputManager.IsPressedOnce(Keys.C))
            {
                choice.Play(volume: Global.AudioScale, pitch: 0.0f, pan: 0.0f);
                InputManager.Reset();
                return menu[selected].State;
            }
            else
            {
                return defaultMenuState;
            }

        }

        public int UpdateOptions(GameTime gameTime, SoundEffect choice, SoundEffect flip)
        {
            float currentTime = Global.Time;
            InputManager.GetState();
            KeyboardState keyBoardState = Keyboard.GetState();
            if (menu.Count > 1)
            {
                if (InputManager.IsPressedOnce(Keys.S) || InputManager.IsPressedOnce(Keys.Down))
                {
                    selected++;
                    flip.Play(volume: Global.AudioScale, pitch: 0.0f, pan: 0.0f);

                    if (selected > menu.Count - 1)
                    {
                        selected = 0;
                    }
                }
                else if (InputManager.IsPressedOnce(Keys.W) || InputManager.IsPressedOnce(Keys.Up))
                {
                    selected--;
                    flip.Play(volume: Global.AudioScale, pitch: 0.0f, pan: 0.0f);

                    if (selected < 0)
                    {
                        selected = menu.Count - 1;
                    }
                }
            }

            return menu[selected].State;

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
                    Global.SpriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.Gray);
                }
                else
                {
                    Global.SpriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.White);
                }
            }
        }

        public void setButtonTexture(int  index, Texture2D newTexture)
        {
            if (index >= 0 && index < menu.Count)
            {
                menu[index].SetTexture(newTexture);
            }
        }

    }
}
