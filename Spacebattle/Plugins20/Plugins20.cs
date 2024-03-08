using PluginBase;

namespace Plugins20;

/// <summary>
/// Корректно загружаемый плагин
/// </summary>

public class Plugin21 : IPlugin
{
    public void Load()
    {
        Console.WriteLine($"{this.GetType().Name} загружен.");
    }
}
/// <summary>
/// Плагин, который не должен загрузиться, т.к. не реализует интерфейс IPlugin 
/// </summary>

public class Plugin22
{
    public void Load()
    {
        Console.WriteLine($"{this.GetType().Name} загружен.");
    }
}
