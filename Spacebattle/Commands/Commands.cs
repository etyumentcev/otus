namespace Commands;

class Command1 : ICommand
{
    ICommand _next;
    public Command1(ICommand next) => _next = next;
    public async Task Execute()
    {
        await _next.Execute();
        await Printer.PrintCommandNumberAsync(1);
    }
}

class Command2 : ICommand
{
    ICommand _next;
    public Command2(ICommand next) => _next = next;
    public async Task Execute()
    {
        await Printer.PrintCommandNumberAsync(2);
        await _next.Execute();
    }
}

class Command3 : ICommand
{
    ICommand _next;
    public Command3(ICommand next) => _next = next;
    public async Task Execute()
    {
        await Printer.PrintCommandNumberAsync(3);
        await _next.Execute();
    }
}

/// <summary>
/// Команда, завершающая цепочку
/// </summary>
class FinishCommand : ICommand
{
    public async Task Execute()
    {
        // Console.WriteLine("Finish");
    }
}
