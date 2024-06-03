using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Starfall.GameManagment;
using Starfall.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Starfall.InputManagment
{
    //==================================================================================================================//
    //  InputManager handles all player inputs accordingly the result of said inputs are handled in different classes   //
    //==================================================================================================================//

    //Add dashing logic
    //Add Jumpbuffering
    //Add coyoteTime
    //Add squizch beyond wall like celeste // shouldnt be focused
    //State Machine prob not here though :/ // fix immediately
    //edge detection :>) // like? what do i mean edge detection? 
    //Proper animations for each  state
    // Sound effects for running/jumping/turning/dying

    public  class InputManager
    {
        static KeyboardState currentState;

        static KeyboardState previousState;


        public static KeyboardState GetState()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
            return currentState;
        }

        public static bool IsPressed(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public static bool IsPressedOnce(Keys key)
        {
            return currentState.IsKeyDown(key) && !previousState.IsKeyDown(key);
        }

        public static bool IsReleased(Keys key)
        {
            return currentState.IsKeyUp(key) && previousState.IsKeyDown(key);
        }

        public static void Reset()
        {
            previousState = currentState;
        }


    }
}
 