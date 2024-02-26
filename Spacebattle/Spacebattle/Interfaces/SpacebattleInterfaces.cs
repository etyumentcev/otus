namespace Spacebattle.Interfaces
{

    public interface IGameObject
    {
        public int ObjectId { get; }
        public string ObjectName { get; }
        public bool Moving { get; }
    }
    public interface ISpaceShip : IGameObject
    {
      public int PlayerId { get; }
      
    }
}
