using Spacebattle;
using Spacebattle.Interfaces;

namespace SpacebattleTests
{
    public class SpaceshipHelperTests
    {
        private class Spaceship : ISpaceship
        {
            public Spaceship(long id, string name, long userId, bool isMoving)
            {
                Id = id;
                Name = name;
                UserId = userId;
                IsMoving = isMoving;
            }
            public long Id { get; set; }
            public string Name { get; set; }
            public long UserId { get; set; }
            public bool IsMoving { get; set; }
            public override string ToString() { return Name; }
        }

        private IEnumerable<Spaceship> TestSet => new Spaceship[]
        {
            new Spaceship(1, "Москва", 1, true),
            new Spaceship(2, "Волгоград", 1, false),
            new Spaceship(3, "Нагибатор", 3, false),
            new Spaceship(4, "Серсея", 2, true),
            new Spaceship(5, "Севастополь", 1, false),
            new Spaceship(6, "Мирцелла", 2, false),
            new Spaceship(7, "Архангельск", 1, true),
        };

        [Fact]
        public void SpaceshipsSholudGetUserSpaceships()
        {
            var userId = 1;
            var userIdEmpty = 4;

            var userSpacships = TestSet.Spaceships(userId);
            var emptySpacships = TestSet.Spaceships(userIdEmpty);


            Assert.True(userSpacships.Count() == 4);
            Assert.True(emptySpacships.Count() == 0);
        }

        [Fact]
        public void SpaceshipSholudFindSpaceshipById()
        {
            var id = 1;
            var idEmpty = 8;

            var spacship = TestSet.Spaceship(id);
            var empty = TestSet.Spaceship(idEmpty);


            Assert.True(spacship?.Name == "Москва");
            Assert.True(empty == null);
        }

        [Fact]
        public void MovableSholudGetMovingSpaceships()
        {
            var movingSpacships = TestSet.Movable();

            Assert.True(movingSpacships.Count() == 3);
        }
    }
}
