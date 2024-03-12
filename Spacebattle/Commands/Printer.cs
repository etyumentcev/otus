namespace Commands;

public static class Printer
{
    /// <summary>
    /// Асинхронный вывод номера выполняемой команды
    /// </summary>
    /// <param name="number">Номер команды</param>
    public static async Task PrintCommandNumberAsync(int number)
    {
        // Задержка, чтобы в методе было что-то асинхронное
        await Task.Delay(1000);
        Console.WriteLine($"Команда №{number}");
    }
}