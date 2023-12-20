using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spacebattle
{
    public class CollisionDetectorTests
    {
        [Fact]
        public void CollissionDetectorShouldFindKnownPatterns()
        {
            var detector = new CollisionDetector<int>();

            bool wasDetected = false;
            Action action = () => { wasDetected = true; };
        
            // Подписываем делегат на событие Detected
            detector.Detected += action;

            detector.Add(new int[] { 2, 7, 8, -3 });
            detector.Add(new int[] { 2, 7, 8, 2 });
            detector.Add(new int[] { 2, 7, 8, 15 });

            detector.Detect(new int[] { 2, 7, 8, 2 });

            // Проверяем, что событие было обнаружено
            Assert.True(wasDetected);
        }

        [Fact]
        public void CollissionDetectorShouldNotFindUnknownPatterns()
        {
            var detector = new CollisionDetector<int>();

            bool wasDetected = false;
            Action action = () => { wasDetected = true; };

            // Подписываем делегат на событие Detected
            detector.Detected += action;

            detector.Add(new int[] { 2, 7, 8, -3 });
            detector.Add(new int[] { 2, 7, 8, 15 });

            detector.Detect(new int[] { 2, 7, 8, 2 });

            // Проверяем, что событие не было обнаружено
            Assert.False(wasDetected);
        }
    }
}
