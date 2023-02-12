using System.Collections.Generic;

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
    private Texture2D _mazeTexture;
    private Input.KeyboardInput _keyboardInput;
    private Screen _display;
    private Maze _maze;
    private Dictionary<int, int[]> _scores; // top 5 scores for each maze size
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

        _scores = new();
    }

    protected override void Initialize() {
        // TODO: Add your initialization logic here

        // New Game Inputs
        _keyboardInput.RegisterCommand(Keys.F1, false, new Input.InputDeviceHelper.CommandDelegate(newGame5));
        _keyboardInput.RegisterCommand(Keys.F2, false, new Input.InputDeviceHelper.CommandDelegate(newGame10));
        _keyboardInput.RegisterCommand(Keys.F3, false, new Input.InputDeviceHelper.CommandDelegate(newGame15));
        _keyboardInput.RegisterCommand(Keys.F4, false, new Input.InputDeviceHelper.CommandDelegate(newGame20));

        // Alternate Display Pages
        _keyboardInput.RegisterCommand(Keys.F5, false, new Input.InputDeviceHelper.CommandDelegate(displayHighScores));
        _keyboardInput.RegisterCommand(Keys.F6, false, new Input.InputDeviceHelper.CommandDelegate(displayCredits));

        _scores.Add(5, new int[5]);
        _scores.Add(10, new int[5]);
        _scores.Add(15, new int[5]);
        _scores.Add(20, new int[5]);

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        _scoreFont = Content.Load<SpriteFont>(@"fonts\Score");
        _mazeFont = Content.Load<SpriteFont>(@"fonts\Maze");
        _headerFont = Content.Load<SpriteFont>(@"fonts\Header");
        _textFont = Content.Load<SpriteFont>(@"fonts\Text");

        _mazeTexture = Content.Load<Texture2D>(@"textures\waterfall");
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        ProcessInput(gameTime);

        if (_maze != null)
        {
            _maze.Update(gameTime);
            if (_maze.WinConditionMet)
            {
                int index = -1;
                for (int i = 0; i < 5; i++)
                {
                    if (_maze.Score > _scores[_maze.Size][i])
                    {
                        index = i;
                        break;
                    }
                }
                if (index != -1) { 
                    for (int i = 4; i > index; i--)
                    {
                        _scores[_maze.Size][i] = _scores[_maze.Size][i - 1];
                    }
                    _scores[_maze.Size][index] = _maze.Score;
                }
                _maze = null;
                _mazeController = null;
                _display = Screen.MAZE_WON;
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
        string text = "M";
        Vector2 textMiddlePoint = _mazeFont.MeasureString(text) / 2;
        Vector2 position = new Vector2(430 / 2 + 40, 180);
        _spriteBatch.DrawString(_mazeFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
        text = "A";
        position.Y += 180;
        _spriteBatch.DrawString(_mazeFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
        text = "Z";
        position.Y += 180;
        _spriteBatch.DrawString(_mazeFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
        text = "E";
        position.Y += 180;
        _spriteBatch.DrawString(_mazeFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);

        // Render Appropriate Screen
        switch (_display) {
            case Screen.MAZE: {
                _maze.Draw(_spriteBatch);
                text = $"SCORE: {_maze.Score}";
                textMiddlePoint = _scoreFont.MeasureString(text) / 2;
                position.Y += 90;
                _spriteBatch.DrawString(_scoreFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                text = $"TIME: {_maze.Seconds:0000}";
                textMiddlePoint = _scoreFont.MeasureString(text) / 2;
                position.Y += 90;
                _spriteBatch.DrawString(_scoreFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                break;
            }
            case Screen.MAZE_WON: {
                    text = "YOU WON!";
                    textMiddlePoint = _mazeFont.MeasureString(text) / 2;
                    position = new Vector2(WIDTH / 2, HEIGHT / 2);
                    _spriteBatch.DrawString(_mazeFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                break;
            }
            case Screen.HIGH_SCORES: {
                text = "HIGH SCORES";
                textMiddlePoint = _headerFont.MeasureString(text) / 2;
                position = new Vector2(WIDTH / 2, 180);
                _spriteBatch.DrawString(_headerFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                text = $"{"5x5",9}{"10x10",9}{"15x15",9}{"20x20",9}";
                textMiddlePoint = _scoreFont.MeasureString(text) / 2;
                position.Y += 180;
                _spriteBatch.DrawString(_scoreFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.0f);
                for (int i = 0; i < 5; i++)
                {
                    text = $"{_scores[5][i],9:000}{_scores[10][i],9:000}{_scores[15][i],9:000}{_scores[20][i],9:000}";
                    position.Y += 90;
                    _spriteBatch.DrawString(_scoreFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.0f);
                }
                break;
            }
            case Screen.CREDITS: {
                text = "Credits";
                textMiddlePoint = _headerFont.MeasureString(text) / 2;
                position = new Vector2(WIDTH / 2, 180);
                _spriteBatch.DrawString(_headerFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                text = "Developed By : Jake Epperson";
                textMiddlePoint = _textFont.MeasureString(text) / 2;
                position.Y += 180;
                _spriteBatch.DrawString(_textFont, text, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                break;
            }
        }

        // Controls
        float x = (WIDTH / 4) * 3 + 40;
        float y = 90;
        _spriteBatch.DrawString(_headerFont, "CONTROLS", new Vector2(x, y), Color.White);
        x += 40;
        y += 50;
        _spriteBatch.DrawString(_textFont, "F1 - New 5 x 5 Maze", new Vector2(x, y + 40), Color.White);
        _spriteBatch.DrawString(_textFont, "F2 - New 10x10 Maze", new Vector2(x, y + 100), Color.White);
        _spriteBatch.DrawString(_textFont, "F3 - New 15x15 Maze", new Vector2(x, y + 160), Color.White);
        _spriteBatch.DrawString(_textFont, "F4 - New 20x20 Maze", new Vector2(x, y + 220), Color.White);
        _spriteBatch.DrawString(_textFont, "F5 - View High Scores", new Vector2(x, y + 280), Color.White);
        _spriteBatch.DrawString(_textFont, "F6 - View Credits", new Vector2(x, y + 340), Color.White);
        _spriteBatch.DrawString(_textFont, "B - Show Breadcrumbs", new Vector2(x, y + 400), Color.White);
        _spriteBatch.DrawString(_textFont, "H - Show Hint", new Vector2(x, y + 460), Color.White);
        _spriteBatch.DrawString(_textFont, "P - Show Solution", new Vector2(x, y + 520), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void newGame(int size) {
        _maze = new Maze(
            this,
            new Vector2(WIDTH / 2 - 450, 80),
            MAZE_DIM,
            size,
            _mazeTexture
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
