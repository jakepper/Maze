namespace Maze.Input
{
    public class Controller
    {
        private KeyboardInput _keyboardInput;
        private IControls _entity;
        public Controller(KeyboardInput keyboardInput, IControls entity)
        {
            _keyboardInput = keyboardInput;
            _entity = entity;
            _entity.RegisterControls(keyboardInput);
        }

        ~Controller() {
            _entity.UnregisterControls(_keyboardInput);
        }
    }
}