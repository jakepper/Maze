using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maze 
{
    public class Player : Component
    {
        private Texture2D _texture;
        private int Width;
        public int X;
        public int Y;
        public Player(Game game, Vector2 position, int width) : base(game, position) {
            _texture = new Texture2D(game.GraphicsDevice, 1, 1);
            _texture.SetData(new[] { Color.White });
            Width = width;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int x = (int) Position.X + (Width / 2);
            int y = (int) Position.Y + (Width / 2);
            spriteBatch.Draw(_texture, new Rectangle(x, y, Width, Width), Color.Aquamarine);
        }

        public override void Initialize()
        {
            X = 0;
            Y = 0;
        }

        public override void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }

        public void MoveRight(int width) {
            Debug.Write($"({X},{Y}) -> ");
            X++;
            Position.X += width;
            Debug.WriteLine($"({X},{Y})");
        }
        public void MoveLeft(int width) {
            Debug.Write($"({X},{Y}) -> ");
            X--;
            Position.X -= width;
            Debug.WriteLine($"({X},{Y})");
        }
        public void MoveUp(int width) {
            Debug.Write($"({X},{Y}) -> ");
            Y--;
            Position.Y -= width;
            Debug.WriteLine($"({X},{Y})");
        }
        public void MoveDown(int width) {
            Debug.Write($"({X},{Y}) -> ");
            Y++;
            Position.Y += width;
            Debug.WriteLine($"({X},{Y})");
        }
    }
}