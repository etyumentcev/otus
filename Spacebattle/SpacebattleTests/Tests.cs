using Spacebattle;
using Spacebattle.Interfaces;
using System.Collections.Concurrent;

public class CommandExecutorTests
{
    [Fact]
    public void ICommand_ShouldStarCommand()
    {
        // Arrange
        var comandCount = 10;
        var queue = new BlockingCollection<ICommand>();
        for (var i = 0; i < comandCount; i++)
        {
            var command = new TestCommand(i + 1);
            queue.Add(command);
        }
        var executor = new CommandExecutor(queue);
        var cts = new CancellationTokenSource();

        // Act
        Task.Run(() => executor.Execute(cts.Token));
        Thread.Sleep(comandCount * 250);
        cts.Cancel();

        // Assert
        foreach (var command in queue)
        {
            var testCommand = command as TestCommand;
            Assert.True(testCommand?.StateComplete);
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
            Thread.Sleep(200);
            StateComplete = true;
            Console.WriteLine($"Команда №{_id} выполнена");
        }
    }
}