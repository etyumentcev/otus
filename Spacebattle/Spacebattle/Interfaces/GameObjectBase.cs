namespace Spacebattle.Interfaces;

public abstract class GameObjectBase
{
    public int ObjectId { get; set; }
    public int PlayerId { get; set; } 
    public bool IsMoving { get; set; }

    protected GameObjectBase(int objectId, int playerId, bool isMoving)
    {
        ObjectId = objectId;
        PlayerId = playerId;
        IsMoving = isMoving;
    }
}