using System.Collections.Concurrent;

namespace Commands;

public class Worker
{
    /// <summary>
    /// Поток для выполнения команд из очереди
    /// </summary>
    public Thread WThread { get; }
    
    /// <summary>
    /// Очередь команд
    /// </summary>
    public ConcurrentQueue<ICommand> Queue { get; }

    /// <summary>
    /// Токен для soft stop
    /// </summary>
    public CancellationTokenSource CtsSoft { get; }


    /// <summary>
    /// Токен для soft stop
    /// </summary>
    public CancellationTokenSource CtsHard { get; }
    
    public Worker()
    {
        CtsHard = new CancellationTokenSource();
        CtsSoft = new CancellationTokenSource();
        Queue = new ConcurrentQueue<ICommand>();
        WThread = new Thread(new ThreadStart(ExecutCommandFromQueue));
    }

    /// <summary>
    /// Обработчик команд
    /// </summary>
    public void ExecutCommandFromQueue()
    {
        while (!CtsSoft.IsCancellationRequested && !CtsHard.IsCancellationRequested)
        {
            ICommand command;
            while (Queue.TryDequeue(out command))
            {
                if (CtsHard.IsCancellationRequested)
                {
                    break;
                }

                if (command != null)
                {
                    try
                    {
                        command.Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"При выполнении команыды: {command} произошла ошибка `{ex.Message}`");
                    }
                }
            }
        }
        
        var message = CtsSoft.IsCancellationRequested ? "SoftStop" : "HardStop";
        Console.WriteLine($"Выполнение потока прервано {message}");
    }

    /// <summary>
    /// Добавление команд, реализующих интерфейс ICommand
    /// </summary>
    /// <param name="newCommand">Команда (объект, реализующий ICommand)</param>
    public void AddCommandToQueue(ICommand newCommand)
    {
        try
        {
            Queue.Enqueue(newCommand);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось добавить команду: {ex}");
        }
    }
}