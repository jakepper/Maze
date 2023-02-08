using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Maze;

public class Model : Game
{
    public const int WIDTH = 1920;
    public const int HEIGHT = 1080;
    public const int MAZE_DIM = 900;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Maze _maze;
    private Input.Controller _controller;

    public Model()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = WIDTH;
        _graphics.PreferredBackBufferHeight = HEIGHT;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = false;

        _maze = null;
        _controller = new(this, _maze);

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _controller.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _maze.Update(gameTime);
        _controller.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code
        _spriteBatch.Begin();

        if (_maze != null)
        {
            _maze.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
