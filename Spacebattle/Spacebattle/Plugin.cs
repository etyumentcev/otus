using OTUS.HomeWorks.PluginInterfaces;

namespace Spacebattle
{
    public class Plugin : IPlugin
    {
        public void Load() => Console.WriteLine("Plugin is Loaded");
    }
}
