using Microsoft.Xna.Framework;

namespace Maze.Input 
{
    public interface IInputDevice
    {
        void Update (GameTime gameTime);
    }

    public interface IControls
    {
        void RegisterControls(KeyboardInput keyboardInput);
        void UnregisterControls(KeyboardInput keyboardInput);
    }
}