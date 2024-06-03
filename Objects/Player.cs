using Microsoft.Xna.Framework;
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
using System.Reflection.Emit;
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

        //Gravity varaibles are fucking around
        //Fix all this code rewrite everything not needed
        // Add double jump and add Glide
        // All mechanincs we need


        //Variables related to player movement
        bool isGlideEnabled = true;

        public Vector2 spawnPoint = Vector2.Zero;

        bool setVelocityY = true;
        public bool isGliding = false;
        bool doubleJumpUp = true;


        public enum PlayerState
        {
            Idle,
            Running,
            Gliding,
            Jumping,
            Flapping,
            Dead,
        }

        public enum JumpState
        {
            Grounded,
            StartJump,
            AccelUp,
            AtTop,
            AccelDown,
            Landing
        }


        private JumpState jumpState;
        private PlayerState playerState;


        #region Movement

        //Variables directly associated with movement
        public float roomnumber = 0;

        public bool GravityAffectable = true;
        public bool canMove = true;
        public float maxVel = 1.3f;
        public float maxVelBase = 1.3f;
        public float accelRate = 0.12f;
        public float deccelRate = 0.1f;
        public float maxAirVel;
        public Vector2 moveDirection = Vector2.Zero;
        public float airMultiplier = 1f;
        public bool isJumping = false;
        public bool isSliding = false;
        public float Score = 0;

        public Vector2 wallJumpVel = new Vector2(350 * Global.Time, -300 * Global.Time);
        public float gravity = 7f;
        public float Initgravity = 7f;



        public float slideSpeed = 2f;


        public bool canDash = true;
        public float dashTimer = 0.3f;
        public Vector2 dashVel = new Vector2(15, 15); // should be used

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

        public float lastPressed = 1;
        public bool isDashing = false;

        //Not sure if these are used at all currently :/
        public bool jumpButtonDown = false;
        public bool normalJump = false;
        public bool apex = false;
        #endregion
        #region Animation And Textures
        private readonly AnimationManager am;
        private readonly AnimationManager am2;
        private readonly AnimationManager am3;


        private bool justLanded = true;


        private readonly Texture2D RunRight;
        private readonly Texture2D RunLeft;
        private readonly Texture2D IdleRight;
        private readonly Texture2D IdleLeft;

        private readonly Texture2D glideRight;
        private readonly Texture2D glideLeft;


        private readonly Texture2D playerRight;
        private readonly Texture2D playerLeft;

        private readonly Texture2D JumpLeft;
        private readonly Texture2D JumpRight;

        private readonly Texture2D startJumpLeft;
        private readonly Texture2D startJumpRight;

        private readonly Texture2D AccelUpRight;
        private readonly Texture2D AccelUpLeft;

        private readonly Texture2D AtTopRight;
        private readonly Texture2D AtTopLeft;

        private readonly Texture2D AccelDownRight;
        private readonly Texture2D AccelDownLeft;

        private readonly Texture2D LandingRight;
        private readonly Texture2D LandingLeft;

        private bool isRunning = false;



        private Texture2D currentTexture;

        #endregion

        //=====================================================================
        // Player(), a constructor for the player class loading animation files
        //=====================================================================
        public Player(Texture2D texture, Vector2 position, Vector2 size, Vector2 velocity) : base(texture, position, size, velocity)
        {
            // load textures for jump individually instead
            // and set current jumpstate to grounded
            // add isjumping bool 
                currentTexture = Global.Content.Load<Texture2D>("PlayerSprites/TestPlayer");

                startJumpRight = Global.Content.Load<Texture2D>("PlayerSprites/TestPlayer");
                startJumpLeft = Global.Content.Load<Texture2D>("PlayerSprites/TestPlayerLeft");

                AccelUpRight = Global.Content.Load<Texture2D>("PlayerSprites/JumpSprites/JumpSpritesRight/AccelUpRight");
                AccelUpLeft = Global.Content.Load<Texture2D>("PlayerSprites/JumpSprites/JumpSpritesLeft/AccelUpLeft");

                AtTopRight = Global.Content.Load<Texture2D>("PlayerSprites/TestPlayer");
                AtTopLeft = Global.Content.Load<Texture2D>("PlayerSprites/TestPlayerLeft");

                AccelDownRight = Global.Content.Load<Texture2D>("PlayerSprites/JumpSprites/JumpSpritesRight/AccelUpRight");
                AccelDownLeft = Global.Content.Load<Texture2D>("PlayerSprites/JumpSprites/JumpSpritesLeft/AccelUpLeft");

                LandingRight = Global.Content.Load<Texture2D>("PlayerSprites/JumpSprites/JumpSpritesRight/LandingRight");
                LandingLeft = Global.Content.Load<Texture2D>("PlayerSprites/JumpSprites/JumpSpritesLeft/LandingLeft");

                 canMove = false;

            


                RunRight = Global.Content.Load<Texture2D>("PlayerSprites/RunSprites/RunRight");
                RunLeft = Global.Content.Load<Texture2D>("PlayerSprites/RunSprites/RunLeft");

                IdleRight = Global.Content.Load<Texture2D>("PlayerSprites/BirbIdleRight");
                IdleLeft = Global.Content.Load<Texture2D>("PlayerSprites/BirbIdle");

                //Replace with glide frames
                glideRight = Global.Content.Load<Texture2D>("PlayerSprites/BirbGlideRight");
                glideLeft = Global.Content.Load<Texture2D>("PlayerSprites/BirbGlideLeft");

                playerRight = Global.Content.Load<Texture2D>("PlayerSprites/TestPlayer");
                playerLeft = Global.Content.Load<Texture2D>("PlayerSprites/TestPlayerLeft");

            


            am = new(8, 8, new Vector2(16, 16));
            am2 = new(2, 2, new Vector2(16, 16));
            am3 = new(8, 8, new Vector2(16, 16));


        }

        //=======================================
        // Update(), Update function for player,
        // handles mostly movement realted logic
        //=======================================
        public void Update(SoundEffect effect, GameTime gameTime, SoundEffect dash)
        {
            if (canMove)
            {
                InputHandling(effect, gameTime, dash);
            }
            //Animations();
            Position.X += Velocity.X;
        }


        //==================================
        // InputHandling(), a function which
        // compiles all logic related
        // to movement.
        //==================================
        private void InputHandling(SoundEffect effect, GameTime gameTime, SoundEffect dash)
        {

            InputManager.GetState();

            
            //Updates timers for Coyote and BufferTime
            CoyoteAndBuffer();

            //Calls OnJumpPressed and performs a Jump
            OnJumpPressed(effect,dash);

            
            
            //Gets current 2D direction Vector
            CheckDirection();


            if (InputManager.IsPressed(Keys.Right) && !InputManager.IsPressed(Keys.Left)) //Checks if D is pressed if true moves to the right and updates animations
            {
                isRunning = true;
                lastPressed = 1;
                am3.Update(1);
                Move();
            }
            if (InputManager.IsPressed(Keys.Left) && !InputManager.IsPressed(Keys.Right))//Checks if A is pressed if true moves to the left and updates animations
            {
                isRunning = true;
                lastPressed = -1;
                am3.Update(1);
                Move();
            }
            if (InputManager.IsPressed(Keys.Left) && InputManager.IsPressed(Keys.Right)) //Checks if D and A are pressed if true decelerates to a stop and updates animations 
            {

                am2.Update(10f);
                OnLeftAndRightPressed();
            }
            if (!InputManager.IsPressed(Keys.Right) && !InputManager.IsPressed(Keys.Left)) //Checks if neither D or A are pressed if true decelerates to a stop and updates animations 
            {
                am2.Update(10f);
                Move();
            }
                Glide(); 
        }


        //===================================================
        // Contains timers for Coyote time and jump buffering
        //===================================================
        private void CoyoteAndBuffer()
        {
            //Checks if player is grounded or in coyoteTime threshold, if true lets player Jump
            if (isGrounded)
            {
                doubleJumpUp = true;
                gravity = Initgravity;
                coyoteTimeCounter = coyoteTime;
                isJumping = false;
                //Not Both just one, whichever gives the wanted result
                maxVel = maxVelBase;
            }
            else
            {
                coyoteTimeCounter -= Global.Time;
            }    
            

        }

        //=================================================================================//
        //  OnJumpPressed handles actions which takes place when the jump button is pressed//
        //=================================================================================//
        private void OnJumpPressed(SoundEffect effect, SoundEffect dash)
        {
            //Checks if player presses the jump button while in the air, if it is pressed
            //within a certain threshold before  hitting the ground it automatically jumps when landing

            if (InputManager.IsPressedOnce(Keys.C))
            {
                isJumping = true;
                jumpBufferTimeCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferTimeCounter -= Global.Time;
            }


            //Checks if player is grounded if true performs a normal jump
            if (coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f )
            {
                isGrounded = false;
                coyoteTimeCounter = 0f;
                jumpBufferTimeCounter = 0f;
                Velocity.Y = 0f;
                Velocity.Y = -175f * Global.Time;
                
                effect.Play(volume: Global.AudioScale, pitch: 0.0f, pan: 0.0f);

            } //current solution binding to different key should be changed entierly in future as to work as a normal 2jump
            else if(doubleJumpUp && InputManager.IsPressedOnce(Keys.X)) //if player isnt grounded instead checks if a DoubleJump can be performed
            {

                    
                    Velocity.Y = 0f;
                    Velocity.Y = -175f * Global.Time;
                    doubleJumpUp = false;

                    dash.Play(volume: Global.AudioScale, pitch: 0.0f, pan: 0.0f);
                
            }
            
        }


        // fix jump interactions
        private void Glide() 
        {
            if (isGlideEnabled && InputManager.IsPressed(Keys.Z) && !isGrounded)
            {
                if (setVelocityY == true)
                {
                    Velocity.Y = 0;
                    setVelocityY = false;
                }

                am.Update(1f);
                isGliding = true;
                if (InputManager.IsPressed(Keys.Down))
                {
                    gravity = Initgravity;
                    Velocity.Y += gravity  * Global.Time;
                }
                else
                {
                    gravity = Initgravity / 12;
                    Velocity.Y += gravity * Global.Time;
                }
            }
            else
            {
                isGliding = false;
                gravity = Initgravity;
                setVelocityY = true;
            }



            if (InputManager.IsPressed(Keys.X) && InputManager.IsPressed(Keys.Z) || InputManager.IsPressed(Keys.C) && InputManager.IsPressed(Keys.Z))
            {
                isGlideEnabled = false;
            }


            if (InputManager.IsReleased(Keys.Z))
            {
                isGlideEnabled = true;
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

        //=======================================================================//
        // Move() contains most of the logic related to movement in the X axis   //
        // such as acceleration, turning, deceleration                           //
        // MaxVel not working accordingly check why?                             //
        //=======================================================================//
        private void Move()
        {
          
            //if moveDirection vector is zero in x axis, we apply a deceleration
            if (moveDirection.X == 0)
            {
                isRunning = false;
                if (Velocity.X > 0)
                {
                    Velocity.X = Math.Max(0, Velocity.X - deccelRate);
                }
                else if (Velocity.X < 0)
                {
                    Velocity.X = Math.Min(0, Velocity.X + deccelRate);
                }
            }

            //If we are currently moving take a moveDirection value between -1 and 1. -1 left, 1 right and apply acceleration based on current velocity.
            //currently a problem, clamping velocity to a maxVel screws up the Dash logic, look for a smoother approach,
            if (moveDirection.X == 1 && Velocity.X >= maxVel) 
            {
                
                Velocity.X = maxVel;
                //Velocity.X = Math.Max(0, Velocity.X - deccelRate);
            }
            else if (moveDirection.X == -1 && Velocity.X <= -maxVel)
            {
                Velocity.X = -maxVel;

                //Velocity.X = Math.Min(0, Velocity.X + deccelRate);
            }
            else //if we are not yet at maxVel
            {
                //we want to change the velocity of the player while in the air to get a better control scheme trough a airMultiplier.
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
            if (InputManager.IsPressed(Keys.Right))
            {
                moveDirection.X = 1;
            }
            else if (InputManager.IsPressed(Keys.Left))
            {
                moveDirection.X = -1;
            }
            else if (!InputManager.IsPressed(Keys.Right) && !InputManager.IsPressed(Keys.Left))
            {
                moveDirection.X = 0;
            }
            if (InputManager.IsPressed(Keys.Down))
            {
                moveDirection.Y = 1;
            }
            else if (InputManager.IsPressed(Keys.Up))
            {
                moveDirection.Y = -1;
            }
            else if (!InputManager.IsPressed(Keys.Up) && !InputManager.IsPressed(Keys.Down))
            {
                moveDirection.Y = 0;
            }

        }
        //================================================================
        // Draw(), draw fucntion for player class,
        // mostly contains animation conditions
        //================================================================


        private void Animations()
        {
            switch (playerState)
            {
                case PlayerState.Idle:
                    if (moveDirection.X == 1)
                    {
                        am2.Draw(IdleRight, Position, 0, 0);
                    }
                    else if (moveDirection.X == -1)
                    {
                        am2.Draw(IdleLeft, Position, 0, 0);
                    }
                    break;

                case PlayerState.Running:
                    if (moveDirection.X == 1)
                    {
                        am2.Draw(IdleRight, Position, 0, 0);
                    }
                    else if (moveDirection.X == -1)
                    {
                        //change to running anims
                        am2.Draw(IdleLeft, Position, 0, 0);
                    }
                    break;

                case PlayerState.Jumping:
                    switch (jumpState)
                    {
                        case JumpState.StartJump:
                            if (moveDirection.X == 1)
                            {
                                currentTexture = startJumpRight;
                            }
                            else if (moveDirection.X == -1)
                            {
                                currentTexture = startJumpLeft;

                            }

                            if (Velocity.Y < -0.2)
                            {
                                jumpState = JumpState.AccelUp;
                            }
                            break;

                        case JumpState.AccelUp:
                            if (moveDirection.X == 1)
                            {
                                currentTexture = startJumpRight;
                            }
                            else if (moveDirection.X == -1)
                            {
                                currentTexture = startJumpLeft;

                            }

                            if (Velocity.Y > -0.5f || Velocity.Y < 0.5f)
                            {
                                jumpState = JumpState.AtTop;
                            }
                            break;

                        case JumpState.AtTop:
                            if (moveDirection.X == 1)
                            {
                                currentTexture = startJumpRight;
                            }
                            else if (moveDirection.X == -1)
                            {
                                currentTexture = startJumpLeft;

                            }

                            if (Velocity.Y > 0.5f)
                            {
                                jumpState = JumpState.AccelDown;
                            }
                            break;

                        case JumpState.AccelDown:
                            if (moveDirection.X == 1)
                            {
                                currentTexture = startJumpRight;
                            }
                            else if (moveDirection.X == -1)
                            {
                                currentTexture = startJumpLeft;

                            }

                            if (justLanded)
                            {
                                jumpState = JumpState.Landing;
                            }
                            break;

                        case JumpState.Landing:
                            justLanded = false;
                            isJumping = false;
                            jumpState = JumpState.Grounded;
                            break;

                    }
                    break;

                case PlayerState.Flapping:
                    break;

                case PlayerState.Gliding:
                    if (moveDirection.X == 1)
                    {
                        am.Draw(glideRight, Position, 0, 0);
                    }
                    else if (moveDirection.X == -1)
                    {
                        am.Draw(IdleLeft, Position, 0, 0);
                    }
                    break;

                case PlayerState.Dead:
                        //am3.Draw(glideRight, Position, 0, 0);
                    break;
            }
        }

        public void Draw()
        {
            if (isJumping && !isGliding)
            {
                if (lastPressed == 1)
                {
                    if (Velocity.Y > -1.2f)
                    {
                        Global.SpriteBatch.Draw(startJumpRight, new Vector2(Position.X, Position.Y), null, Color.White);

                    }
                    else if (Velocity.Y < -1.4f)
                    {
                        Global.SpriteBatch.Draw(AccelUpRight, new Vector2(Position.X, Position.Y), null, Color.White);
                    }
                    else if (Velocity.Y < -0.5f && Velocity.Y < 0.5f)
                    {
                        Global.SpriteBatch.Draw(AtTopRight, new Vector2(Position.X, Position.Y), null, Color.White);
                    }
                    else if (Velocity.Y > 0.5f)
                    {
                        Global.SpriteBatch.Draw(AccelDownRight, new Vector2(Position.X, Position.Y), null, Color.White);

                    }
                }
                else if (lastPressed == -1)
                {
                    if (Velocity.Y > -1.2f)
                    {
                        Global.SpriteBatch.Draw(startJumpLeft, new Vector2(Position.X, Position.Y), null, Color.White);

                    }
                    else if (Velocity.Y < -1.4f)
                    {
                        Global.SpriteBatch.Draw(AccelUpLeft, new Vector2(Position.X, Position.Y), null, Color.White);
                    }
                    else if (Velocity.Y < -0.5f && Velocity.Y < 0.5f)
                    {
                        Global.SpriteBatch.Draw(AtTopLeft, new Vector2(Position.X, Position.Y), null, Color.White);
                    }
                    else if (Velocity.Y > 0.5f)
                    {
                        Global.SpriteBatch.Draw(AccelDownLeft, new Vector2(Position.X, Position.Y), null, Color.White);

                    }
                }
            }
            else if (isRunning && !isGliding)
            {
                if (lastPressed == 1)
                {
                    am3.Draw(RunRight, Position, 0, 0);
                }
                else if (lastPressed == -1)
                {
                    am3.Draw(RunLeft, Position, 0, 0);
                }

            }
            else if (isGliding)
            {
                if (lastPressed == 1)
                {
                    am.Draw(glideRight, Position, 0, 0);
                }
                else if (lastPressed == -1)
                {
                    am.Draw(glideLeft, Position, 0, 0);
                }
            }
            else if (Velocity == new Vector2(0, 0))
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
            else
            {
                if (lastPressed == 1)
                {
                    Global.SpriteBatch.Draw(playerRight, new Vector2(Position.X, Position.Y), null, Color.White);

                }
                else if (lastPressed == -1)
                {
                    Global.SpriteBatch.Draw(playerLeft, new Vector2(Position.X, Position.Y), null, Color.White);

                }
            }

        }
    }
}
