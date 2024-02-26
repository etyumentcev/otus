using Spacebattle.Interfaces;

namespace Spacebattle.Extensions
{
    public static class SpacebattleExtensions
    {
        public static IEnumerable<ISpaceShip> GetSpaceshipsByPlayerId(this IEnumerable<ISpaceShip> spaceships, int playerId)
        {
            return spaceships.Where(s => s.PlayerId == playerId);
        }

        public static IGameObject? GetObjectById(this IEnumerable<IGameObject> gameObjects, int objectId)
        {
            return gameObjects.FirstOrDefault(obj => obj.ObjectId == objectId);
        }

        public static IEnumerable<IGameObject> GetMovingObjects(this IEnumerable<ISpaceShip> spaceships)
        {
            return spaceships.Where(obj => obj.Moving);
        }
    }
}
