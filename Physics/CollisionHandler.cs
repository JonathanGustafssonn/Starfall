using Microsoft.Xna.Framework;
using Starfall.GameManagment;
using Starfall.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool Collision(Actor A, Objects.BoundingBox B)
        {
            bool isColliding =
                A.Position.Y + A.Size.Y  >= B.Y &&
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
            for(int i = 0; i < solid.Count; i++)
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
        public void VerticalCollision(Player actor, List<Objects.BoundingBox> solid)
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
        }

        #endregion

        #region Sensors

        public void checkforGround(Player actor, List<Objects.BoundingBox> Solid)
        {
            actor.isGrounded = false;
            for(int i = 0; i < Solid.Count; i++)
            {
                if (tempGroundCheck(actor, Solid[i]))
                {
                    actor.isGrounded = true;
                }
            }
        }

        public bool tempGroundCheck(Player actor, Objects.BoundingBox Solid)
        {
            bool tempGroundCheck =
                actor.Position.Y + actor.Size.Y + 6f >= Solid.Y &&
                actor.Position.Y + actor.Size.Y <= Solid.Y + Solid.Height &&
                actor.Position.X +1f <= Solid.X + Solid.Width &&
                actor.Position.X + actor.Size.X -1f >= Solid.X;

            if (tempGroundCheck)
            {
                return true;
            }
            return false;
        }

        #endregion

        public void Gravity(Player actor)
        {
            
            
                actor.Velocity.Y += 10f * Global.Time;
                
            
            actor.Position.Y += actor.Velocity.Y;

        }

        // CollisionHandling takes the Collision functions and applies them
        public void CollisionHandling(Player actor, List<Objects.BoundingBox> solid)
        {
            checkforGround(actor, solid);
            HorizontalCollision(actor, solid);
            Gravity(actor);
            VerticalCollision(actor, solid);
            
            
        }

    }
}
