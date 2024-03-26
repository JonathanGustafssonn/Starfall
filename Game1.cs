using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Starfall.GameManagment;
using System.Collections;

namespace Starfall
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Texture2D rectTexture;
        public static int ScreenHeight;
        public static int ScreenWidth;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GameManager.currentState = GameManager.State.Menu;
            GameManager.Initialize();
            base.Initialize();
            //Add initializeation logic for GameManager
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // texture Rectangle
            rectTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectTexture.SetData(new Color[] { Color.White });
            Global.graphicsDevice = GraphicsDevice;
            Global.Content = Content;
            Global.GameWindow = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            GameManager.LoadContent(Window);

            //Split GameManager.LoadContent into two different functions one for update one for menu, one for menu can stay here but loadcontent
            // for run loop should initialize when play is pressed,  that way we can restart, fix date Friday
            Global.SpriteBatch = _spriteBatch;
        }

        protected override void Update(GameTime gameTime)
        {
            //Add State switching for Menu, Run, Exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                GameManager.shouldExit = true;

            // TODO: Add your update logic here
            if(GameManager.shouldExit) Exit();

            Global.Update(gameTime);
            GameManager.Update(gameTime);
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        { 
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GameManager.Draw();
            base.Draw(gameTime);
        }
    }
}