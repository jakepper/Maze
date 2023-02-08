using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Maze.Input
{
    public class Controller
    {
        private Game _game;
        private Maze _maze;
        private KeyboardInput _keyboardInput;
        public Controller(Game game, Maze maze)
        {
            _game = game;
            _maze = maze;
            _keyboardInput = new();
        }

        public void Initialize()
        {
            // New Game Inputs
            _keyboardInput.RegisterCommand(Keys.A, false, new Input.InputDeviceHelper.CommandDelegate(newGame5));
            _keyboardInput.RegisterCommand(Keys.S, false, new Input.InputDeviceHelper.CommandDelegate(newGame10));
            _keyboardInput.RegisterCommand(Keys.D, false, new Input.InputDeviceHelper.CommandDelegate(newGame15));
            _keyboardInput.RegisterCommand(Keys.F, false, new Input.InputDeviceHelper.CommandDelegate(newGame20));

            // Alternate Display Pages
            _keyboardInput.RegisterCommand(Keys.F5, false, new Input.InputDeviceHelper.CommandDelegate(displayHighScores));
            _keyboardInput.RegisterCommand(Keys.F6, false, new Input.InputDeviceHelper.CommandDelegate(displayCredits));

            // Togglers
            _keyboardInput.RegisterCommand(Keys.T, false, new Input.InputDeviceHelper.CommandDelegate(toggleTrail));
            _keyboardInput.RegisterCommand(Keys.P, false, new Input.InputDeviceHelper.CommandDelegate(toggleSolution));
            _keyboardInput.RegisterCommand(Keys.H, false, new Input.InputDeviceHelper.CommandDelegate(toggleHint));

            // Player Controls
            _keyboardInput.RegisterCommand(Keys.Right, true, new Input.InputDeviceHelper.CommandDelegate(moveRight));
            _keyboardInput.RegisterCommand(Keys.Left, true, new Input.InputDeviceHelper.CommandDelegate(moveLeft));
            _keyboardInput.RegisterCommand(Keys.Up, true, new Input.InputDeviceHelper.CommandDelegate(moveUp));
            _keyboardInput.RegisterCommand(Keys.Down, true, new Input.InputDeviceHelper.CommandDelegate(moveDown));

        }

        public void Update(GameTime gameTime)
        {
            _keyboardInput.Update(gameTime);
        }

        private void newGame(int size)
        {
            _maze = new Maze(
                _game,
                new Vector2(Model.WIDTH / 4, 90),
                Model.MAZE_DIM,
                size
            );
            _maze.Initialize();

        }
        private void newGame5(GameTime gameTime)
        {
            newGame(5);
        }
        private void newGame10(GameTime gameTime)
        {
            newGame(10);
        }
        private void newGame15(GameTime gameTime)
        {
            newGame(15);
        }
        private void newGame20(GameTime gameTime)
        {
            newGame(20);
        }
        private void displayHighScores(GameTime gameTime)
        {
            Console.WriteLine("[TODO]: Display High Scores");
        }
        private void displayCredits(GameTime gameTime)
        {
            Console.WriteLine("[TODO]: Display Credits");
        }
        private void toggleSolution(GameTime gameTime)
        {
            if (_maze != null)
            {
                _maze.TogglePath();
            }
        }
        private void toggleHint(GameTime gameTime)
        {
            if (_maze != null)
            {
                _maze.ToggleHint();
            }
        }
        private void toggleTrail(GameTime gameTime)
        {
            if (_maze != null)
            {
                _maze.ToggleTrail();
            }
        }
        private void moveRight(GameTime gameTime)
        {
            if (_maze != null)
            {
                _maze.MoveRight(gameTime);
            }
        }
        private void moveLeft(GameTime gameTime)
        {
            if (_maze != null)
            {
                _maze.MoveLeft(gameTime);
            }
        }
        private void moveUp(GameTime gameTime)
        {
            if (_maze != null)
            {
                _maze.MoveUp(gameTime);
            }
        }
        private void moveDown(GameTime gameTime)
        {
            if (_maze != null)
            {
                _maze.MoveDown(gameTime);
            }
        }
    }
}