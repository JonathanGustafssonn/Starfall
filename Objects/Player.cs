﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Starfall.AnimationManagment;
using Starfall.GameManagment;
using Starfall.InputManagment;
using Starfall.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Objects
{
    //==========================================
    // Player, class which contains player logic
    //==========================================
    public class Player : Actor
    {

        //Variables related to player movement
        #region Movement

        //Variables directly associated with movement
        public bool GravityAffectable = true;
        public bool canMove = true;
        public float speed = 2.5f; //rename to max velocity
        public float maxVel = 2.8f;
        public float accelRate = 0.18f;
        public float deccelRate = 0.1f;
        public float maxAirVel;
        public Vector2 moveDirection = Vector2.Zero;
        public float airMultiplier = 1f;
        public bool isJumping = false;
        public bool isSliding = false;
	    public float Score = 0;

        public Vector2 wallJumpVel = new Vector2(350 * Global.Time, -300 * Global.Time);

        

        public float slideSpeed = 2f;
        public float gravity = 15f;

        public bool canDash = true;
        public float dashTime = 0.15f;
        public float dashTimer = 0.2f;
        public Vector2 dashVel = new Vector2(15, 15);

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
        public bool hasLeftWall = true; // rename for clarity, Probably not needed at all

        float lastPressed = 1;
        public bool isDashing = false;

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

        //=====================================================================
        // Player(), a constructor for the player class loading animation files
        //=====================================================================
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

        //=======================================
        // Update(), Update function for player,
        // handles mostly movement realted logic
        //=======================================
        public void Update(SoundEffect effect, GameTime gameTime, SoundEffect dash)
        {
            #region Player Controller

            InputHandling(effect,gameTime,dash);
            Position.X += Velocity.X;

            #endregion
        }


        //==================================
        // InputHandling(), a function which
        // compiles all logic related
        // to movement.
        //==================================
        private void InputHandling(SoundEffect effect,GameTime gameTime, SoundEffect dash)
        {
            
            InputManager.GetState();

            #region JumpLogic

            //Checks if player is grounded or in coyoteTime threshold, if true lets player Jump
            if (isGrounded)
            {
                coyoteTimeCounter = coyoteTime;
                isJumping = false;
                canDash = true;
                //Not Both just one, whichever gives the wanted result
                maxVel = 2.8f;
                accelRate = 0.18f;

            }
            else
            {
                coyoteTimeCounter -= Global.Time;
            }

            //Checks if player presses the jump button while in the air, if it is pressed
            //within a certain threshold before  hitting the ground it automatically jumps when landing

            if (InputManager.IsPressedOnce(Keys.Space))
            {
                isJumping = true;
                jumpBufferTimeCounter = jumpBufferTime;

            }
            else
            {

                jumpBufferTimeCounter -= Global.Time;
            }



            if (canMove)
            {
                
                //Calls OnJumpPressed and performs a Jump
                OnJumpPressed(effect);

                #endregion


                //================================================
                //Movement Code responsible for movement in X axis
                //================================================

                CheckDirection();

                if (InputManager.IsPressed(Keys.D) && !InputManager.IsPressed(Keys.A)) //Checks if D is pressed if true moves to the right and updates animations
                {
                    am.Update(1f);
                    lastPressed = 1;
                    //moveDirection.X = 1;
                    Move();
                }
                if (InputManager.IsPressed(Keys.A) && !InputManager.IsPressed(Keys.D))//Checks if A is pressed if true moves to the left and updates animations
                {
                    am.Update(1f);
                    lastPressed = -1;
                    //moveDirection.X = -1;
                    Move();
                }
                if (InputManager.IsPressed(Keys.A) && InputManager.IsPressed(Keys.D)) //Checks if D and A are pressed if true decelerates to a stop and updates animations 
                {
                    OnLeftAndRightPressed();
                    am2.Update(10f);
                }
                if (!InputManager.IsPressed(Keys.D) && !InputManager.IsPressed(Keys.A)) //Checks if neither D or A are pressed if true decelerates to a stop and updates animations 
                {
                    am2.Update(10f);
                
                    Move();
                }


                //Dash logic currently work in progress
                if (InputManager.IsPressedOnce(Keys.E) && canDash && !isDashing) 
                {
                    dash.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);
                    isDashing = true;
                    canMove = false;
                    GravityAffectable = false;
                }
            }

            if (InputManager.IsPressedOnce(Keys.R))
            {
                canDash = true;
            }

            Dash();
            
            



        }

        //=================================================================================//
        //  OnJumpPressed handles actions which takes place when the jump button is pressed//
        //=================================================================================//
        private void OnJumpPressed(SoundEffect effect)
        {
            //Checks if player is grounded if true performs a normal jump
            if (coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f )
            {
                isGrounded = false;
                coyoteTimeCounter = 0f;
                jumpBufferTimeCounter = 0f;
                Velocity.Y = 0f;
                Velocity.Y = -320f * Global.Time;
                
                effect.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);

            }
            else if(!isGrounded) //if player isnt grounded instead checks if a walljump can be performed
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

        //============================================
        // OnLeftAndRightPressed(), Function which
        // decelaretes player if both keys are pressed
        //============================================
        private void OnLeftAndRightPressed()
        {
            Velocity.X *= 0.75f;

            if (Math.Abs(Velocity.X) < 0.01f)
            {
                Velocity.X = 0;
            }
        }

        //========================================================
        // WallJump(), function called when able
        // to perform a walljump, currently not working as intended
        //========================================================
        private void WallJump(SoundEffect effect)
        {

            if((touchWallRight || touchWallLeft) && !isGrounded &&jumpBufferTimeCounter > 0f)
            {
                jumpBufferTimeCounter = 0f;
                effect.Play(volume: 0.2f, pitch: 0.0f, pan: 0.0f);
                Velocity.Y = 0;
                Velocity += new Vector2(wallJumpVel.X * -lastPressed, wallJumpVel.Y); //Look over lastPressed possibly bug inducing

                //Change either maxVelocity or Acceleration value to allow movement in opposite direction of wall jump to lesser degree, so either
                maxVel = 1.2f;
                accelRate = 0.05f;
                // either of these should work
                // reset to default values upon touching ground aka isGrounded
            }
            
        }

        //=======================================================================//
        // Move() contains most of the logic related to movement in the X axis   //
        // such as acceleration, turning, deceleration                           //
        // MaxVel not working accordingly check why?                             //
        //=======================================================================//
        private void Move()
        {
            
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

            if (moveDirection.X == 1 && Velocity.X >= maxVel && !isDashing)
            {
                Velocity.X = maxVel;
            }
            else if (moveDirection.X == -1 && Velocity.X <= -maxVel && !isDashing)
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

        private void CheckDirection()
        {
            if (InputManager.IsPressed(Keys.D))
            {
                moveDirection.X = 1;
            }
            else if (InputManager.IsPressed(Keys.A))
            {
                moveDirection.X = -1;
            }
            else if (!InputManager.IsPressed(Keys.D) && !InputManager.IsPressed(Keys.A))
            {
                moveDirection.X = 0;
            }
            if (InputManager.IsPressed(Keys.S))
            {
                moveDirection.Y = 1;
            }
            else if (InputManager.IsPressed(Keys.W))
            {
                moveDirection.Y = -1;
            }
            else if (!InputManager.IsPressed(Keys.W) && !InputManager.IsPressed(Keys.S))
            {
                moveDirection.Y = 0;
            }

        }






        //=======================================
        // Dash(), Function responsible for logic
        // performed when doing a dash
        //=======================================
        private void Dash()
        {
            

            if (isDashing)
            {
                
                //Should move in direction of moveDirection
                Vector2 normalizedVector = Vector2.Normalize((moveDirection));

                Velocity = normalizedVector * new Vector2(6f, 6f);
                
                

                dashTimer -= Global.Time;
                if(dashTimer <= 0)
                {
                    isDashing = false;
                    dashTimer = 0.2f;
                    canDash = false;
                    canMove = true;
                    GravityAffectable = true;

                }
            }

        }

        //==================================================
        // Slide(), function responsible for WallSlide logic
        //==================================================
        private void Slide()
        {
            if ((touchWallLeft || touchWallRight) && !isGrounded && Velocity.Y > 0)
            {
                isSliding = true;
            }
            else
            {
                isSliding = false;
            }

        }

        //================================================================
        // Draw(), draw fucntion for player class,
        // mostly contains animation conditions
        //================================================================
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
