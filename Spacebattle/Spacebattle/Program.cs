using Spacebattle.Commands;

namespace Spacebattle;

public static class Program
{
    public static async void Main()
    {
        var final = new FinalCommand();
        var cmd3 = new Command3(final);
        var cmd2 = new Command2(cmd3);
        var cmd1 = new Command1(cmd2);

        await cmd1.Execute();
    }
}