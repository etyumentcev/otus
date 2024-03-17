using Spacebattle.Interfaces;

namespace Spacebattle.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Получение списка космических кораблей принадлежащих игроку
        /// </summary>
        /// <param name="collection">коллекция игровых объектов</param>
        /// <param name="playerId">id игрока</param>
        /// <returns>Список космических кораблей</returns>
        public static IEnumerable<GameObjectBaseType> Spaseships(this IEnumerable<GameObjectBaseType> collection, int playerId)
        {
            return collection.Where(x => x.PlayerId == playerId && x.GetType() == typeof(Spaceship)).ToList();
        }

        /// <summary>
        /// Получение ссылки на объект
        /// </summary>
        /// <param name="collection">коллекция игровых объектов</param>
        /// <param name="objectId">id объекта</param>
        /// <returns>Ссылка на объект</returns>
        public static GameObjectBaseType? Spaceship(this IEnumerable<GameObjectBaseType> collection, int objectId)
        {
            return collection.FirstOrDefault(x => x.ObjectId == objectId);
        }

        /// <summary>
        /// Получение списка игровых объектов, которые находятся в движении
        /// </summary>
        /// <param name="collection">коллекция игровых объектов</param>
        /// <returns>Список движущихся объектов</returns>
        public static IEnumerable<GameObjectBaseType> Movable(this IEnumerable<GameObjectBaseType> collection)
        {
            return collection.Where(x => x.IsMoving).ToList();
        }
    }
}
