using System.Collections.Concurrent;

namespace Commands;

class Program
{
    static void Main(string[] args)
    {
        var runCommand = new RunWorkerCommand();
        runCommand.Execute();
        
        //  Вывод двух строк "Команда вывода строки".
        // RunCommand.Worker.AddCommandToQueue(new WriteLineCommand());
        // RunCommand.Worker.AddCommandToQueue(new SoftStopCommand(RunCommand.Worker));
        // RunCommand.Worker.AddCommandToQueue(new WriteLineCommand());
        
        
        //  Вывод одной строки "Команда вывода строки".
        runCommand.Worker.AddCommandToQueue(new WriteLineCommand());
        runCommand.Worker.AddCommandToQueue(new HardStopCommand(runCommand.Worker));
        runCommand.Worker.AddCommandToQueue(new WriteLineCommand());
    }
}