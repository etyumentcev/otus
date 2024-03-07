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

        public async Task Execute(CancellationToken cancellationToken)
        {
            InProgress = true;
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    InProgress = false;
                    break;
                }

                try
                {
                    var command = _commandQueue.Take();
                    await command.Execute();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при выполнении команды: {ex.Message}");
                    if (!_commandQueue.Any())
                        await Task.Delay(1000);
                }
            }
        }

        public int CommandCountInQueue => _commandQueue.Count;

        public bool InProgress { get; private set; }
    }
}
