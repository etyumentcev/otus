using Spacebattle.Interfaces;

namespace Spacebattle
{
    public class HardStopMainCommand : ICommand
    {
        private readonly CancellationTokenSource _cts;

        public HardStopMainCommand(CancellationTokenSource cts)
        {
            _cts = cts;
        }

        public Task Execute()
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }
    }
}
