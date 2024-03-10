namespace Commands;

/// <summary>
/// Создание и запуск Workerа в рамках которого будет запущен поток с циклом выполнения задач
/// </summary>
public class RunWorkerCommand: ICommand
{
    public Worker Worker { get; }

    public RunWorkerCommand()
    {
        Worker = new Worker();
    }

    /// <summary>
    /// Старт цикла выполнения команд
    /// </summary>
    public void Execute()
    {
        Worker.WThread.Start();
    }
}

/// <summary>
/// Остановка цикла выполнения команд, дожидаясь их полного завершения 
/// </summary>
public class SoftStopCommand: ICommand
{
    private readonly CancellationTokenSource _cts;

    public SoftStopCommand(Worker worker)
    {
        _cts = worker.CtsSoft;
    }
    public void Execute()
    {
        _cts.Cancel();
    }
}

/// <summary>
/// Остановка цикла выполнения команд, не дожидаясь их полного завершения 
/// </summary>
public class HardStopCommand: ICommand
{
    private readonly CancellationTokenSource _cts;

    public HardStopCommand(Worker worker)
    {
        _cts = worker.CtsHard;
    }
    public void Execute()
    {
        _cts.Cancel();
    }
}