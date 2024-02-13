using OTUS.HomeWorks.PluginInterfaces;
using System.Collections.Concurrent;
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

            var queue = new ConcurrentQueue<IPlugin>();
            for (var i = 0; i < 12;  i++)
            {
                var plugin = LoadPlugin();
                if (plugin == null)
                {
                    Console.WriteLine($"Не удалось найти плагин №{i}");
                    continue;
                }
                plugin.SetNumber(i);
                queue.Enqueue(plugin);
            }

            Thread thread = new Thread(() =>
            {
                var maxCount = queue.Count * 2;
                var count = 0;
                IPlugin item;
                while (queue.TryDequeue(out item))
                {
                    try
                    {
                        item.Load();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"При загрузке плагина произошла ошибка: {ex.Message}. Он будет загружен повторно позднее.");
                        queue.Enqueue(item);
                    }

                    count++;
                    if (count >= maxCount)
                    {
                        Console.WriteLine($"Количество попыток загрузки плагинов превысило максимальное значение.");
                        break;
                    }
                }
            });

            thread.Start();
            thread.Join();

            if (queue.IsEmpty)
                Console.WriteLine("Все плагины загружены.");
            else
                Console.WriteLine($"Не загрузилось плагинов: {queue.Count}. Вам просто не повезло, попробуйте ещё раз.");

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
