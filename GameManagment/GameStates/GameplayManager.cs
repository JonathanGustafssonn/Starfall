using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Starfall.InputManagment;
using Starfall.Map;
using Starfall.Objects;
using Starfall.Physics;
using Starfall.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using static Starfall.GameManagment.GameManager;

namespace Starfall.GameManagment.GameStates
{
    public class GameplayManager
    {
        //Player variables
        static Player player;

        //SFX (should be initialized in player class change later)
        static SoundEffect effect;
        static SoundEffect dash;

        //CollisionHandler instance handles collision between player and level
        static CollisionHandler collisionHandler;


        //===========================================================
        // GamePlayInitialize(), initializes content for the game map
        //===========================================================
        public static void GamePlayInitialize()
        {
            levelManager.Initialize();
        }
        //=======================================
        // GamePlayLoad(), loads gameplay content
        //=======================================
        public static void GamePlayLoad(GameWindow window)
        {
            //Code for loading SFX
            effect = Global.Content.Load<SoundEffect>("jump");
            dash = Global.Content.Load<SoundEffect>("dash");

            //Code for loading player variables
            player = new(Global.Content.Load<Texture2D>("PlayerSprites/TestPlayer"), new Vector2(100, 150), new Vector2(15, 40), new Vector2(0, 0)); //15 40 //8 12
            player.Hitbox = new Objects.BoundingBox(player.Position.X, player.Position.Y, player.Size.X, player.Size.Y);

            //Code for loading Collision
            collisionHandler = new CollisionHandler();

            //loads level
            levelManager.Load();
        }

        //===================================================================
        // GamePlayUpdate(), handles updating values when the game is running
        //===================================================================
        public static State GamePlayUpdate(GameTime gameTime, GameCamera gameCamera, Vector2 Worldsize)
        {
            gameCamera.CalculateView(player, Worldsize, levelManager.maploader);
            player.Update(effect, gameTime, dash);
            collisionHandler.CollisionHandling(player, levelManager.boundingBoxes, levelManager.Spikes,
                                                       levelManager.Platform, levelManager.Collectibles);

            //Doesnt restart game just resumes, approach good for in game menu maybe, should look into restarting things though.
            if (InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.L))
            {
                ResetGame();
                return State.Menu;
            }

            return State.Run;
        }

        //==============================================
        // ResetGame(), function resets the games values
        //==============================================
        public static void ResetGame()
        {
            player.Position = new Vector2(165, 150);
        }

        //===============================================================================
        // GamePlayDraw(), handles drawing objects to the screen when the game is running
        //===============================================================================
        public static void GamePlayDraw(SpriteFont font)
        {
            levelManager.Draw();
            // Global.SpriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
            player.Draw();
            // Global.SpriteBatch.Draw(Background, new Vector2(player.Hitbox.X, player.Hitbox.Y) ,null, Color.Green,0f, Vector2.Zero, new Vector2(player.Size.X / Background.Width,player.Size.Y / Background.Height), SpriteEffects.None, 0f);
            //Global.SpriteBatch.Draw(rectTexture, new Rectangle((int)player.Position.X, (int)(player.Position.Y + player.Size.Y), (int)player.Size.X, 6), Color.Red);
            foreach (Objects.BoundingBox box in levelManager.boundingBoxes)
            {
                Global.SpriteBatch.DrawString(font, "WallLeft" + player.isDashing + "", player.Position + new Vector2(10, 10), Color.Red);
                Global.SpriteBatch.DrawString(font, "WallRight" + player.touchWallRight + "", player.Position + new Vector2(20, 20), Color.Yellow);
                //Global.SpriteBatch.DrawString(font, "MoveDir " + player.isDashing + "", player.Position + new Vector2(0, 0), Color.Green);
                //Global.SpriteBatch.DrawString(font, "WallLeft" + player.touchWallLeft + "", new Vector2(0, 80), Color.Red);
                //Global.SpriteBatch.DrawString(font, "WallRight" + player.touchWallRight + "", new Vector2(0, 100), Color.Red);
            }
        }

    }
}
