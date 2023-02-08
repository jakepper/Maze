using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Maze;

public class Model : Game
{
    private const int WIDTH = 800;
    private const int HEIGHT = 800;

    private const int MAZE_DIM = 600;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Maze _maze;
    private Input.KeyboardInput _keyboardInput;

    public Model()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = WIDTH;
        _graphics.PreferredBackBufferHeight = HEIGHT;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _keyboardInput = new();

        _maze = null;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _keyboardInput.RegisterCommand(Keys.F1, false, new Input.InputDeviceHelper.CommandDelegate(newGame5));
        _keyboardInput.RegisterCommand(Keys.F2, false, new Input.InputDeviceHelper.CommandDelegate(newGame10));
        _keyboardInput.RegisterCommand(Keys.F3, false, new Input.InputDeviceHelper.CommandDelegate(newGame15));
        _keyboardInput.RegisterCommand(Keys.F4, false, new Input.InputDeviceHelper.CommandDelegate(newGame20));

        _keyboardInput.RegisterCommand(Keys.F5, false, new Input.InputDeviceHelper.CommandDelegate(displayHighScores));
        _keyboardInput.RegisterCommand(Keys.F6, false, new Input.InputDeviceHelper.CommandDelegate(displayCredits));

        // _keyboardInput.RegisterCommand(Keys.T, false, new Input.InputDeviceHelper.CommandDelegate(toggleTrail));
        _keyboardInput.RegisterCommand(Keys.P, false, new Input.InputDeviceHelper.CommandDelegate(toggleSolution));
        _keyboardInput.RegisterCommand(Keys.H, false, new Input.InputDeviceHelper.CommandDelegate(toggleHint));
        
        // Player Controls
        _keyboardInput.RegisterCommand(Keys.Right, false, new Input.InputDeviceHelper.CommandDelegate(moveRight));
        _keyboardInput.RegisterCommand(Keys.Left, false, new Input.InputDeviceHelper.CommandDelegate(moveLeft));
        _keyboardInput.RegisterCommand(Keys.Up, false, new Input.InputDeviceHelper.CommandDelegate(moveUp));
        _keyboardInput.RegisterCommand(Keys.Down, false, new Input.InputDeviceHelper.CommandDelegate(moveDown));

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
        _keyboardInput.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code
        _spriteBatch.Begin();
        
        if (_maze != null) {
            _maze.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void newGame(int size) {
        _maze = new Maze(
            this,
            new Vector2(100, 100),
            MAZE_DIM,
            size
        );
        _maze.Initialize();

    }
    private void newGame5(GameTime gameTime) {
        newGame(5);
    }
    private void newGame10(GameTime gameTime) {
        newGame(10);
    }
    private void newGame15(GameTime gameTime) {
        newGame(15);
    }
    private void newGame20(GameTime gameTime) {
        newGame(20);
    }
    private void displayHighScores(GameTime gameTime) {
        Console.WriteLine("[TODO]: Display High Scores");
    }
    private void displayCredits(GameTime gameTime) {
        Console.WriteLine("[TODO]: Display Credits");
    }
    private void toggleSolution(GameTime gameTime) {
        if (_maze != null) {
            _maze.TogglePath();
        }
    }
    private void toggleHint(GameTime gameTime) {
        if (_maze != null) {
            _maze.ToggleHint();
        }
    }
    private void moveRight(GameTime gameTime) {
        if (_maze != null) {
            _maze.MoveRight(gameTime);
        }
    }
    private void moveLeft(GameTime gameTime) {
        if (_maze != null) {
            _maze.MoveLeft(gameTime);
        }
    }
    private void moveUp(GameTime gameTime) {
        if (_maze != null) {
            _maze.MoveUp(gameTime);
        }
    }
    private void moveDown(GameTime gameTime) {
        if (_maze != null) {
            _maze.MoveDown(gameTime);
        }
    }
}
