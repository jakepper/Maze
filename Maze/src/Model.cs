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
    private SpriteFont _scoreFont;
    private SpriteFont _mazeFont;
    private Input.KeyboardInput _keyboardInput;
    private Maze _maze;
    private Input.Controller _mazeController;

    public Model()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = WIDTH;
        _graphics.PreferredBackBufferHeight = HEIGHT;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = false;

        _keyboardInput = new();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        // Always Active Inputs

        // New Game Inputs
        _keyboardInput.RegisterCommand(Keys.F1, false, new Input.InputDeviceHelper.CommandDelegate(newGame5));
        _keyboardInput.RegisterCommand(Keys.F2, false, new Input.InputDeviceHelper.CommandDelegate(newGame10));
        _keyboardInput.RegisterCommand(Keys.F3, false, new Input.InputDeviceHelper.CommandDelegate(newGame15));
        _keyboardInput.RegisterCommand(Keys.F4, false, new Input.InputDeviceHelper.CommandDelegate(newGame20));

        // Alternate Display Pages
        _keyboardInput.RegisterCommand(Keys.F5, false, new Input.InputDeviceHelper.CommandDelegate(displayHighScores));
        _keyboardInput.RegisterCommand(Keys.F6, false, new Input.InputDeviceHelper.CommandDelegate(displayCredits));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        _scoreFont = Content.Load<SpriteFont>(@"fonts\Score");
        _mazeFont = Content.Load<SpriteFont>(@"fonts\Maze");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        if (_maze != null) {
            _maze.Update(gameTime);
            if (_maze.WinConditionMet) displayCredits();
        }

        _keyboardInput.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code
        _spriteBatch.Begin();

        _spriteBatch.DrawString(_mazeFont, "MAZE", new Vector2(90, 90), Color.Black);

        if (_maze != null) {
            _maze.Draw(_spriteBatch);
            _spriteBatch.DrawString(_scoreFont, "Score: 000", new Vector2(90, 180), Color.Black);
        }
        // else {
        //     _spriteBatch.DrawString(_scoreFont, "YOU WON", new Vector2(WIDTH / 4, 90), Color.Black);
        // }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void newGame(int size)
    {
        _maze = new Maze(
            this,
            new Vector2(WIDTH / 4, 90),
            MAZE_DIM,
            size
        );
        _maze.Initialize();
        _mazeController = new(_keyboardInput, _maze);

    }
    private void newGame5()
    {
        newGame(5);
    }
    private void newGame10()
    {
        newGame(10);
    }
    private void newGame15()
    {
        newGame(15);
    }
    private void newGame20()
    {
        newGame(20);
    }
    private void displayHighScores()
    {
        _maze = null;
        _mazeController = null;
        Console.WriteLine("[TODO]: Display High Scores");
    }
    private void displayCredits()
    {
        _maze = null;
        _mazeController = null;
        Console.WriteLine("[TODO]: Display Credits");
    }
}
