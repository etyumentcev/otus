namespace Spacebattle
{
    public interface IPlayer
    {
        Guid Id { get; }

        string Name { get; }

        IEnumerable<ISpaceship> Spaceships { get; }
    }
}
