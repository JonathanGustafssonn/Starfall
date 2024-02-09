using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Starfall.GameManagment;
using Starfall.InputManagment;
using Starfall.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Physics
{
    public class CollisionHandler
    {
        #region Collision Functions
        public bool areWeInCollision = false;

        //-------------------------------------------------------------------------------------------------//
        // This Region contains all the separate functions which make up the collision logic of the game.  //
        // The system in place is so called Discrete collision detection algorithm.                        //
        //-------------------------------------------------------------------------------------------------//

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
                    actor.Position = new Vector2(32, 424);
                }
            }
            
        }

        public void CheckForCollectibles(Player actor, List<Objects.BoundingBox> Collectible)
        {
            if(collision(actor,Collectible[i]))
            {
                Score++;
            }
        }

        #endregion

        #region Gravity Functions
        public void Gravity(Player actor)
        {

            if (actor.Velocity.Y >= 6f)
            {
                actor.Velocity.Y = 6f;
            }
            else if (actor.Velocity.Y <= 0.3f && actor.Velocity.Y >= -0.3f && !actor.isGrounded && actor.isJumping)
            {
                JumpApex(actor);
            }
            else if (actor.touchWallLeft && Keyboard.GetState().IsKeyDown(Keys.A) && actor.Velocity.Y > 0 || actor.touchWallRight && Keyboard.GetState().IsKeyDown(Keys.D) && actor.Velocity.Y > 0)
            {
                WallSlideGravity(actor);
            }
            else if (actor.Velocity.Y > 0)
            {
                actor.maxVel = 2.8f;
                HigherDownGravity(actor);
            }
            else
            {
                actor.maxVel = 2.8f;

                UpwardsGravity(actor);
            }


            actor.Position.Y += actor.Velocity.Y;

        }

    
        private void JumpApex(Player actor)
        {
            actor.Velocity.Y += actor.gravity / 2 * Global.Time;
            actor.apex = true;
            actor.maxVel = 4;
            
        }

        private void WallSlideGravity(Player actor)
        {
            actor.Velocity.Y = actor.gravity * Global.Time;
        }

        private void HigherDownGravity(Player actor)
        {
            actor.Velocity.Y += actor.gravity * 1.5f * Global.Time; //check if player is moving downwards and if that is the case make gravity larger else normal gravity 
        }

        private void UpwardsGravity(Player actor)
        {
            actor.Velocity.Y += actor.gravity  * Global.Time;
        }
        #endregion

        // CollisionHandling takes the Collision functions and applies them
        public void CollisionHandling(Player actor, List<Objects.BoundingBox> solid, List<Objects.BoundingBox> Spikes, List<Objects.BoundingBox> Platform)
        {
            
            HorizontalCollision(actor, solid);
            Gravity(actor);
            VerticalCollision(actor, solid, Platform);
            CheckForSpikes(actor, Spikes);
            CheckForCollectibles(actor, Collectibles);
            checkforGround(actor, solid,Platform);
            CheckForWalls(actor, solid);

        }
    
    }
}
