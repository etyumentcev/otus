using Spacebattle.Interfaces;

public class CommandExecutorTests
{
    [Fact]
    public void ICommand_ShouldStarCommand()
    {
        // Arrange
        var command = new TestCommand(1);

        // Act
        command.Execute();

        // Assert
        Assert.True(command.StateComplete);
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
            Thread.Sleep(500);
            StateComplete = true;
            Console.WriteLine($"Команда №{_id} выполнена");
        }
    }
}