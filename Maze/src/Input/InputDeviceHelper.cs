using Microsoft.Xna.Framework;

namespace Maze.Input
{
    public class InputDeviceHelper
    {
        public delegate void CommandDelegate();
        public delegate void CommandDelegateTime(GameTime gameTime);
        public delegate void CommandDelegatePosition(GameTime gameTime, int x, int y);
    }
}