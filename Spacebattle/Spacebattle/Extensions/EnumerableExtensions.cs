namespace Spacebattle.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<ISpaceship> Spaceships(this IEnumerable<IPlayer> players, Guid playerId)
        {
            var player = players.Where(_ => _.Id == playerId).FirstOrDefault();

            if (player != null)
            {
                return player.Spaceships;
            }

            return Enumerable.Empty<ISpaceship>();
        }

        public static ISpaceship Spaceship(this IEnumerable<ISpaceship> spaceships, Guid spaceshipId) => spaceships.Where(_ => _.Id == spaceshipId).FirstOrDefault();

        public static IEnumerable<IGameObject> Movable(this IEnumerable<IGameObject> gameObjects) => gameObjects.Where(_ => _.IsMoving).ToList();
    }
}
