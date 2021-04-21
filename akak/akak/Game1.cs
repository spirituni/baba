using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace akak
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private char _tile = '-';
        private int _w = 10;
        private int _h = 10;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.P))
            {
                _tile = 'p';
            }  else if (key.IsKeyDown(Keys.O))
            {
                _tile = 'o';
            } else if (key.IsKeyDown(Keys.X))
            {
                _tile = 'x';
            } else if (key.IsKeyDown(Keys.OemMinus))
            {
                _tile = '-';
            }

            //Mouse.GetState().Position


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            /*
            for (int i = 0; i < w; ++i)
            {
                for (int j = 0; j < h; ++j)
                {
                    _spriteBatch.Draw(tileTexture, new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize), color);
                }
            }
            */

            base.Draw(gameTime);
        }
    }
}
