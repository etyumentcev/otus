using System.Collections.Concurrent;
using System.Reflection;
using PluginBase;

namespace PluginLoader;

class Program
{
    private const string Extension = "*.dll";
    private static volatile bool _shouldStop = false;
    
    // Необходимо указать путь к каталогу с плагинами (*.dll файлы, содержащие классы, реализующие IPlugin)
    // В качестве примера можно собрать и использовать проекты Plugins10 и Plugins20 (Собранные в dll они находятся в каталоге PluginsDLL)
    private const string PluginsPath = @"d:\plugins";
    
    static void Main(string[] args)
    {
        var queue = new ConcurrentQueue<IPlugin>();

        var threadGetFromFolder = new Thread(() => GetPluginsFromFolder(PluginsPath, queue));
        var threadGetFromQueue = new Thread(() => LoadPluginsFromQueue(queue));
        
        Console.WriteLine($"Старт загрузки плагинов.");
        
        threadGetFromFolder.Start();
        threadGetFromQueue.Start();
        
        threadGetFromFolder.Join();
        threadGetFromQueue.Join();
        
        Console.WriteLine($"Загрузка плагинов завершена.");
    }

    private static void GetPluginsFromFolder(string pathToPlugins, ConcurrentQueue<IPlugin> queue)
    {
        try
        {
            var files = Directory.GetFiles(pathToPlugins, Extension, SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                Console.WriteLine($"В каталоге {pathToPlugins} не найдено файлов с расширением {Extension}.");
                return;
            }

            foreach (var fileToLoad in files)
            {
                try
                {
                    var pluginAssembly = Assembly.LoadFrom(fileToLoad);
                    var pluginTypes = pluginAssembly.GetTypes().Where(x => typeof(IPlugin).IsAssignableFrom(x))
                        .ToList();

                    foreach (var pluginType in pluginTypes)
                    {
                        IPlugin pluginInstance = Activator.CreateInstance(pluginType) as IPlugin;
                        queue.Enqueue(pluginInstance);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"При загрузке сборки файла `{fileToLoad}` произошла ошибка `{e.Message}`");
                }
                
                // Имитация длительной загрузки файлов
                Thread.Sleep(12000);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            _shouldStop = true;
        }
    }

    private static void LoadPluginsFromQueue(ConcurrentQueue<IPlugin> queue)
    {
        int srcQueueCount = queue.Count;
        var queueErrorsLoadPlugins = new ConcurrentQueue<IPlugin>();
        IPlugin plugin;
        while (queue.TryDequeue(out plugin) || !_shouldStop)
        {
            if (plugin != null)
            {
                try
                {
                    plugin.Load();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"При загрузке плагина {plugin} произошла ошибка `{ex.Message}`");
                    queueErrorsLoadPlugins.Enqueue(plugin);
                }
            }
        }

        if (!queueErrorsLoadPlugins.IsEmpty && queueErrorsLoadPlugins.Count != srcQueueCount)
        {
            LoadPluginsFromQueue(queueErrorsLoadPlugins);
        }
        else
        {
            Console.WriteLine($"Количество незагруженных плагинов: {queueErrorsLoadPlugins.Count}`");
        }
    }
}