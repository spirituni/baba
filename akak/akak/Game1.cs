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

        private Texture2D _aki;
        private Texture2D _blue;
        private Texture2D _pink;
        private Texture2D _yellow;
        private Texture2D _white;
        private Texture2D _box;
        private Texture2D _circle;
        private Texture2D _dash;

        private char _tile = 'p';
        private Vector2 _cursor = new Vector2(0, 0);
        private const int _w = 10;
        private const int _h = 10;

        private int _delay = 0;
        private bool _place = false;

        private char[,] _level = new char[_h,_w];

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
                    for (int row = 0; row < _h; ++row)
                    {
                        string line = "";
                        for (int col = 0; col < _w; ++col)
                        {
                            line += _level[row, col];
                        }
                        Trace.WriteLine(line);
                        writer.WriteLine(line);
                    }
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
                Trace.WriteLine("Reading to file");
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    int row = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Trace.WriteLine(line);
                        for (int col = 0; col < _w; ++col)
                        {
                            _level[row, col] = line[col];
                        }
                        ++row;
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

            if (_delay++ != 5)
            {
                return;
            }

            // change cursor
            if (key.IsKeyDown(Keys.D1))
            {
                _tile = 'p';
            }  else if (key.IsKeyDown(Keys.D2))
            {
                _tile = 'o';
            } else if (key.IsKeyDown(Keys.D3))
            {
                _tile = 'x';
            } else if (key.IsKeyDown(Keys.D4))
            {
                _tile = '-';
            } else if (key.IsKeyDown(Keys.D5))
            {
                _tile = ' ';
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
            for (int i = 0; i < _h; ++i)
            {
                for (int j = 0; j < _w; ++j)
                {
                    char t = _level[i, j];
                    if (t == ' ')
                    {
                        _spriteBatch.Draw(_white, new Rectangle(i * 50, j * 50, 50, 50), Color.White);
                    } else if (t == 'p')
                    {
                        _spriteBatch.Draw(_aki, new Rectangle(i * 50, j * 50, 50, 50), Color.White);
                    } else if (t == 'o')
                    {
                        _spriteBatch.Draw(_blue, new Rectangle(i * 50, j * 50, 50, 50), Color.White);
                        _spriteBatch.Draw(_circle, new Rectangle(i * 50, j * 50, 50, 50), Color.White);
                    } else if (t == 'x')
                    {
                        _spriteBatch.Draw(_blue, new Rectangle(i * 50, j * 50, 50, 50), Color.White);
                        _spriteBatch.Draw(_box, new Rectangle(i * 50, j * 50, 50, 50), Color.White);
                    } else if (t == '-')
                    {
                        _spriteBatch.Draw(_pink, new Rectangle(i * 50, j * 50, 50, 50), Color.White);
                    }


                }
            }

            // draw cursor
            _spriteBatch.Draw(_yellow, new Rectangle((int)_cursor.X, (int)_cursor.Y, 50, 50), Color.White);

            if (_tile == 'p')
            {
                _spriteBatch.Draw(_aki, new Rectangle((int)_cursor.X, (int)_cursor.Y, 50, 50), Color.White);
            }
            else if (_tile == 'o')
            {
                _spriteBatch.Draw(_box, new Rectangle((int)_cursor.X, (int)_cursor.Y, 50, 50), Color.White);
            }
            else if (_tile == 'x')
            {
                _spriteBatch.Draw(_circle, new Rectangle((int)_cursor.X, (int)_cursor.Y, 50, 50), Color.White);
            }
            else if (_tile == '-')
            {
                _spriteBatch.Draw(_dash, new Rectangle((int)_cursor.X, (int)_cursor.Y, 50, 50), Color.White);
            }

            // place tile at cursor
            if (_place)
            {
                int x = (int)_cursor.X;
                int y = (int)_cursor.Y;
                _spriteBatch.Draw(_pink, new Rectangle(x, y, 50, 50), Color.White);
                _level[x / 50, y / 50] = _tile;
                _place = false;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
