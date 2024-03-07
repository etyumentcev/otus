using Spacebattle;
using Spacebattle.Interfaces;
using System.Collections.Concurrent;

public class CommandExecutorTests
{
    private const int COMMAND_JOB_TIME = 200;

    [Fact]
    public void HardStopMainCommand_ShouldHardStopExecutor()
    {
        // Arrange
        var comandCount = 10;
        var queue = new List<ICommand>();
        var blockingCollection = new BlockingCollection<ICommand>();
        for (var i = 0; i < comandCount; i++)
        {
            var command = new TestCommand(i + 1);
            queue.Add(command);
            blockingCollection.Add(command);
        }
        var executor = new CommandExecutor(blockingCollection);
        var cts = new CancellationTokenSource();
        var mainCommand = new MainCommand(executor, cts.Token);
        var hardStopMainCommand = new HardStopMainCommand(cts);

        // Act
        mainCommand.Execute();
        Thread.Sleep((4 * COMMAND_JOB_TIME) + 100);
        hardStopMainCommand.Execute();
        Thread.Sleep(1000);

        // Assert
        Assert.False(executor.InProgress);
        for (var i = 0; i < 5; i++)
        {
            var command = queue[i];
            var testCommand = command as TestCommand;
            Assert.True(testCommand != null);
            Assert.True(testCommand.StateComplete);
        }

        for (var i = 5; i < queue.Count; i++)
        {
            var command = queue[i];
            var testCommand = command as TestCommand;
            Assert.True(testCommand != null);
            Assert.False(testCommand.StateComplete);
        }
    }

    [Fact]
    public void SoftStopMainCommand_ShouldSoftStopExecutor()
    {
        // Arrange
        var comandCount = 10;
        var queue = new List<ICommand>();
        var blockingCollection = new BlockingCollection<ICommand>();
        for (var i = 0; i < comandCount; i++)
        {
            var command = new TestCommand(i + 1);
            queue.Add(command);
            blockingCollection.Add(command);
        }
        var executor = new CommandExecutor(blockingCollection);
        var cts = new CancellationTokenSource();
        var mainCommand = new MainCommand(executor, cts.Token);
        var softStopMainCommand = new SoftStopMainCommand(mainCommand, cts);

        // Act
        mainCommand.Execute();
        Thread.Sleep((4 * COMMAND_JOB_TIME) + 100);
        softStopMainCommand.Execute();
        Thread.Sleep((6 * COMMAND_JOB_TIME) + 100);

        // Assert
        Assert.False(executor.InProgress);
        for (var i = 0; i < 10; i++)
        {
            var command = queue[i];
            var testCommand = command as TestCommand;
            Assert.True(testCommand != null);
            Assert.True(testCommand.StateComplete);
        }
    }

    [Fact]
    public void ChainMainCommand_ShouldExecuteChainCommand()
    {
        // Arrange
        var results = new List<int>(3);
        var func = async (int num) =>
        {
            if (num == 1)
                await Task.Delay(100);
            Console.WriteLine($"Команда{num}");
            results.Add(num);
            return Task.CompletedTask;
        };
        var command3 = new ChainCommand(() => func(3), null);
        var command2 = new ChainCommand(() => func(2), command3);
        var command1 = new ChainCommand(() => func(1), command2);

        // Act
        command1.Execute()
            .GetAwaiter().GetResult();

        // Assert
        Assert.True(results[0] == 2);
        Assert.True(results[1] == 3);
        Assert.True(results[2] == 1);
        Assert.True(command1.Complete);
        Assert.True(command2.Complete);
        Assert.True(command3.Complete);
    }

    private class TestCommand : ICommand
    {
        private readonly int _id;
        public TestCommand(int id)
        {
            _id = id;
        }

        public bool StateComplete { get; private set; }

        public async Task Execute()
        {
            Console.WriteLine($"Запуск выполнения команды №{_id}");
            await Task.Delay(COMMAND_JOB_TIME);
            StateComplete = true;
            Console.WriteLine($"Команда №{_id} выполнена");
        }
    }
}