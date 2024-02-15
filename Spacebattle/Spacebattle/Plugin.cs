using OTUS.HomeWorks.PluginInterfaces;

namespace Spacebattle
{
    public class Plugin : IPlugin
    {
        private readonly string[] mainWord = new[] { "Лебедь", "Слон", "Тигр", "Енот", "Волк", "Лис", "Кот", "Пёс", "Конь" };
        private readonly string[] addWord = new[] { "Крассный", "Синий", "Жёлтый", "Чёрный", "Зелёный", "Белый", "Бурый", "Рыжий", "Серый" };

        private string? _name;

        private string GeneratePluginName()
        {
            if (!string.IsNullOrEmpty(_name))
                return _name;

            var random = new Random();
            return $"{addWord[random.Next(addWord.Length - 1)]} {mainWord[random.Next(addWord.Length - 1)]}";
        }

        public void Load()
        {
            _name = GeneratePluginName();

            var isException = new Random().NextDouble() < 0.25;
            if (isException)
                throw new Exception($"Какая досада! Плагин \"{_name}\" вызвал ошибку при загрузке.");

            Console.WriteLine($"Плагин \"{_name}\" загружен.");
        }
    }
}
