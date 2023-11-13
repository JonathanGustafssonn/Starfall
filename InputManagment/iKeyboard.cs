using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.InputManagment
{
    //--------------------------------------------------------------------------------------//
    //  iKeyboard is an Improved version of the Keyboard class already present in monogame. //
    //  It contains function for pressing a key once, as well as holding a key down.        //
    //--------------------------------------------------------------------------------------//
    public class iKeyboard
    {
        static KeyboardState currentState;

        static KeyboardState previousState;


        public static KeyboardState GetState()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
            return currentState;
        }

        public static bool IsKeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public static bool PressedOnce(Keys key)
        {
            return currentState.IsKeyDown(key) && !previousState.IsKeyDown(key);
        }
    }
}
