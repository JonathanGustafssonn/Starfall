using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.InputManagment
{
    public static class InputManager
    {
        private static Vector2 direction;
        public static Vector2 Direction => direction;

        public static void Update()
        {
            KeyboardState keyboardstate = iKeyboard.GetState();

            direction = Vector2.Zero;

            if (keyboardstate.IsKeyDown(Keys.W)) direction.Y--;
            if (keyboardstate.IsKeyDown(Keys.S)) direction.Y++;
            if (keyboardstate.IsKeyDown(Keys.A)) direction.X--;
            if (keyboardstate.IsKeyDown(Keys.D)) direction.X++;
            //if (iKeyboard.PressedOnce(Keys.X)) direction.X++;


            /*
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }
            */
        }

    }
}
