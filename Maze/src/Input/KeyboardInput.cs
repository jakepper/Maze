using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Maze.Input
{
    public class KeyboardInput : IInputDevice
    {
        private struct CommandEntry
        {
            public CommandEntry(Keys key, bool keyPressOnly, InputDeviceHelper.CommandDelegate callback)
            {
                this.key = key;
                this.keyPressOnly = keyPressOnly;
                this.callback = callback;
            }
            public Keys key;
            public bool keyPressOnly;
            public InputDeviceHelper.CommandDelegate callback;
        }
        private Dictionary<Keys, CommandEntry> _commandEntries = new();
        private List<CommandEntry> _toRegister = new();
        private List<Keys> _toUnregister = new();
        private KeyboardState _statePrevious;

        private bool keyPressed(KeyboardState state, Keys key)
        {
            return (state.IsKeyDown(key) && !_statePrevious.IsKeyDown(key));
        }

        public bool keyReleased(KeyboardState state, Keys key)
        {
            return (state.IsKeyUp(key) && _statePrevious.IsKeyDown(key));
        }

        public void Update(GameTime gameTime)
        {
            // Unregister Commands
            if (_toUnregister.Count > 0) {
                foreach (Keys key in _toUnregister) {
                    _commandEntries.Remove(key);
                }
                _toUnregister.Clear();
            }

            // Register New Commands
            if (_toRegister.Count > 0) {
                foreach (CommandEntry entry in _toRegister) {
                    _commandEntries.Add(entry.key, entry);
                }
                _toRegister.Clear();
            }

            // Handle Keyboard Input
            KeyboardState state = Keyboard.GetState();
            foreach (CommandEntry entry in _commandEntries.Values)
            {
                if (entry.keyPressOnly && keyPressed(state, entry.key))
                {
                    entry.callback();
                }
                else if (!entry.keyPressOnly && keyReleased(state, entry.key))
                {
                    entry.callback();
                }
            }
            _statePrevious = state;
        }

        public void RegisterCommand(Keys key, bool keyPressOnly, InputDeviceHelper.CommandDelegate callback)
        {
            if (_commandEntries.ContainsKey(key))
            {
                _toUnregister.Add(key);
            }
            _toRegister.Add(new CommandEntry(key, keyPressOnly, callback));
        }

        public void UnregisterCommand(Keys key)
        {
            if (_commandEntries.ContainsKey(key))
            {
                _toUnregister.Add(key);
            }
        }
    }
}