global using Xunit;
using Commands;

public class CommandsTests
{
    [Fact]
    public void RunWorkerCommand_ThreadShouldRunning()
    {
        //Arrange
        var runCommand = new RunWorkerCommand();
        
        //Act
        runCommand.Execute();
        
        //Assert
        Assert.True(runCommand.Worker.WThread.ThreadState == ThreadState.Running || runCommand.Worker.WThread.ThreadState == ThreadState.WaitSleepJoin);
        
        runCommand.Worker.AddCommandToQueue(new HardStopCommand(runCommand.Worker));
    }

    [Fact]
    public void HardStopCommand_ThreadShouldStoppedEndQueueNotEmpty()
    {
        //Arrange
        var random = new Random();
        
        var runCommand = new RunWorkerCommand();

        // Добавление команд до hard stop и после, чтобы убедиться, что в очереди остались невыполненные команды
        for (var i = 0; i < random.Next(2, 10); i++)
        {
            runCommand.Worker.AddCommandToQueue(new WriteLineCommand());
        }
        
        runCommand.Worker.AddCommandToQueue(new HardStopCommand(runCommand.Worker));
        
        for (var j = 0; j < random.Next(2, 10); j++)
        {
            runCommand.Worker.AddCommandToQueue(new WriteLineCommand());
        }

        //Act
        runCommand.Execute();
        runCommand.Worker.WThread.Join();

        //Assert
        Assert.Equal(ThreadState.Stopped, runCommand.Worker.WThread.ThreadState);
        Assert.NotEmpty(runCommand.Worker.Queue);
    }

    [Fact]
    public void SoftStopCommand_ThreadShouldStoppedEndQueueEmpty()
    {
        //Arrange
        var random = new Random();
        
        var runCommand = new RunWorkerCommand();

        // Добавление команд до hard stop и после, чтобы убедиться, что в очереди остались невыполненные команды
        for (var i = 0; i < random.Next(2, 10); i++)
        {
            runCommand.Worker.AddCommandToQueue(new WriteLineCommand());
        }
        
        runCommand.Worker.AddCommandToQueue(new SoftStopCommand(runCommand.Worker));
        
        for (var j = 0; j < random.Next(2, 10); j++)
        {
            runCommand.Worker.AddCommandToQueue(new WriteLineCommand());
        }

        //Act
        runCommand.Execute();
        runCommand.Worker.WThread.Join();

        //Assert
        Assert.Equal(ThreadState.Stopped, runCommand.Worker.WThread.ThreadState);
        Assert.Empty(runCommand.Worker.Queue);
    }
}