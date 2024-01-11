using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
       // public PlayerState playerState = new PlayerState();
        public float speed = 4.5f;
        public bool isGrounded = false;
        public bool touchWallLeft = false;
        public bool touchWallRight = false;
        public float gravity = 15f;
        public bool jumpButtonDown = false;
        public bool normalJump = false;
        public bool jumpInputBuffer = false;
        public bool isJumping = false;
        public float jumpBufferWindow = 0.2f;
       
        public Player(Texture2D texture, Vector2 position, Vector2 size, Vector2 velocity) : base(texture,position,size,velocity)
        {
          
        }

        public void Update()
        {
            
            #region Player Controller
            
            gravity = 15f;











            Position.X += Velocity.X;
            
            UpdateHitbox();
            #endregion
        }

    }
}
