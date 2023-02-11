using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Maze;

public class Model : Game
{
    private enum Screen
    {
        NONE,
        MAZE,
        MAZE_WON,
        HIGH_SCORES,
        CREDITS
    }
    public const int WIDTH = 1920;
    public const int HEIGHT = 1080;
    public const int MAZE_DIM = 900;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _mazeFont;
    private SpriteFont _scoreFont;
    private SpriteFont _headerFont;
    private SpriteFont _textFont;
    private Input.KeyboardInput _keyboardInput;
    private Screen _display;
    private Maze _maze;
    private Input.Controller _mazeController;

    public Model() {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = WIDTH;
        _graphics.PreferredBackBufferHeight = HEIGHT;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _keyboardInput = new();
        _display = Screen.NONE;
    }

    protected override void Initialize() {
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

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        _scoreFont = Content.Load<SpriteFont>(@"fonts\Score");
        _mazeFont = Content.Load<SpriteFont>(@"fonts\Maze");
        _headerFont = Content.Load<SpriteFont>(@"fonts\Header");
        _textFont = Content.Load<SpriteFont>(@"fonts\Text");
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        ProcessInput(gameTime);

        if (_maze != null)
        {
            _maze.Update(gameTime);
            if (_maze.WinConditionMet) {
                _display = Screen.MAZE_WON;
                _maze = null;
                _mazeController = null;
            }
        }

        base.Update(gameTime);
    }

    protected void ProcessInput(GameTime gameTime) {
        _keyboardInput.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.DarkSlateGray);

        // TODO: Add your drawing code
        _spriteBatch.Begin();

        // Game Title
        _spriteBatch.DrawString(_mazeFont, "MAZE", new Vector2(90, 90), Color.Black);

        // Render Appropriate Screen
        switch (_display) {
            case Screen.MAZE: {
                _maze.Draw(_spriteBatch);
                _spriteBatch.DrawString(_scoreFont, $"SCORE: {_maze.score}", new Vector2(90, 270), Color.Black);
                _spriteBatch.DrawString(_scoreFont, $"TIME: {_maze.seconds:0000}", new Vector2(90, 360), Color.Black);
                break;
            }
            case Screen.MAZE_WON: {
                _spriteBatch.DrawString(_headerFont, "YOU WON", new Vector2(90, 180), Color.Black);
                break;
            }
            case Screen.HIGH_SCORES: {
                _spriteBatch.DrawString(_headerFont, "HIGH SCORES", new Vector2(90, 180), Color.Black);
                break;
            }
            case Screen.CREDITS: {
                _spriteBatch.DrawString(_headerFont, "CREDITS", new Vector2(90, 180), Color.Black);
                break;
            }
        }

        // Controls
        float x = (WIDTH / 4) * 3;
        float y = 90;
        _spriteBatch.DrawString(_headerFont, "CONTROLS", new Vector2(x, y), Color.Black);
        _spriteBatch.DrawString(_textFont, "F1 - New 5 x 5 Maze", new Vector2(x, y + 90), Color.Black);
        _spriteBatch.DrawString(_textFont, "F2 - New 10x10 Maze", new Vector2(x, 230), Color.Black);
        _spriteBatch.DrawString(_textFont, "F3 - New 15x15 Maze", new Vector2(x, 280), Color.Black);
        _spriteBatch.DrawString(_textFont, "F4 - New 20x20 Maze", new Vector2(x, 330), Color.Black);
        _spriteBatch.DrawString(_textFont, "F5 - View High Scores", new Vector2(x, 380), Color.Black);
        _spriteBatch.DrawString(_textFont, "Arrows - Movement", new Vector2(x, 430), Color.Black);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void newGame(int size) {
        _maze = new Maze(
            this,
            new Vector2(WIDTH / 4, 90),
            MAZE_DIM,
            size
        );
        _maze.Initialize();
        _mazeController = new(_keyboardInput, _maze);
        _display = Screen.MAZE;
    }
    private void newGame5() {
        newGame(5);
    }
    private void newGame10() {
        newGame(10);
    }
    private void newGame15() {
        newGame(15);
    }
    private void newGame20() {
        newGame(20);
    }
    private void displayHighScores() {
        _display = Screen.HIGH_SCORES;
    }
    private void displayCredits() {
        _display = Screen.CREDITS;
    }
}
