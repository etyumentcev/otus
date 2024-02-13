using OTUS.HomeWorks.PluginInterfaces;
using System.Reflection;

namespace PluginLoader
{
    internal class Program
    {
        private const string PLUGIN_PATH = "C:\\Temp\\Plugin\\Spacebattle.dll";

        static void Main(string[] args)
        {
            Console.WriteLine("Положите плагин Spacebattle.dll в папку C:\\Temp\\Plugin и нажмите любую кнопку.");
            Console.ReadKey();
            Console.WriteLine();

            var plugin = LoadPlugin();
            plugin.Load();
            Console.WriteLine("Плагин загружен");
        }

        private static IPlugin LoadPlugin()
        {
            var assembly = Assembly.LoadFrom(PLUGIN_PATH);
            if (assembly == null)
                throw new Exception("Не найден файл плагина.");

            Type[] pluginTypes = assembly.GetTypes();
            Type? pluginType = pluginTypes.FirstOrDefault(type => typeof(IPlugin).IsAssignableFrom(type));

            if (pluginType == null)
                throw new Exception("Файл плагина не содержит интерфейс IPlugin");

            return (IPlugin)Activator.CreateInstance(pluginType);
        }
    }
}
