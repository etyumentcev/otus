using Spacebattle.Interfaces;

namespace Spacebattle.Commands;

public class Command2 : ICommand
{
    private const string _commandText = "Команда №2";
    private ICommand _next;
    
    public Command2(ICommand nextCommand) => _next = nextCommand;
    
    public async Task Execute()
    {
        await Task.Factory.StartNew(() =>
        {
            Console.WriteLine(_commandText);
        });
        await _next.Execute();
    }
}