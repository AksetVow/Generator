using System.Collections.Generic;

namespace Core.Command
{
    public class CommandManager
    {
        //TODO implement cleaning strategy for commands

        private Stack<ICommand> _doneCommands = new Stack<ICommand>();
        private Stack<ICommand> _undoneCommands = new Stack<ICommand>();


        public void ExecuteCommand(ICommand command)
        {
            command.Do();
            _doneCommands.Push(command);
        }

        public void Undo()
        {
            if (_doneCommands.Count > 0)
            {
                var command = _doneCommands.Pop();
                command.Undo();
                _undoneCommands.Push(command);
            }
        }

        public void Redo()
        {
            if (_undoneCommands.Count > 0)
            {
                var command = _undoneCommands.Pop();
                command.Redo();
                _doneCommands.Push(command);
            }
        }

    }
}
