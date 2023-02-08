using Microsoft.Xna.Framework;

namespace Maze.Input
{
    public class InputDeviceHelper
    {
        public delegate void CommandDelegate(GameTime gameTime);
        public delegate void CommandDelegatePosition(GameTime gameTime, int x, int y);
    }
}