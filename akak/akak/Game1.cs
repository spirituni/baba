using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System;

namespace akak
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // sprites (images) that can be drawn
        private Texture2D _aki;
        private Texture2D _blue;
        private Texture2D _pink;
        private Texture2D _yellow;
        private Texture2D _white;
        private Texture2D _box;
        private Texture2D _circle;
        private Texture2D _dash;

        private int _delay = 0;                         // counter to decrease movement speed
        private bool _place = false;                    // whether or not to place the tile

        private char _tile = 'p';                       // the tile type currently selected, can be: 'p' for player, 'x' for box, 'o' for hole, '-' for wall, or ' ' for blank
        private Vector2 _cursor = new Vector2(0, 0);    // x and y coordinates of which tile on the map is selected
        private const int _w = 10;                      // expected width of the level in tiles
        private const int _h = 10;                      // expected height of the level in tiles

        private char[,] _level = new char[_h,_w];       // 2d array of characters that represents each tile on the map, with _h rows and _w columns

        private const string fileName = "level.txt";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        private void _saveFile()
        {
            try
            {
                Trace.WriteLine("Writing to file");
                using (StreamWriter writer = new StreamWriter(fileName))
                {

                    // TODO: Save changes made to the level back into the file
                    writer.WriteLine("This string will be saved to the file.");
                }
            } catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            _graphics.PreferredBackBufferWidth = 500;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // read textures
            _aki = this.Content.Load<Texture2D>("aki");
            _blue = this.Content.Load<Texture2D>("blue");
            _pink = this.Content.Load<Texture2D>("pink");
            _yellow = this.Content.Load<Texture2D>("yellow");
            _white = this.Content.Load<Texture2D>("white");
            _box = this.Content.Load<Texture2D>("box");
            _circle = this.Content.Load<Texture2D>("circle");
            _dash = this.Content.Load<Texture2D>("dash");

            // read level file
            try
            {
                Trace.WriteLine("Reading file one line at a time");
                using (StreamReader reader = new StreamReader(fileName))
                {
                    // TODO: Fill the _level array with data from the file "level.txt"
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Trace.WriteLine(line);
                    }
                }
            } catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            var key = Keyboard.GetState();

            // place tile
            if (key.IsKeyDown(Keys.Enter))
            {
                _place = true;
                _saveFile();
            }

            // change cursor
            else if (key.IsKeyDown(Keys.D1))
            {
                _tile = 'p';
            }
            else if (key.IsKeyDown(Keys.D2))
            {
                _tile = 'o';
            }
            else if (key.IsKeyDown(Keys.D3))
            {
                _tile = 'x';
            }
            else if (key.IsKeyDown(Keys.D4))
            {
                _tile = '-';
            }
            else if (key.IsKeyDown(Keys.D5))
            {
                _tile = ' ';
            }


            if (_delay++ != 5)
            {
                return;
            }

            // move cursor
            if (key.IsKeyDown(Keys.W) || key.IsKeyDown(Keys.Up))
            {
                _cursor -= new Vector2(0, 50);
            }
            if (key.IsKeyDown(Keys.A) || key.IsKeyDown(Keys.Left))
            {
                _cursor -= new Vector2(50, 0);
            }
            if (key.IsKeyDown(Keys.S) || key.IsKeyDown(Keys.Down))
            {
                _cursor += new Vector2(0, 50);
            }
            if (key.IsKeyDown(Keys.D) || key.IsKeyDown(Keys.Right))
            {
                _cursor += new Vector2(50, 0);
            }

            // keep cursor in bounds
            if (_cursor.X < 0)
            {
                _cursor.X = 0;
            }
            if (_cursor.Y < 0)
            {
                _cursor.Y = 0;
            }
            if (_cursor.X > 50 * (_w - 1))
            {
                _cursor.X = 50 * (_w - 1);
            }
            if (_cursor.Y > 50 * (_h - 1))
            {
                _cursor.Y = 50 * (_h - 1);
            }

            _delay = 0;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(color: Color.White);

            _spriteBatch.Begin();

            // draw grid
            for (int r = 0; r < _h; ++r)
            {
                for (int c = 0; c < _w; ++c)
                {
                    char t = _level[r, c];
                    if (t == ' ')
                    {
                        _spriteBatch.Draw(_white, new Rectangle(c * 50, r * 50, 50, 50), Color.White);
                    } else if (t == 'p')
                    {
                        _spriteBatch.Draw(_aki, new Rectangle(c * 50, r * 50, 50, 50), Color.White);
                    } else if (t == 'o')
                    {
                        _spriteBatch.Draw(_blue, new Rectangle(c * 50, r * 50, 50, 50), Color.White);
                        _spriteBatch.Draw(_circle, new Rectangle(c * 50, r * 50, 50, 50), Color.White);
                    } else if (t == 'x')
                    {
                        _spriteBatch.Draw(_blue, new Rectangle(c * 50, r * 50, 50, 50), Color.White);
                        _spriteBatch.Draw(_box, new Rectangle(c * 50, r * 50, 50, 50), Color.White);
                    } else if (t == '-')
                    {
                        _spriteBatch.Draw(_pink, new Rectangle(c * 50, r * 50, 50, 50), Color.White);
                    }
                }
            }

            int row = (int)_cursor.Y;
            int col = (int)_cursor.X;

            // draw cursor
            _spriteBatch.Draw(_yellow, new Rectangle(col, row, 50, 50), Color.White);

            if (_tile == 'p')
            {
                _spriteBatch.Draw(_aki, new Rectangle(col, row, 50, 50), Color.White);
            }
            else if (_tile == 'o')
            {
                _spriteBatch.Draw(_circle, new Rectangle(col, row, 50, 50), Color.White);
            }
            else if (_tile == 'x')
            {
                _spriteBatch.Draw(_box, new Rectangle(col, row, 50, 50), Color.White);
            }
            else if (_tile == '-')
            {
                _spriteBatch.Draw(_dash, new Rectangle(col, row, 50, 50), Color.White);
            }

            // place tile at cursor
            if (_place)
            {
                _spriteBatch.Draw(_pink, new Rectangle(col, row, 50, 50), Color.White);
                _level[row / 50, col / 50] = _tile;
                _place = false;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
