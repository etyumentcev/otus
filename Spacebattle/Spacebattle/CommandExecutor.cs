using Spacebattle.Interfaces;
using System.Collections.Concurrent;

namespace Spacebattle
{
    public class CommandExecutor
    {
        private readonly BlockingCollection<ICommand> _commandQueue;

        public CommandExecutor(BlockingCollection<ICommand> commandQueue)
        {
            _commandQueue = commandQueue;
        }

        public void Execute(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    var command = _commandQueue.Take();
                    command.Execute();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при выполнении команды: {ex.Message}");
                }
            }
        }
    }
}
