namespace Commands;

public class WriteLineCommand: ICommand
{
    public void Execute()
    {
        Console.WriteLine("Команда вывода строки.");
    }
}

public class GenerateExceptionCommand: ICommand
{
    public void Execute()
    {
        throw new Exception("Тестовое исключение");
    }
}
