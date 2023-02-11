using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maze.Input;

namespace Maze
{
    public class Maze : Component, IControls
    {
        private Texture2D _texture;
        private List<Cell> Path;
        private bool showPath;
        private bool showHint;
        private bool showTrail;
        public int score;
        public float seconds;
 
        public int Width;
        public int CellWidth;
        public int Size;
        private Cell[,] Grid;
        public Player Player;
        public bool WinConditionMet;

        public Maze(Game game, Vector2 position, int width, int size) : base(game, position)
        {
            Width = width;
            CellWidth = (int)(width / size);

            Size = size;
            Grid = new Cell[size, size];
            Path = new();

            _texture = new(game.GraphicsDevice, 1, 1);
            _texture.SetData(new[] { Color.White });

            Player = new(game, new Vector2(position.X, position.Y), (int)CellWidth / 2);
        }

        public override void Initialize()
        {
            Vector2 cellPos;
            int distance;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    cellPos = new Vector2(i * CellWidth, j * CellWidth); // render position
                    distance = (Size - i) + (Size - j) - 2; // heuristic value for pathfinding
                    Grid[i, j] = new Cell(_game, i, j, distance, Position + cellPos, CellWidth, _texture);
                    Grid[i, j].Initialize();
                }
            }

            Generate();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j].Visited = false;
                }
            }

            FindPath();

            Player.Initialize();

            seconds = 0f;
            score = 0;
        }

        public void RegisterControls(KeyboardInput keyboardInput)
        {
            keyboardInput.RegisterCommand(Keys.B, false, new Input.InputDeviceHelper.CommandDelegate(ToggleTrail));
            keyboardInput.RegisterCommand(Keys.P, false, new Input.InputDeviceHelper.CommandDelegate(TogglePath));
            keyboardInput.RegisterCommand(Keys.H, false, new Input.InputDeviceHelper.CommandDelegate(ToggleHint));
            keyboardInput.RegisterCommand(Keys.Right, true, new Input.InputDeviceHelper.CommandDelegate(MoveRight));
            keyboardInput.RegisterCommand(Keys.Left, true, new Input.InputDeviceHelper.CommandDelegate(MoveLeft));
            keyboardInput.RegisterCommand(Keys.Up, true, new Input.InputDeviceHelper.CommandDelegate(MoveUp));
            keyboardInput.RegisterCommand(Keys.Down, true, new Input.InputDeviceHelper.CommandDelegate(MoveDown));
        }

        public void UnregisterControls(KeyboardInput keyboardInput) {
            keyboardInput.UnregisterCommand(Keys.B);
            keyboardInput.UnregisterCommand(Keys.P);
            keyboardInput.UnregisterCommand(Keys.H);
            keyboardInput.UnregisterCommand(Keys.Right);
            keyboardInput.UnregisterCommand(Keys.Left);
            keyboardInput.UnregisterCommand(Keys.Up);
            keyboardInput.UnregisterCommand(Keys.Down);
        }

        public override void Update(GameTime gameTime)
        {
            seconds += (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (Player.Moved) {
                Player.Moved = false;

                var playerCell = Grid[Player.GridPosition.X, Player.GridPosition.Y];
                if (!playerCell.WalkedOn) {
                    if (Path.Contains(playerCell)) {
                        score += 5;
                    }
                    else {
                        if (score > 0) score--;
                    }
                }
                FindPath();
            }

            if (!Grid[Player.GridPosition.X, Player.GridPosition.Y].WalkedOn)
            {
                Grid[Player.GridPosition.X, Player.GridPosition.Y].WalkedOn = true;
            }

            // Win Condition
            if (Player.GridPosition == (Size - 1, Size - 1)) {
                WinConditionMet = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Width), Color.Crimson);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j].Draw(spriteBatch);
                }
            }

            if (showTrail)
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (Grid[i, j].WalkedOn)
                        {
                            int x = (int)Grid[i, j].Position.X - 2 + Grid[i, j].Width / 2;
                            int y = (int)Grid[i, j].Position.Y - 2 + Grid[i, j].Width / 2;
                            int size = (int)(Width / Size) / 4;
                            spriteBatch.Draw(
                                _texture,
                                new Rectangle(x, y, size, size),
                                null,
                                Color.DimGray,
                                MathHelper.PiOver4,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0.0f
                            );
                        }
                    }
                }
            }

            if (showPath)
            {
                int x, y;
                int size = (int)(Width / Size) / 8;
                foreach (var cell in Path)
                {
                    x = (int)cell.Position.X - 2 + cell.Width / 2;
                    y = (int)cell.Position.Y - 2 + cell.Width / 2;
                    spriteBatch.Draw(
                        _texture,
                        new Rectangle(x, y, size, size),
                        null,
                        Color.WhiteSmoke,
                        MathHelper.PiOver4,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0.0f
                    );
                }
            }

            if (showHint && Path.Count > 1)
            {
                var cell = Path[1];
                int x = (int)cell.Position.X - 2 + cell.Width / 2;
                int y = (int)cell.Position.Y - 2 + cell.Width / 2;
                int size = (int)(Width / Size) / 4;
                spriteBatch.Draw(
                    _texture,
                    new Rectangle(x, y, size, size),
                    null,
                    Color.WhiteSmoke,
                    MathHelper.PiOver4,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0.0f
                );
            }

            Player.Draw(spriteBatch);
        }

        public void TogglePath()
        {
            showPath = !showPath;
            // if (showPath) FindPath();
        }
        public void ToggleHint()
        {
            showHint = !showHint;
            // if (showHint) FindPath();
        }
        public void ToggleTrail()
        {
            showTrail = !showTrail;
        }

        /// ----- Maze Generation -----
        /// Prim's Algorithm for Maze Generation
        private void Generate()
        {
            var rand = new Random();
            var frontier = new List<Cell>();

            // Randomly mark an initial cell as visited
            Mark(rand.Next(Size), rand.Next(Size), frontier);

            while (frontier.Count != 0)
            {
                // select random cell from frontier
                var index = rand.Next(frontier.Count);
                var cell = frontier[index];
                frontier.RemoveAt(index);

                var neighbors = Neighbors(cell.X, cell.Y); // get list of neighbors that have been visited
                var neighbor = neighbors[rand.Next(neighbors.Count)]; // select random neighbor

                // get direction from cell to neighbor and vice versa
                int direction = cell.Direction(neighbor);
                int opposite = Cell.Opposite(direction);

                // change cell edge values appropriately
                cell.walls[direction] = false;
                neighbor.walls[opposite] = false;

                // mark neighbor as visited
                Mark(cell.X, cell.Y, frontier);
            }
        }

        /// ----- Maze Generation -----
        /// Mark Cell as 'Visited'
        /// Add neighboring cells to frontier
        private void Mark(int x, int y, List<Cell> frontier)
        {
            Grid[x, y].Visited = true;
            Debug.WriteLine($"[DEBUG]: Marked cell ({x},{y})");
            AddFrontier(x + 1, y, frontier);
            AddFrontier(x - 1, y, frontier);
            AddFrontier(x, y + 1, frontier);
            AddFrontier(x, y - 1, frontier);
        }

        /// ----- Maze Generation -----
        /// Add Cell to Frontier if not 'Visited' or already part of frontier
        private void AddFrontier(int x, int y, List<Cell> frontier)
        {
            if (x >= 0 && y >= 0 && x < Size && y < Size)
            {
                if (!Grid[x, y].Visited && !Grid[x, y].Frontier)
                {
                    Grid[x, y].Frontier = true;
                    frontier.Add(Grid[x, y]);
                }
            }
        }

        /// ----- Maze Generation -----
        /// Returns a List of the neighboring Cell's that have been marked as 'Visited'
        private List<Cell> Neighbors(int x, int y)
        {
            var n = new List<Cell>();

            if (x > 0)
            {
                if (Grid[x - 1, y].Visited) n.Add(Grid[x - 1, y]);
            }
            if (x + 1 < Size)
            {
                if (Grid[x + 1, y].Visited) n.Add(Grid[x + 1, y]);
            }
            if (y > 0)
            {
                if (Grid[x, y - 1].Visited) n.Add(Grid[x, y - 1]);
            }
            if (y + 1 < Size)
            {
                if (Grid[x, y + 1].Visited) n.Add(Grid[x, y + 1]);
            }

            return n;
        }

        /// ----- Path Finding -----
        /// A* Pathfinding Algorithm
        private void FindPath()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j].Reset();
                }
            }

            var start = Grid[Player.GridPosition.X, Player.GridPosition.Y];
            var end = Grid[Size - 1, Size - 1];

            var closed = new List<Cell>();
            var open = new List<Cell>();

            start.G = 0;
            open.Add(start);

            while (open.Count != 0)
            {
                open.Sort((x, y) => x.FCost.CompareTo(y.FCost));
                var cur = open[0];
                open.RemoveAt(0);
                closed.Add(cur);

                var adjacent = Walkable(cur.X, cur.Y);
                for (int i = 0; i < adjacent.Count; i++)
                {
                    var next = adjacent[i];
                    if (closed.Contains(next))
                    {
                        continue;
                    }
                    int newGCost = cur.G + 1;
                    if (next.FCost == int.MaxValue || newGCost < next.G)
                    {
                        next.G = newGCost;
                        next.Parent = cur;

                        if (!open.Contains(next)) open.Add(next);
                    }
                }

                if (cur == end)
                {
                    RetracePath(start, end);
                    return;
                }
            }
        }

        /// ----- Path Finding -----
        private void RetracePath(Cell start, Cell end)
        {
            Path.Clear();

            var cur = end;
            while (cur != start)
            {
                Path.Add(cur);
                cur = cur.Parent;
            }
            Path.Add(start);
            Path.Reverse();
        }

        /// ----- Path Finding -----
        private List<Cell> Walkable(int x, int y)
        {
            var n = new List<Cell>();

            if (!Grid[x, y].walls[Cell.W]) n.Add(Grid[x - 1, y]);
            if (!Grid[x, y].walls[Cell.E]) n.Add(Grid[x + 1, y]);
            if (!Grid[x, y].walls[Cell.N]) n.Add(Grid[x, y - 1]);
            if (!Grid[x, y].walls[Cell.S]) n.Add(Grid[x, y + 1]);

            return n;
        }

        /// ----- Controls -----
        public void MoveRight()
        {
            if (Player.GridPosition.X < Size - 1 && !Grid[Player.GridPosition.X, Player.GridPosition.Y].walls[Cell.E])
            {
                Player.MoveRight(CellWidth);
            }
        }
        public void MoveLeft()
        {
            if (Player.GridPosition.X > 0 && !Grid[Player.GridPosition.X, Player.GridPosition.Y].walls[Cell.W])
            {
                Player.MoveLeft(CellWidth);
            }
        }
        public void MoveUp()
        {
            if (Player.GridPosition.Y > 0 && !Grid[Player.GridPosition.X, Player.GridPosition.Y].walls[Cell.N])
            {
                Player.MoveUp(CellWidth);
            }
        }
        public void MoveDown()
        {
            if (Player.GridPosition.Y < Size - 1 && !Grid[Player.GridPosition.X, Player.GridPosition.Y].walls[Cell.S])
            {
                Player.MoveDown(CellWidth);
            }
        }
    }
}