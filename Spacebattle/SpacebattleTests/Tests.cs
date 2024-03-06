using Spacebattle;
using Spacebattle.Interfaces;
using System.Collections.Concurrent;

public class CommandExecutorTests
{
    private const int COMMAND_JOB_TIME = 200;

    [Fact]
    public void ICommand_ShouldStarCommand()
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
        var stopMainCommand = new StopMainCommand(cts.Token);

        // Act
        mainCommand.Execute();
        Thread.Sleep(5 * COMMAND_JOB_TIME + 100);
        stopMainCommand.Execute();

        // Assert
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
            Assert.True(!testCommand.StateComplete);
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