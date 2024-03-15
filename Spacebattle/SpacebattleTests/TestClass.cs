global using Xunit;
using System.Collections.Concurrent;
using Spacebattle;
using Spacebattle.Commands;
using Spacebattle.Interfaces;

namespace SpacebattleTests;

public class TestClass
{
    private List<TestCommand> _testCommands = new List<TestCommand>()
    {
        new TestCommand("Test command 1"),
        new TestCommand("Test command 2"),
        new TestCommand("Test command 3"),
        new TestCommand("Test command 4"),
        new TestCommand("Test command 5"),
        new TestCommand("Test command 6")
    };

    [Fact]
    public void MainCommandExecuting_StartsThread()
    {
        // Arrange
        var queue = new ConcurrentQueue<ICommand>();
        foreach (var command in _testCommands)
        {
            queue.Enqueue(command);
        }
        var commander = new Commander(queue);
        var mainCommand = new MainCommand(commander);
        
        // Act
        mainCommand.Execute();
        
        //Assert
        Assert.True(commander.CommanderThread is { IsAlive: true, ThreadState: ThreadState.Running });
        commander.AddCommand(new SoftStopCommand(commander.SoftStopToken));
    }
    
    [Fact]
    public void HardStopCommandExecuting_StopsThread()
    {
        // Arrange
        var queue = new ConcurrentQueue<ICommand>();
        for (var i = 0; i < 3; i++)
        {
            queue.Enqueue(_testCommands[i]);
        }
        var commander = new Commander(queue);
        var hardStopCommand = new HardStopCommand(commander.HardStopToken);
        commander.AddCommand(hardStopCommand);
        for (var i = 3; i < _testCommands.Count; i++)
        {
            commander.AddCommand(_testCommands[i]);
        }
        var mainCommand = new MainCommand(commander);
        
        // Act
        mainCommand.Execute();
        Thread.Sleep(1000);
        
        //Assert
        Assert.Equal(ThreadState.Stopped,  commander.CommanderThread.ThreadState);
        Assert.Equal(3, commander.CommandQueue.Count);
    }
    
    [Fact]
    public void SoftStopCommandExecuting_StopsThread()
    {
        // Arrange
        var queue = new ConcurrentQueue<ICommand>();
        for (var i = 0; i < 3; i++)
        {
            queue.Enqueue(_testCommands[i]);
        }
        var commander = new Commander(queue);
        var softStopCommand = new SoftStopCommand(commander.SoftStopToken);
        commander.AddCommand(softStopCommand);
        for (var i = 3; i < _testCommands.Count; i++)
        {
            commander.AddCommand(_testCommands[i]);
        }
        var mainCommand = new MainCommand(commander);
        
        // Act
        mainCommand.Execute();
        Thread.Sleep(1000);
        
        //Assert
        Assert.True(commander.CommanderThread.ThreadState == ThreadState.Stopped);
        Assert.Empty(commander.CommandQueue);
    }
}

public class TestCommand : ICommand
{
    private string _message;
    
    public TestCommand(string message)
    {
        _message = message;
    }

    public void Execute()
    {
        Console.WriteLine(_message);
    }
}