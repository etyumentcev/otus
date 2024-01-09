using System.Collections.Generic;

namespace Spacebattle
{
    public class VectorTests
    {
        [Fact]
        public void VectorSizeShouldBeEqualTheSizeOfInitialValuesSequences()
        {
            var list = new List<int> { 1, 2, 3, 4 };
            Vector<int> v = new Vector<int>(list);

            Assert.Equal(list.Count, v.Size);
        }

        [Fact]
        public void VectorCannotBeInitializedByEmptySequence()
        {
            Assert.Throws<ArgumentException>(() => new Vector<int>(new List<int>()));
        }

        [Fact]
        public void ArrayCanBeAssignedToVector()
        {
            Vector<int> v = new int[] { 1, 2, 3 };

            Assert.Equal(3, v.Size);
        }

        [Fact]
        public void EmptyArraytCannotBeAssignedToVector()
        {
            Assert.Throws<ArgumentException>(() => { Vector<int> v = new int[0]; });
        }

        [Fact]
        public void VectorIsASequence()
        {
            var arr = new int[] { 1, 2, 3 };
            Vector<int> v = arr;

            Assert.Equal(new int[] { 1, 2, 3 }, v.ToList<int>());
            Assert.NotEqual(new int[] { 1, 2 }, v.ToList<int>());
        }

        [Fact]
        public void VectorCanBeConvertedToString()
        {
            var vector = new Vector<int>( new int[]{ 1, 2, 3 } );
            Assert.Equal("(1, 2, 3)", vector.ToString());
        }

        [Fact]
        public void VectorCanBeParsedfromString()
        {
            var v = Vector<int>.Parse("(1, 2, 3, 4)", (string v) => int.Parse(v));

            Assert.Equal(new int[] { 1, 2, 3, 4 }, v.ToList<int>());
        }

        [Fact]
        public void VectorParseShouldThorwExceptionIfStringHasAwrongFormat()
        {
            Assert.Throws<FormatException>(()=> 
              Vector<int>.Parse("(1, 2, 3, 4) ", (string v) => int.Parse(v))
            );

            Assert.Throws<FormatException>(() =>
              Vector<int>.Parse("(1, 2, 3,)", (string v) => int.Parse(v))
            );

            Assert.Throws<FormatException>(() =>
              Vector<int>.Parse("()", (string v) => int.Parse(v))
            );
        }

        [Fact]
        public void VectorsCanBeAdded()
        {
            Vector<int> v1 = new int[] { 1, 2, 3};
            Vector<int> v2 = new int[] { 2, 3, 4 };

            Assert.Equal(new int[] { 3, 5, 7 }, (v1 + v2).ToList());
        }

        [Fact]
        public void AddidngVectorsMustHaveSameSize()
        {
            Vector<int> v1 = new int[] { 1, 2, 3 };
            Vector<int> v2 = new int[] { 2, 3, 4, 5 };

            Assert.Throws<ArgumentException>(() => v1 + v2);
        }
    }
}
