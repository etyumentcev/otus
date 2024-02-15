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
            Console.WriteLine("Положите плагин Spacebattle.dll в папку C:\\Temp\\Plugin и нажмите Enter.");
            Console.ReadLine();

            var queue = GeneratePluginQueue();
            var success = TryRunPluginQueue(queue);

            if (success)
                Console.WriteLine("Все плагины загружены.");
            else
                Console.WriteLine(
                    $"Часть плагинов так и не удалось загрузить. " +
                    $"Вам просто не повезло, попробуйте ещё раз.");
        }

        private static bool TryRunPluginQueue(ConcurrentQueue<IPlugin> queue)
        {
            var nextQueue = new ConcurrentQueue<IPlugin>();
            Thread thread = new Thread(() =>
            {
                while (queue.Count > 0)
                {
                    var queueCount = queue.Count;

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
                            nextQueue.Enqueue(item);
                        }
                    }
                    Console.WriteLine($"=====");

                    if (queueCount == nextQueue.Count)
                        break;

                    if (nextQueue.Any())
                        Console.WriteLine($"Часть плагинов не были загружены. Предпринимается повторная попытка их загрузить.");

                    queue = nextQueue;
                    nextQueue = new ConcurrentQueue<IPlugin>();
                }

            });

            thread.Start();
            thread.Join();

            return nextQueue.IsEmpty;
        }

        private static ConcurrentQueue<IPlugin> GeneratePluginQueue()
        {
            var queue = new ConcurrentQueue<IPlugin>();
            for (var i = 0; i < 12; i++)
            {
                var plugin = LoadPlugin();
                if (plugin == null)
                {
                    Console.WriteLine($"Не удалось найти плагин №{i}");
                    continue;
                }
                queue.Enqueue(plugin);
            }

            return queue;
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
