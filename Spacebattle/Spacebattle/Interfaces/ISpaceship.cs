namespace Spacebattle.Interfaces
{
    public interface ISpaceship
    {
        public long Id { get; }
        public string Name { get; }
        public long UserId { get; }
        public bool IsMoving { get; }

    }
}
