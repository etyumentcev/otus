using Spacebattle.Interfaces;

namespace Spacebattle
{
    public static class SpaceshipHelper
    {
        public static IEnumerable<ISpaceship> Spaceships(this IEnumerable<ISpaceship> spaceships, long userId) => spaceships.Where(s => s.UserId == userId);

        public static ISpaceship? Spaceship(this IEnumerable<ISpaceship> spaceships, long id) => spaceships.SingleOrDefault(s => s.Id == id);

        public static IEnumerable<ISpaceship> Movable(this IEnumerable<ISpaceship> spaceships) => spaceships.Where(s => s.IsMoving);
    }
}
