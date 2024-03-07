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
            var tasks = new List<Task>
            {
                _action.Invoke(),
            };

            if (_next != null)
                tasks.Add(_next.Execute());

            await Task.WhenAll(tasks);
            _complete = true;
        }

        public bool Complete => _complete;
    }
}
