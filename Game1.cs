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
        private GameManager gameManager;

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

            //Add rest of LoadContent logic for GameManager aka GameManager.LoadContent;
            



            Global.SpriteBatch = _spriteBatch;

        }

        protected override void Update(GameTime gameTime)
        {
            //Add State switching for Menu, Run, Exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Global.Update(gameTime);
            switch (GameManager.currentState)
            {
                case GameManager.State.Run:
                    GameManager.currentState = GameManager.RunUpdate(gameTime);
                    break;
                case GameManager.State.Menu:
                    GameManager.currentState = GameManager.MenuUpdate(gameTime);
                    break;
                case GameManager.State.Quit:
                    this.Exit();
                    break;
                default:
                    GameManager.currentState = GameManager.MenuUpdate(gameTime);
                    break;
            }
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (GameManager.currentState)
            {
                case GameManager.State.Run:
                    GameManager.RunDraw();
                    break;
                case GameManager.State.Menu:
                    GameManager.MenuDraw();
                    break;
                case GameManager.State.Quit:
                    this.Exit();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}