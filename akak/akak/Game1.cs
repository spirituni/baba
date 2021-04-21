using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace akak
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _aki;
        private Texture2D _blue;
        private Texture2D _pink;
        private Texture2D _yellow;
        private Texture2D _white;

        private const string fileName = "test.txt";
        private List<string> fileData = new List<string>();

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

            _aki = this.Content.Load<Texture2D>("aki");
            _blue = this.Content.Load<Texture2D>("blue");
            _pink = this.Content.Load<Texture2D>("pink");
            _yellow = this.Content.Load<Texture2D>("yellow");
            _yellow = this.Content.Load<Texture2D>("white");

            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(fileName)))
                {
                    string data = reader.ReadLine();
                    fileData.Add(data);
                }
            }
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

            // Mouse.GetState().Position


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(color: Color.White);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_aki, new Rectangle(0, 0, 50, 50), Color.White);
            /*
                        _spriteBatch.Draw(_aki, new Rectangle(0, 0, 50, 50), Color.White);
                        _spriteBatch.Draw(_blue, new Rectangle(0, 0, 50, 50), Color.White);
                        _spriteBatch.Draw(_pink, new Rectangle(0, 0, 50, 50), Color.White);
                        _spriteBatch.Draw(_yellow, new Rectangle(0, 0, 50, 50), Color.White);
            */

            for (int i = 0; i < _w; ++i)
            {
                for (int j = 0; j < _h; ++j)
                {
                    _spriteBatch.Draw(_blue, new Rectangle(i*50, j*50, 50, 50), Color.White);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
