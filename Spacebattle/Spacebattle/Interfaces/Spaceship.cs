namespace Spacebattle.Interfaces
{
    internal class Spaceship : GameObjectBaseType
    {
        public Spaceship(int objectId, int playerId, bool isMoving) :
            base(objectId, playerId, isMoving) { }
    }
}
