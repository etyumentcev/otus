using Spacebattle.Interfaces;

namespace Spacebattle
{
    public class StopMainCommand : ICommand
    {
        private readonly CancellationToken _stopMainCommandToken;

        public StopMainCommand(CancellationToken stopMainCommandToken)
        {
            _stopMainCommandToken = stopMainCommandToken;
        }

        public void Execute() => _stopMainCommandToken.ThrowIfCancellationRequested();
    }
}
