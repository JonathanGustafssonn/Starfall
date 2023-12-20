using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Starfall.GameManagment;
using Starfall.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Starfall.InputManagment
{
    //==================================================================================================================//
    //  InputManager handles all player inputs accordingly the result of said inputs are handled in different classes   //
    //==================================================================================================================//

    public static class InputManager
    {
        

        public static void Update(Player player)
        {
            iKeyboard.GetState();
            float LerpVariabel;


            

            if (iKeyboard.IsPressedOnce(Keys.Space) && player.isGrounded)
            {
                    player.isGrounded = false;
                    player.Velocity.Y = -350f * Global.Time; 
            }
            else if(iKeyboard.IsPressedOnce(Keys.Space) && !player.isGrounded)
            {
                if(player.touchWallLeft)
                {
                    player.Velocity.Y = 0;
                    player.Velocity += new Vector2(150 * Global.Time, -350 * Global.Time);
                }
                else if(player.touchWallRight)
                {
                    player.Velocity.Y = 0;
                    player.Velocity += new Vector2(-150 * Global.Time, -350 * Global.Time);
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

                // Implementation with Lerping (Not sure if Lerping is the way to go or not)
                // 15f borde byttas mot variabel, För Speed som Vector2 (X,Y) för att lättare hantera hastigheter   

                /*
                if(player.Velocity.Y <= 0.5f && player.Velocity.Y >= -05f && !player.isGrounded)
                {
                    LerpVariabel = 25f;
                }
                else LerpVariabel = 15f;

                MathHelper.Lerp(0, -4.5f, -15f * Global.Time);
                */
            }
            
            if (iKeyboard.IsPressed(Keys.D))
            {
                //First implementatition of Movement in the Positive X axis
                if (player.Velocity.X >= player.speed) player.Velocity.X = player.speed;
                else if (player.Velocity.X < 0) player.Velocity.X += 25f * Global.Time;
                else if(player.Velocity.Y <= 0.5f && player.Velocity.Y >= -0.5f && !player.isGrounded) player.Velocity.X += 25f * Global.Time;
                else player.Velocity.X += 15f * Global.Time;

                // Implementation with Lerping (Not sure if Lerping is the way to go or not)
                /*
                if(player.Velocity.Y <= 0.5f && player.Velocity.Y >= -05f && !player.isGrounded)
                {
                    LerpVariabel = 25f;
                }
                else LerpVariabel = 15f;

                MathHelper.Lerp(0, 4.5f, LerpVariabel * Global.Time);
                */
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
