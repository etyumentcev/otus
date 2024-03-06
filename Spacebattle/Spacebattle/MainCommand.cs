using Spacebattle.Interfaces;

namespace Spacebattle
{
    public class MainCommand : ICommand
    {
        private readonly CommandExecutor _commandExecutor;
        private readonly CancellationToken _cancellationToken;

        public MainCommand(CommandExecutor commandExecutor, CancellationToken cancellationToken)
        {
            _commandExecutor = commandExecutor;
            _cancellationToken = cancellationToken;
        }

        public void Execute() => Task.Run(() => _commandExecutor.Execute(_cancellationToken));

        public int CommandCountInExecutor => _commandExecutor.CommandCountInQueue;
    }
}
