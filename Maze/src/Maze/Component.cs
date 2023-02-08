using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maze {
    public abstract class Component {
        public Game _game;
        public Vector2 Position;

        public Component(Game game, Vector2 position) {
            _game = game;
            Position = position;
        }

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}