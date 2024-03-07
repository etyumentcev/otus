using Spacebattle;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var command3 = new ChainCommand(() => CommandAction(3, 0), null);
            var command2 = new ChainCommand(() => CommandAction(2, 0), command3);
            var command1 = new ChainCommand(() => CommandAction(1, 100), command2);

            command1.Execute()
                .GetAwaiter().GetResult();

            Console.ReadLine();
        }

        private static async Task<Task> CommandAction(int num, int delay)
        {
            await Task.Delay(delay);
            Console.WriteLine($"Команда{num}");
            return Task.CompletedTask;
        }
    }
}
