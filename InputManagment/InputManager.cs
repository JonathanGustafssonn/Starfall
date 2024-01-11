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

        public static void Update(Player player, SoundEffect effect)
        {

            iKeyboard.GetState();
            
            if (iKeyboard.IsPressedOnce(Keys.Space) && player.isGrounded)
            {
                    player.isGrounded = false;
                    player.Velocity.Y = -350f * Global.Time; 
                    effect.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);
            }
            else if(iKeyboard.IsPressedOnce(Keys.Space) && !player.isGrounded)
            {
                if(player.touchWallLeft)
                {
                    player.Velocity.Y = 0;
                    player.Velocity += new Vector2(150 * Global.Time, -350 * Global.Time);
                    effect.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);

                }
                else if(player.touchWallRight)
                {
                    player.Velocity.Y = 0;
                    player.Velocity += new Vector2(-150 * Global.Time, -350 * Global.Time);
                    effect.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);

                }
            }



            //Look into implementing lerping for acceleartion later on
            // Should probably check out air speed we want to make the character generally more controllable in the air so should tweak values and remove cap of velocity in air i think.
            // Also add comments explaining everything ffs


            if (iKeyboard.IsPressed(Keys.A))
            {
                // First implementation of Movement in the Negative X axis
               
                
                if (player.Velocity.X <= -player.speed) player.Velocity.X = -player.speed;
                else if (player.Velocity.X > 0) player.Velocity.X -= 25f * Global.Time;
                else if (player.Velocity.Y <= 0.5f && player.Velocity.Y >= -0.5f && !player.isGrounded) player.Velocity.X -= 25f * Global.Time;
                else player.Velocity.X -= 15f * Global.Time;
                


            }

            if (iKeyboard.IsPressed(Keys.D))
            {
                //First implementatition of Movement in the Positive X axis
        
                if (player.Velocity.X >= player.speed) player.Velocity.X = player.speed;
                else if (player.Velocity.X < 0) player.Velocity.X += 25f * Global.Time;
                else if (player.Velocity.Y <= 0.5f && player.Velocity.Y >= -0.5f && !player.isGrounded) player.Velocity.X += 25f * Global.Time;
                else player.Velocity.X += 15f * Global.Time;
                

            }

           



            if(Keyboard.GetState().IsKeyUp(Keys.A) && player.Velocity.X < 0)
            {
                player.Velocity.X = MathHelper.Lerp(player.Velocity.X, 0, 0.1f);
                if(Math.Abs(player.Velocity.X) < 0.01f)
                {
                    player.Velocity.X = 0;
                }
            }
            else if(Keyboard.GetState().IsKeyUp(Keys.D) && player.Velocity.X > 0)
            {
                player.Velocity.X = MathHelper.Lerp(player.Velocity.X, 0, 0.1f);
                if (Math.Abs(player.Velocity.X) < 0.01f)
                {
                    player.Velocity.X = 0;
                }
            }
            

        }

    }
}
 