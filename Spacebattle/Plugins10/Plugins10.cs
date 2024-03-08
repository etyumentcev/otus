using PluginBase;

namespace Plugins10;

/// <summary>
/// Корректно загружаемый плагин
/// </summary>
public class Plugin11 : IPlugin
{
    public void Load()
    {
        Console.WriteLine($"{this.GetType().Name} загружен.");
    }
}

/// <summary>
/// Корректно загружаемый плагин
/// </summary>
public class Plugin12 : IPlugin
{
    public void Load()
    {
        Console.WriteLine($"{this.GetType().Name} загружен.");
    }
}

/// <summary>
/// Плагин выдающий исключение при выполнении Load
/// </summary>
public class Plugin13 : IPlugin
{
    public void Load()
    {
        throw new Exception($"{this.GetType().Name} ошибка для проверки выполнения.");
    }
}