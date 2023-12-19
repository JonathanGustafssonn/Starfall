using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Starfall.GameManagment;
using Starfall.InputManagment;
using Starfall.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Objects
{
    public class Player : Actor
    {
        public float speed = 4.5f;
        public bool isGrounded = false;
        public bool touchWallLeft = false;
        public bool touchWallRight = false;
        public float gravity = 15f;
        public bool jumpButtonDown = false;
        public bool normalJump = false;
        public bool isJumping = false;
        public Player(Texture2D texture, Vector2 position, Vector2 size, Vector2 velocity) : base(texture,position,size,velocity)
        {
        }

        public void Update()
        {
            
            #region Player Controller
            gravity = 15f;
            /*
            KeyboardState keyState = Keyboard.GetState();



            if(keyState.IsKeyDown(Keys.D))
            {
                if (Velocity.X >= 4.5f)
                {
                    Velocity.X = 4.5f;
                }
                else
                {
                    Velocity.X += 5F * Global.Time;
                }


            }
            else if (keyState.IsKeyDown(Keys.A))
            {
                if(Velocity.X <= -4.5f)
                {
                    Velocity.X = -4.5f;
                }
                else
                {
                    Velocity.X -= 5F * Global.Time;
                }        
            }
            
            



            if (keyState.IsKeyDown(Keys.Space) && isGrounded == true && !jumpButtonDown)
            {
                isGrounded = false;
                Velocity.Y = -350F * Global.Time;
                jumpButtonDown = true;
                normalJump = true;
            }
            else if (touchWallLeft == true && keyState.IsKeyDown(Keys.W))
            {
                Velocity.Y = 0;
                Velocity.X = 0;
                Velocity += new Vector2(50, -350) * Global.Time;
            }
            else if (touchWallRight == true && keyState.IsKeyDown(Keys.W))
            {
                Velocity.Y = 0;
                Velocity.X = 0;
                Velocity += new Vector2(-50, -350) * Global.Time;
            }
            if(keyState.IsKeyUp(Keys.Space) && normalJump == true)
            {
                jumpButtonDown = false;
                normalJump = false;
            }

            */


            //is pressed doesnt work figure out why :( 

            // Velocity += InputManager.Direction * 5f * Global.Time;
            Position.X += Velocity.X;
            //Position = Vector2.Clamp(Position,new Vector2(0,0),new Vector2(960,320));
            UpdateHitbox();
            #endregion
        }
    }
}
