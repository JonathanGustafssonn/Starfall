using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Starfall.InputManagment;
using Starfall.Map;
using Starfall.Objects;
using Starfall.Objects.Physical_Objects;
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
        static Texture2D eggTexture;
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
        }
        //=======================================
        // GamePlayLoad(), loads gameplay content
        //=======================================
        public static void GamePlayLoad()
        {
            
            levelManager.Initialize();

            //Code for loading SFX
            effect = Global.Content.Load<SoundEffect>("jump");
            dash = Global.Content.Load<SoundEffect>("dash");
            

            //Code for loading player variables
            player = new(Global.Content.Load<Texture2D>("PlayerSprites/TestPlayer"), new Vector2(86, 167), new Vector2(16, 16), new Vector2(0, 0)); //15 40 //8 12
            player.Hitbox = new Objects.BoundingBox(player.Position.X, player.Position.Y, player.Size.X, player.Size.Y);

            //Code for loading Collision
            collisionHandler = new CollisionHandler();

            //loads level
            levelManager.Load(player);

        }

        //===================================================================
        // GamePlayUpdate(), handles updating values when the game is running
        //===================================================================
        public static State GamePlayUpdate(GameTime gameTime, GameCamera gameCamera, Vector2 Worldsize)
        {

            gameCamera.Update(Worldsize,levelManager.maploader,levelManager.levelindex, levelManager.levels,player);
            //gameCamera.CalculateView(player, Worldsize, levelManager.maploader);
            player.Update(effect, gameTime, dash);
            collisionHandler.CollisionHandling(player, levelManager.boundingBoxes, levelManager.Spikes,
                                                       levelManager.Platform);
            levelManager.Update(player, gameTime);
            //Doesnt restart game just resumes, approach good for in game menu maybe, should look into restarting things though.
            if (InputManager.IsPressed(Keys.L))
            {
                ResetGame();

                return State.Menu;
            }

            if(InputManager.IsPressedOnce(Keys.O))
            {
                levelManager.ChangeLevel(player, gameTime);
                levelManager.Load(player);
            }

            return State.Run;
        }

        //==============================================
        // ResetGame(), function resets the games values
        //==============================================
        public static void ResetGame()
        {
            player.Position = player.spawnPoint;
        }

        //===============================================================================
        // GamePlayDraw(), handles drawing objects to the screen when the game is running
        //===============================================================================
        public static void GamePlayDraw(SpriteFont font)
        {
            levelManager.Draw();
            //Global.SpriteBatch.DrawString(font, "Camera " + player.Velocity.X+ "", player.Position + new Vector2(10, 10), Color.Red);
            // Global.SpriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
            player.Draw();
            // Global.SpriteBatch.Draw(Background, new Vector2(player.Hitbox.X, player.Hitbox.Y) ,null, Color.Green,0f, Vector2.Zero, new Vector2(player.Size.X / Background.Width,player.Size.Y / Background.Height), SpriteEffects.None, 0f);
            //Global.SpriteBatch.Draw(rectTexture, new Rectangle((int)player.Position.X, (int)(player.Position.Y + player.Size.Y), (int)player.Size.X, 6), Color.Red);
            
                Global.SpriteBatch.DrawString(font, "Jump.Y " + player.isJumping + "", player.Position +new Vector2(20, 20), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);


        }

    }
}
