using Core.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace TestCore
{
    [TestClass]
    public class CommandManagerTest
    {
        private CommandManager _commandManager;
        private MockRepository _mocks;

        [TestInitialize]
        public void Initialize()
        {
            _commandManager = new CommandManager();
            _mocks = new MockRepository();
        }


        [TestMethod]
        public void TestExecute()
        {
            var command = _mocks.StrictMock<ICommand>();

            Expect.Call(command.Do);
            _mocks.ReplayAll();
            _commandManager.ExecuteCommand(command);
            _mocks.VerifyAll();
        }


        [TestMethod]
        public void TestUndo()
        {
            var command = _mocks.StrictMock<ICommand>();

            Expect.Call(command.Do);
            Expect.Call(command.Undo);
            _mocks.ReplayAll();
            _commandManager.ExecuteCommand(command);
            _commandManager.Undo();

            _mocks.VerifyAll();        
        }

        [TestMethod]
        public void TestRedo()
        {
            var command = _mocks.StrictMock<ICommand>();

            Expect.Call(command.Do);
            Expect.Call(command.Undo);
            Expect.Call(command.Redo);
            _mocks.ReplayAll();
            _commandManager.ExecuteCommand(command);
            _commandManager.Undo();
            _commandManager.Redo();

            _mocks.VerifyAll();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _commandManager.Clean();
        }
    }
}
