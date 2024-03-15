using System.Collections.Concurrent;
using Spacebattle.Interfaces;

namespace Spacebattle;

public class Commander
{
    public ConcurrentQueue<ICommand> CommandQueue { get; }
    
    public Thread CommanderThread { get; }
    
    public CancellationTokenSource SoftStopToken { get; }
    
    public CancellationTokenSource HardStopToken { get; }

    public Commander(ConcurrentQueue<ICommand> queue)
    {
        CommanderThread = new Thread(DoWork);
        CommandQueue = queue;
        SoftStopToken = new CancellationTokenSource();
        HardStopToken = new CancellationTokenSource();
    }

    private void DoWork()
    {
        while (!SoftStopToken.IsCancellationRequested && !HardStopToken.IsCancellationRequested)
        {
            while (!CommandQueue.IsEmpty)
            {
                try
                {
                    if (HardStopToken.IsCancellationRequested)
                    {
                        break;
                    }
                    CommandQueue.TryDequeue(out var command);
                    command?.Execute();
                }
                catch
                {
                    Console.WriteLine("Exception occured");
                }
            }
        }
        Console.WriteLine("Выполнение завершено");
    }

    public void AddCommand(ICommand command)
    {
        CommandQueue.Enqueue(command);
    }
}