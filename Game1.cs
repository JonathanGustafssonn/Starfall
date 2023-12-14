using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Starfall.GameManagment;

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

            //_graphics.PreferredBackBufferWidth = 640;
            //_graphics.PreferredBackBufferHeight = 360;
            _graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
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
            gameManager = new GameManager();
            



            Global.SpriteBatch = _spriteBatch;
            // TODO: use this.Content to load your game content here

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Global.Update(gameTime);
            gameManager.Update();
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            gameManager.Draw(rectTexture);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}