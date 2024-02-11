using Spacebattle.Interfaces;

namespace Spacebattle.Extensions;

public static class CollectionExtensions
{
    public static IEnumerable<GameObjectBase> Spaceships(this IEnumerable<GameObjectBase> collection, int playerId)
    {
        return collection
            .Where(x => x.PlayerId == playerId)
            .ToList();
    }

    public static GameObjectBase? Spaceship(this IEnumerable<GameObjectBase> collection, int objectId)
    {
        return collection
            .FirstOrDefault(x => x.ObjectId == objectId);
    }

    public static IEnumerable<GameObjectBase> Movable(this IEnumerable<GameObjectBase> collection)
    {
        return collection
            .Where(x => x.IsMoving)
            .ToList();
    }
}