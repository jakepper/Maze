using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maze
{
    public class Cell : Component
    {
        public const int N = 0;
        public const int E = 1;
        public const int S = 2;
        public const int W = 3;
        
        public int X; // grid position (X)
        public int Y; // grid position (Y)
        public int H; // Heuristic cost for A*
        public int G; // Minimum distance from start to cell for A*
        public int FCost { // cost value for A*
            get {
                return G + H;
            }
        }
        public Cell Parent;
        public bool Visited; // Visited marker for Prim's (generation) and A* (pathfinding)
        public bool Frontier; // Frontier marker for Prim's (generation)
        public bool WalkedOn; // Walked on by player
        public bool[] walls;
        public int Width;
        private Texture2D _texture;


        public Cell(Game game, int x, int y, int h, Vector2 position, int width, Texture2D texture) : base(game, position) {
            X = x;
            Y = y;
            walls = new bool[4];

            H = h;

            Width = width;
            _texture = texture;
        }

        public override void Initialize()
        {
            walls[N] = true;
            walls[E] = true;
            walls[S] = true;
            walls[W] = true;

            WalkedOn = false;
            Reset();
        }

        public void Reset() {
            Parent = null;
            G = int.MaxValue;
            Visited = false;
            Frontier = false;
        }

        public override void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (walls[N]) {
                spriteBatch.Draw(
                    _texture, 
                    new Rectangle((int)Position.X, (int)Position.Y, Width, 3), 
                    null, 
                    Color.Black, 
                    0.0f, 
                    Vector2.Zero, 
                    SpriteEffects.None, 
                    0.0f
                );
            }
            if (walls[E]) {
                spriteBatch.Draw(
                    _texture, 
                    new Rectangle((int)Position.X + Width, (int)Position.Y, Width, 3), 
                    null, 
                    Color.Black, 
                    MathHelper.PiOver2, 
                    Vector2.Zero, 
                    SpriteEffects.None, 
                    0.0f
                );
            }
            if (walls[S]) {
                spriteBatch.Draw(
                    _texture, 
                    new Rectangle((int)Position.X, (int)Position.Y + Width, Width, 3), 
                    null, 
                    Color.Black, 
                    0.0f,
                    Vector2.Zero, 
                    SpriteEffects.None, 
                    0.0f
                );
            }
            if (walls[W]) {
                spriteBatch.Draw(
                    _texture, 
                    new Rectangle((int)Position.X, (int)Position.Y, Width, 3), 
                    null, 
                    Color.Black, 
                    MathHelper.PiOver2, 
                    Vector2.Zero, 
                    SpriteEffects.None, 
                    0.0f
                );
            }
        }

        public int Direction(Cell neighbor) {
            int direction = -1;

            int dx = X - neighbor.X;
            int dy = Y - neighbor.Y;

            if (dx > 0) {
                direction = W;
            }
            if (dx < 0) {
                direction = E;
            }
            if (dy > 0) {
                direction = N;
            }
            if (dy < 0) {
                direction = S;
            }

            return direction;
        }

        public static int Opposite(int direction) {
            int opposite;
            switch (direction) {
                case N:
                    opposite = S;
                    break;
                case E: 
                    opposite = W;
                    break;
                case S: 
                    opposite = N;
                    break;
                default: 
                    opposite = E;
                    break;
            }

            return opposite;
        }
    }
}