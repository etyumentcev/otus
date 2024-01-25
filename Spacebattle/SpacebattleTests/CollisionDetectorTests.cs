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
        public void CollissionDetectorSholudFindKnownPatterns()
        {

            var detector = new CollisionDetector<int>();

            bool wasDetected = false;
            Action action = () => { wasDetected = true; };
            // сюда добавить подписку на событие - коллизия обнаружена
            detector.Detected += action;
            detector.Add(new int[] { 2, 7, 8, -3 });
            detector.Add(new int[] { 2, 7, 8, 2 });
            detector.Add(new int[] { 2, 7, 8, 15 });

            detector.Detect(new int[] { 2, 7, 8, 2 });

            Assert.True(wasDetected);
        }

        [Fact]
        public void CollissionDetectorSholudNotFindUnknownPatterns()
        {

            var detector = new CollisionDetector<int>();

            bool wasDetected = false;
            Action action = () => { wasDetected = true; };
            // сюда добавить подписку на событие - коллизия обнаружена
            detector.Detected += action;
            detector.Add(new int[] { 2, 7, 8, -3 });
            detector.Add(new int[] { 2, 7, 8, 15 });

            detector.Detect(new int[] { 2, 7, 8, 2 });

            Assert.False(wasDetected);
        }
    }
}
