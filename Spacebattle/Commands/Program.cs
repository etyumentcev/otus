namespace Commands;

class Program
{
    static void Main(string[] args)
    {
        var command3 = new Command3(new FinishCommand());
        var command2 = new Command2(command3);
        var command1 = new Command1(command2);
        
        command1.Execute().GetAwaiter().GetResult();
    }
}