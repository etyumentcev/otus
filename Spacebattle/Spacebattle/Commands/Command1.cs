using Spacebattle.Interfaces;

namespace Spacebattle.Commands;

public class Command1 : ICommand
{
    private const string _commandText = "Команда №1";
    private ICommand _next;
    
    public Command1(ICommand nextCommand) => _next = nextCommand;
    
    public async Task Execute()
    {
        await _next.Execute();
        await Task.Factory.StartNew(() =>
        {
            Console.WriteLine(_commandText);
        });
    }
}