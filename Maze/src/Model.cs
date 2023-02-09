﻿using System;

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
        HIGH_SCORES,
        CREDITS
    }
    public const int WIDTH = 1920;
    public const int HEIGHT = 1080;
    public const int MAZE_DIM = 900;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _scoreFont;
    private SpriteFont _mazeFont;
    private Input.KeyboardInput _keyboardInput;
    private Screen display;
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
        display = Screen.NONE;
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
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        if (_maze != null)
        {
            _maze.Update(gameTime);
            if (_maze.WinConditionMet) {
                display = Screen.CREDITS;
                _maze = null;
                _mazeController = null;
            }
        }

        _keyboardInput.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.DarkSlateGray);

        // TODO: Add your drawing code
        _spriteBatch.Begin();

        // Game Title
        _spriteBatch.DrawString(_mazeFont, "MAZE", new Vector2(90, 90), Color.Black);

        // Render Appropriate Screen
        switch (display) {
            case Screen.MAZE: {
                _maze.Draw(_spriteBatch);
                _spriteBatch.DrawString(_scoreFont, "SCORE: 000", new Vector2(90, 180), Color.Black);
                break;
            }
            case Screen.HIGH_SCORES: {
                _spriteBatch.DrawString(_mazeFont, "HIGH SCORES", new Vector2(90, 180), Color.Black);
                break;
            }
            case Screen.CREDITS: {
                _spriteBatch.DrawString(_mazeFont, "YOU WON", new Vector2(90, 180), Color.Black);
                break;
            }
        }

        _spriteBatch.DrawString(_mazeFont, "CONTROLS", new Vector2((WIDTH / 4) * 3 + 10, 90), Color.Black);
        _spriteBatch.DrawString(_scoreFont, "MENU", new Vector2((WIDTH / 4) * 3 + 10, 180), Color.Black);
        _spriteBatch.DrawString(_scoreFont, "F1 - New 5 x 5 Maze", new Vector2((WIDTH / 4) * 3 + 10, 220), Color.Black);
        _spriteBatch.DrawString(_scoreFont, "F2 - New 10x10 Maze", new Vector2((WIDTH / 4) * 3 + 10, 260), Color.Black);
        _spriteBatch.DrawString(_scoreFont, "F3 - New 15x15 Maze", new Vector2((WIDTH / 4) * 3 + 10, 300), Color.Black);
        _spriteBatch.DrawString(_scoreFont, "F4 - New 20x20 Maze", new Vector2((WIDTH / 4) * 3 + 10, 340), Color.Black);
        _spriteBatch.DrawString(_scoreFont, "MOVEMENT", new Vector2((WIDTH / 4) * 3 + 10, 395), Color.Black);
        _spriteBatch.DrawString(_scoreFont, "Arrow Keys", new Vector2((WIDTH / 4) * 3 + 10, 435), Color.Black);

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
        display = Screen.MAZE;
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
        display = Screen.HIGH_SCORES;
    }
    private void displayCredits() {
        display = Screen.CREDITS;
    }
}
