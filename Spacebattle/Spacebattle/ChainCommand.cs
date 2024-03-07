using Spacebattle.Interfaces;
using System.Data;

namespace Spacebattle
{
    public class ChainCommand : ICommand
    {
        private readonly Func<Task> _action;
        private ICommand _next;
        private bool _complete;

        public ChainCommand(Func<Task> action, ICommand next)
        {
            _action = action;
            _next = next;
        }

        public void SetNextCommand(ICommand next)
        {
            _next = next;
        }

        public async Task Execute()
        {
            await _action.Invoke();
            _complete = true;
            if (_next != null)
                await _next.Execute();
        }

        public bool Complete => _complete;
    }
}
