using Spacebattle.Interfaces;

namespace Spacebattle.Models;

public class Spaceship : GameObjectBase
{
    public Spaceship(int objectId, int playerId, bool isMoving) 
        : base(objectId, playerId, isMoving) { }
}