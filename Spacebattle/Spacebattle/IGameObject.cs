namespace Spacebattle
{
    public interface IGameObject
    {
        Guid Id { get; }

        string Name { get; }

        bool IsMoving { get; }
    }
}
