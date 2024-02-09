using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Starfall.AnimationManagment;
using Starfall.GameManagment;
using Starfall.InputManagment;
using Starfall.Physics;
using Starfall.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Objects
{
    public class Player : Actor
    {




        //Variables related to player movement
        #region Movement

        //Variables directly associated with movement
        public float speed = 2.5f; //rename to max velocity
        public float maxVel = 2.8f;
        public float accelRate = 0.18f;
        public float deccelRate = 0.1f;
        public float maxAirVel;
        public Vector2 moveDirection = Vector2.Zero;
        public float airMultiplier = 1f;
        public bool isJumping = false;
        public bool isSliding = false;


        

        public float slideSpeed = 2f;
        public float gravity = 15f;
       


        public float coyoteTime = 0.15f;
        public float coyoteTimeCounter;

        public float jumpBufferTime = 0.1f;
        public float jumpBufferTimeCounter;

        //Collision Check variables
        public bool isGrounded = false;
        public bool isPlatformGrounded = false;
        public bool hasjumped = false;
        public float gravityMultiplier = 1.0f;

        public bool touchWallLeft = false;
        public bool touchWallRight = false;
        public bool hasLeftWall = true; // rename for clarity

        float lastPressed = 1;

        //Not sure if these are used at all currently :/
        public bool jumpButtonDown = false;
        public bool normalJump = false;
        public bool apex = false;
        #endregion







        
        
        
        
        
        
        


        #region Animation And Textures
        private readonly AnimationManager am;
        private readonly AnimationManager am2;

        private readonly Texture2D RunRight;
        private readonly Texture2D RunLeft;
        private readonly Texture2D IdleRight;
        private readonly Texture2D IdleLeft;
        private readonly Texture2D FallRight;
        private readonly Texture2D FallLeft;
        private readonly Texture2D ApexFrameRight;
        #endregion





        public Player(Texture2D texture, Vector2 position, Vector2 size, Vector2 velocity) : base(texture,position,size,velocity)
        {

            RunRight = Global.Content.Load<Texture2D>("SpreadSheet-RunAnimation");
            RunLeft = Global.Content.Load<Texture2D>("RunLeft");

            IdleRight = Global.Content.Load<Texture2D>("IdleRight");
            IdleLeft = Global.Content.Load<Texture2D>("IdleLeft");
            FallRight = Global.Content.Load<Texture2D>("FallFrame");
            FallLeft = Global.Content.Load<Texture2D>("FallFrameAlt");
            ApexFrameRight = Global.Content.Load<Texture2D>("ApexFrame");





            am = new(8, 8, new Vector2(33, 44));
            am2 = new(4, 4, new Vector2(15, 40));


        }

        public void Update(SoundEffect effect)
        {

            #region Player Controller



            
            InputHandling(effect);


            




            Position.X += Velocity.X;
            #endregion
        }

        private void InputHandling(SoundEffect effect)
        {
            
            //increase maxacc at top of jumpi jumpi
            //maybe tweak air speed (make slower) 
            //lerping for walljumps
            // ADD THE GOODDAMN DASH
            // edge detection would be good
            // tweak values for acc to make character less stiff

            //Currently working on walljumps :) 

            InputManager.GetState();

            if (isGrounded)
            {
                coyoteTimeCounter = coyoteTime;
                isJumping = false;

            }
            else
            {
                coyoteTimeCounter -= Global.Time;
            }



           
                if (InputManager.IsPressedOnce(Keys.Space))
                {
                    isJumping = true;
                    jumpBufferTimeCounter = jumpBufferTime;

                }
                else
                {

                jumpBufferTimeCounter -= Global.Time;
                }




           // Slide();

            OnJumpPressed(effect);


            //Movement Code responsible for movement in X axis >:)

            if (InputManager.IsPressed(Keys.D) && !InputManager.IsPressed(Keys.A))
            {
                am.Update(1f);
                lastPressed = 1;
                moveDirection.X = 1;
                Move();
            }
            if (InputManager.IsPressed(Keys.A) && !InputManager.IsPressed(Keys.D))
            {
                am.Update(1f);
                lastPressed = -1;
                moveDirection.X = -1;
                Move();
            }
            if (InputManager.IsPressed(Keys.A) && InputManager.IsPressed(Keys.D))
            {
                OnLeftAndRightPreessed();
                am2.Update(10f);
            }
            if (!InputManager.IsPressed(Keys.D) && !InputManager.IsPressed(Keys.A))
            {
                am2.Update(10f);
                moveDirection.X = 0;
                Move();

            }




            /*
            if (InputManager.IsPressed(Keys.D) && !InputManager.IsPressed(Keys.A))
            {
                am.Update(1f);
                lastPressed = 0;
                moveDirection.X = 1;

                Move();
            }
            if (InputManager.IsPressed(Keys.A) && !InputManager.IsPressed(Keys.D))
            {
                am.Update(1);
                lastPressed = 1;
                moveDirection.X = -1;
                Move();
            }

            else if (InputManager.IsPressed(Keys.D) && InputManager.IsPressed(Keys.A))
            {
                OnLeftAndRightPreessed();
                am2.Update(10f);
            }

            if(Keyboard.GetState().IsKeyUp(Keys.D))
            {
               // moveDirection.X = -1;
               // Decelerate();
                

            }
            if (Keyboard.GetState().IsKeyUp(Keys.A))
            {
              //  moveDirection.X = 1;
              //  Decelerate();

            }

            if (Keyboard.GetState().IsKeyUp(Keys.A) && Keyboard.GetState().IsKeyUp(Keys.D))
            {
                am2.Update(10f);
            }



            if (InputManager.IsPressedOnce(Keys.E))
            {
                Dash();
            }
            */



        }
        private void OnJumpPressed(SoundEffect effect)
        {
            
            //=================================================================================//
            //  OnJumpPressed handles actions which takes place when the jump button is pressed//
            //=================================================================================//


            //Checks if player is grounded if true performs a normal jump
            if (coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f )
            {
                isGrounded = false;
                coyoteTimeCounter = 0f;
                jumpBufferTimeCounter = 0f;
                Velocity.Y = 0f;
                Velocity.Y = -300f * Global.Time;
                
                effect.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);

            }
            else if(!isGrounded ) //if player isnt grounded instead checks if a walljump can be performed
            {
                if (touchWallLeft && jumpBufferTimeCounter > 0f)
                {
                    jumpBufferTimeCounter = 0f;
                    effect.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);
                    Velocity.Y = 0;
                    Velocity += new Vector2(350 * Global.Time, -300 * Global.Time);
                }
                else if (touchWallRight && jumpBufferTimeCounter > 0f)
                {
                    jumpBufferTimeCounter = 0f;
                    effect.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);
                    Velocity.Y = 0;
                    Velocity += new Vector2(-350 * Global.Time, -300 * Global.Time);
                }
            }
        }
        private void OnLeftAndRightPreessed()
        {
            Velocity.X *= 0.75f;

            if (Math.Abs(Velocity.X) < 0.01f)
            {
                Velocity.X = 0;
            }
        }

        private void WallJump()
        {
            
        }
        private void Move()
        {
            //=======================================================================//
            // Move() contains most of the logic related to movement in the X axis   //
            // such as acceleration, turning, deceleration                           //
            //=======================================================================//


            //If the player is currently not moving aka if moveDirection vector is zero in x axis, we apply a deceleration
            if (moveDirection.X == 0)
            {
                if (Velocity.X > 0)
                {
                    Velocity.X = Math.Max(0, Velocity.X - deccelRate);
                }
                else if (Velocity.X < 0)
                {
                    Velocity.X = Math.Min(0, Velocity.X + deccelRate);
                }
            }


            //If we are currently moving take a moveDirection value between -1 and 1. -1 left, 1 right and apply acceleration based on current velocity to simulate better movement
            if (moveDirection.X == 1 && Velocity.X >= maxVel)
            {
                Velocity.X = maxVel;
            }
            else if (moveDirection.X == -1 && Velocity.X <= -maxVel)
            {
                Velocity.X = -maxVel;

            }
            else
            {
                //if we arent moving we want to change the velocity of the player while in the air to get a better control scheme trough a airMultiplier.
                if (!isGrounded)
                {
                    float speedDif = moveDirection.X * maxVel - Velocity.X;
                    float movement = speedDif * accelRate * airMultiplier;
                    Velocity.X += movement;
                }
                else // normal acceleration applied based on current velocity
                {
                    float speedDif = moveDirection.X * maxVel - Velocity.X;
                    float movement = speedDif * accelRate;
                    Velocity.X += movement;
                }
            }
            
        }

        private void Dash()
        {

        }

        private void Slide()
        {
            if ((touchWallLeft || touchWallRight) && !isGrounded && Velocity.Y > 0)
            {
                isSliding = true;
                //Velocity.Y = 
            }
            else
            {
                isSliding = false;

            }

        }
        public void Draw()
        {

            if (Velocity.Y >= 6 && !isGrounded)
            {
                if (lastPressed == 1)
                {
                    Global.SpriteBatch.Draw(FallRight, new Vector2(Position.X - 6, Position.Y - 16), null, Color.White);
                }
                else if (lastPressed == -1)
                {
                    Global.SpriteBatch.Draw(FallLeft, new Vector2(Position.X - 6, Position.Y - 16), null, Color.White);
                }


            }
            else if (Velocity.Y > 0 && Velocity.Y < 6)
            {
                Global.SpriteBatch.Draw(ApexFrameRight, new Vector2(Position.X - 7, Position.Y ), null, Color.White);
            }
            else if (InputManager.IsPressed(Keys.D) && !InputManager.IsPressed(Keys.A))
            {
                am.Draw(RunRight, Position, -13, -3);

            }
            else if (InputManager.IsPressed(Keys.A) && !InputManager.IsPressed(Keys.D))
            {

                am.Draw(RunLeft, Position, -5, -3);
            }
            else
            {
                if (lastPressed == 1)
                {
                    am2.Draw(IdleRight, Position, 0, 0);
                }
                else if (lastPressed == -1)
                {
                    am2.Draw(IdleLeft, Position, 0, 0);
                }

            }
        }
    }
}
