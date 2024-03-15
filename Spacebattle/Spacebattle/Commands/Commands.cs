using Spacebattle.Interfaces;

namespace Spacebattle.Commands;

public class HardStopCommand : ICommand
{
    private readonly CancellationTokenSource _tokenToHardStop;
    
    public HardStopCommand(CancellationTokenSource tokenSource)
    {
        _tokenToHardStop = tokenSource;
    }
    
    public void Execute()
    {
        _tokenToHardStop.Cancel();
    }
}

public class SoftStopCommand : ICommand
{
    private readonly CancellationTokenSource _tokenToSoftStop;

    public SoftStopCommand(CancellationTokenSource tokenSource)
    {
        _tokenToSoftStop = tokenSource;
    }
    public void Execute()
    {
        _tokenToSoftStop.Cancel();
    }
}

public class MainCommand : ICommand
{
    private readonly Commander _commander;

    public MainCommand(Commander commander)
    {
        _commander = commander;
    }

    public void Execute()
    {
        _commander.CommanderThread.Start();
    }
}