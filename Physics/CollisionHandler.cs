using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Starfall.GameManagment;
using Starfall.InputManagment;
using Starfall.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Physics
{
    //===================================================================
    // CollisionHandler, is the class responsible for handling collisions 
    // between the player and game world. It also contains gravity logic
    //===================================================================
    public class CollisionHandler
    {
        //-------------------------------------------------------------------------------------------------
        // Collision Functions is a region that contains all the separate functions which
        // make up the collision logic of the game The system in place is so called
        // Discrete collision detection algorithm.                     
        //-------------------------------------------------------------------------------------------------
        #region Collision Functions
        public bool areWeInCollision = false;

        //Problem Log issue with resolution of collision because of inconsistent position of Player.hitbox :) hitboxes and sensors shouldnt be made as separate boxes but
        // directly as changes of position and size of the player object.


        public bool PlatformCollision(Actor A, Objects.BoundingBox B)
        {
            bool isColliding =
                A.Position.Y + A.Size.Y >= B.Y &&
                A.Position.Y - 3 <= B.Y + B.Height &&
                A.Position.X + 10 <= B.X + B.Width &&
                A.Position.X + A.Size.X >= B.X;
            if (isColliding)
            {
                return true;
            }
            return false;
        }
        public bool Collision(Actor A, Objects.BoundingBox B)
        {
            bool isColliding =
                A.Position.Y + A.Size.Y >= B.Y &&
                A.Position.Y <= B.Y + B.Height &&
                A.Position.X <= B.X + B.Width &&
                A.Position.X + A.Size.X >= B.X;

            if (isColliding)
            {
                return true;
            }

            return false;
        }
        public void HorizontalCollision(Player actor, List<Objects.BoundingBox> solid)
        {
            for (int i = 0; i < solid.Count; i++)
            {
                areWeInCollision = false;
                if (Collision(actor, solid[i]))
                {
                    areWeInCollision = true;
                    if (actor.Velocity.X > 0)
                    {
                        actor.Velocity.X = 0;
                        actor.Position.X = solid[i].X - actor.Size.X - 0.01f;
                        break;
                    }
                    if (actor.Velocity.X < 0)
                    {
                        actor.Velocity.X = 0;
                        actor.Position.X = solid[i].X + solid[i].Width + 0.01f;
                        break;
                    }
                }
            }
        }
        public void VerticalCollision(Player actor, List<Objects.BoundingBox> solid, List<Objects.BoundingBox> Platform)
        {
            for (int i = 0; i < solid.Count; i++)
            {
                areWeInCollision = false;
                if (Collision(actor, solid[i]))
                {
                    areWeInCollision = true;
                    if (actor.Velocity.Y > 0)
                    {
                        actor.Velocity.Y = 0;
                        actor.Position.Y = solid[i].Y - actor.Size.Y - 0.01f;
                        actor.isJumping = false;
                        break;
                    }
                    if (actor.Velocity.Y < 0)
                    {
                        actor.Velocity.Y = 0;
                        actor.Position.Y = solid[i].Y + solid[i].Height + 0.01f;
                        break;

                    }
                }
            }


            //Platform
            for (int i = 0; i < Platform.Count; i++)
            {
                areWeInCollision = false;
                if (PlatformCollision(actor, Platform[i]))
                {
                    areWeInCollision = true;
                    if (actor.isPlatformGrounded = true && InputManager.IsPressed(Keys.S))
                    {
                        break;
                    }
                    else
                    {
                        if (actor.Velocity.Y > 0)
                        {
                            actor.Velocity.Y = 0;
                            actor.Position.Y = Platform[i].Y - actor.Size.Y - 0.01f;
                            actor.isJumping = false;
                            break;
                        }
                    }

                }
            }
        }


        #endregion

        //-------------------------------------------------------------------------------------
        // Sensors is a region containing sensor functions that determine when collision occurs
        //-------------------------------------------------------------------------------------
        #region Sensors

        public void checkforGround(Player actor, List<Objects.BoundingBox> solid, List<Objects.BoundingBox> Platform)
        {
            actor.isGrounded = false;
            actor.isPlatformGrounded = false;
            for (int i = 0; i < solid.Count; i++)
            {
                if (tempGroundCheck(actor, solid[i]))
                {
                    actor.isGrounded = true;
                    break;
                }
            }

            for (int i = 0; i < Platform.Count; i++)
            {
                if (tempGroundCheck(actor, Platform[i]))
                {
                    actor.isGrounded = true;
                    actor.isPlatformGrounded = true;
                    break;
                }
            }
        }

        public bool tempGroundCheck(Player actor, Objects.BoundingBox Solid)
        {
            bool tempGroundCheck =
                actor.Position.Y + actor.Size.Y + (actor.Size.Y / 10) >= Solid.Y &&
                actor.Position.Y + actor.Size.Y <= Solid.Y + Solid.Height &&
                actor.Position.X + 1f <= Solid.X + Solid.Width &&
                actor.Position.X + actor.Size.X - 1f >= Solid.X;

            if (tempGroundCheck)
            {
                return true;
            }
            return false;
        }

        public bool WallCheckRight(Player actor, Objects.BoundingBox solid)
        {
            bool wallCheckRight =
                actor.Position.Y + 1 + actor.Size.Y - 1 >= solid.Y &&
                actor.Position.Y + 1 <= solid.Y + solid.Height &&
                actor.Position.X + actor.Size.X <= solid.X + solid.Width &&
                actor.Position.X + actor.Size.X + 2f >= solid.X;

            if (wallCheckRight)
            {
                return true;
            }
            return false;
        }

        public bool WallCheckLeft(Player actor, Objects.BoundingBox solid)
        {
            bool wallCheckLeft =
                actor.Position.Y + 1 + actor.Size.Y - 1 >= solid.Y &&
                actor.Position.Y + 1 <= solid.Y + solid.Height &&
                actor.Position.X - 2f <= solid.X + solid.Width &&
                actor.Position.X + 2f >= solid.X;

            if (wallCheckLeft)
            {
                return true;
            }
            return false;

        }

        public void CheckForWalls(Player actor, List<Objects.BoundingBox> Solid)
        {
            actor.touchWallRight = false;
            actor.touchWallLeft = false;

            if (actor.isGrounded == false)
            {
                for (int i = 0; i < Solid.Count; i++)
                {
                    if (WallCheckLeft(actor, Solid[i]))
                    {
                        actor.touchWallLeft = true;
                    }
                    else if (WallCheckRight(actor, Solid[i]))
                    {
                        actor.touchWallRight = true;
                    }
                }
            }

        }

        public void CheckForSpikes(Player actor, List<Objects.BoundingBox> Spike)
        {

            for (int i = 0; i < Spike.Count; i++)
            {
                if (Collision(actor, Spike[i]))
                {
                    //send bool value instead change pos and thingies in player code for later
                    actor.Position = new Vector2(32, 424);
                    
                }
            }
            
        }

        public void CheckForCollectibles(Player actor, List<Objects.BoundingBox> Collectible)
        {
            for (int i = 0; i < Collectible.Count; i++)
            {
                if (Collision(actor, Collectible[i]))
                {
                    actor.Score++;
                }
            }
        }

        #endregion

        //----------------------------------------------------------------
        // Gravity Functions is a region containing gravity related logic
        //----------------------------------------------------------------
        #region Gravity Functions

        //===============================================================
        // Gravity(), is the main function applying gravity to the player
        //===============================================================
        public void Gravity(Player actor)
        {
            if (actor.GravityAffectable)
            {
                if (actor.Velocity.Y !<= 0.2f && actor.Velocity.Y !>= -0.2f )
                {
                    actor.apex = false;
                }
            
                if (actor.Velocity.Y >= 6f)
                {
                    actor.Velocity.Y = 6f;
                }
                else if (actor.Velocity.Y <= 0.2f && actor.Velocity.Y >= -0.2f && !actor.isGrounded && actor.isJumping)
                {
                    JumpApex(actor);
                }
                else if (actor.touchWallLeft && Keyboard.GetState().IsKeyDown(Keys.Left) && actor.Velocity.Y > 0 || actor.touchWallRight && Keyboard.GetState().IsKeyDown(Keys.Right) && actor.Velocity.Y > 0)
                {
                    WallSlideGravity(actor);
                }
                else if (actor.Velocity.Y > 0)
                {
                    actor.maxVel = 3f;
                    HigherDownGravity(actor);
                }
                else
                {
                    actor.maxVel = 3f;

                    UpwardsGravity(actor);
                }


                
            }
            actor.Position.Y += actor.Velocity.Y;
            
        }

        //=================================================
        // JumpApex(), determines the gravity on the player
        // at the apex of the players jump
        //=================================================
        private void JumpApex(Player actor)
        {
            actor.Velocity.Y += actor.gravity / 4f * Global.Time;
            actor.apex = true;
            actor.maxVel = 3.4f;
        }

        //=========================================================
        // WallSlideGravity(), determines the gravity on the player
        // when the player is wallsliding
        //=========================================================
        private void WallSlideGravity(Player actor)
        {
            actor.Velocity.Y += actor.gravity * 0.1f * Global.Time;
        }

        //==============================================================
        // HigherDownGravity(), increases gravity when player is falling
        // to get snappier controls
        //==============================================================
        private void HigherDownGravity(Player actor)
        {
            actor.Velocity.Y += actor.gravity * 1.5f * Global.Time; //check if player is moving downwards and if that is the case make gravity larger else normal gravity 
        }

        //=====================================================================
        // UpwardsGravity(), sets the gravity when the player is moving upwards
        //=====================================================================
        private void UpwardsGravity(Player actor)
        {
            actor.Velocity.Y += actor.gravity  * Global.Time;
        }
        #endregion

        //======================================================================
        // CollisionHandling(), it a function which applies all collision logic,
        // it is called once every gameUpdate
        //======================================================================
        public void CollisionHandling(Player actor, List<Objects.BoundingBox> solid, List<Objects.BoundingBox> Spikes, List<Objects.BoundingBox> Platform, List<Objects.BoundingBox> Collectible)
        {
            HorizontalCollision(actor, solid);
            Gravity(actor);
            VerticalCollision(actor, solid, Platform);
            CheckForSpikes(actor, Spikes);
            CheckForCollectibles(actor, Collectible);
            checkforGround(actor, solid,Platform);
            CheckForWalls(actor, solid);
        }
    }
}
