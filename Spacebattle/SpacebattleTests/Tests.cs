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

    private class TestCommand : ICommand
    {
        private readonly int _id;
        public TestCommand(int id)
        {
            _id = id;
        }

        public bool StateComplete { get; private set; }

        public void Execute()
        {
            Console.WriteLine($"Запуск выполнения команды №{_id}");
            Thread.Sleep(COMMAND_JOB_TIME);
            StateComplete = true;
            Console.WriteLine($"Команда №{_id} выполнена");
        }
    }
}