using OTUS.HomeWorks.PluginInterfaces;

namespace Spacebattle
{
    public class Plugin : IPlugin
    {
        private int Number { get; set; }

        public void Load()
        {
            var isException = new Random().NextDouble() < 0.25;
            if (isException)
                throw new Exception($"Какая досада! Плагин №{Number} вызвал ошибку при загрузке.");
            
            Console.WriteLine($"Плагин №{Number} загружен.");
        }

        public void SetNumber(int i)
        {
            Number = i;
        }
    }
}
