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
        private const float speed = 50f;
        public bool isGrounded = false;
        public bool isJumping = false;
        public Player(Texture2D texture, Vector2 position, Vector2 size, Vector2 velocity) : base(texture,position,size,velocity)
        {
        }

        public void Update()
        {
            KeyboardState keyboardstate = iKeyboard.GetState();

            if(iKeyboard.IsKeyDown(Keys.D))
            {
                Velocity.X += 5F * Global.Time;
            }
            else if (iKeyboard.IsKeyDown(Keys.A))
            {
                Velocity.X -= 5F * Global.Time;
            }

            if (iKeyboard.IsKeyDown(Keys.W) && isGrounded == true)
            {
                Velocity.Y = 0;
                Velocity.Y -= 350F * Global.Time;
                isGrounded = false;
            }
            


            // Velocity += InputManager.Direction * 5f * Global.Time;
            Position.X += Velocity.X;
            //Position = Vector2.Clamp(Position,new Vector2(0,0),new Vector2(960,320));
            UpdateHitbox();
        }
    }
}
