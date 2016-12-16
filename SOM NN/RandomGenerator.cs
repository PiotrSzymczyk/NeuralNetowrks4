using System;

namespace SOM_NN
{
    public static class RandomGenerator
    {
        private static Random generator = new Random();

        public static double NextDouble()
        {
            lock (generator)
            {
                return generator.NextDouble();
            }
        }

        public static int Next(int max)
        {
            lock (generator)
            {
                return generator.Next(max);
            }
        }

        public static byte NextByte()
        {
            lock (generator)
            {
                return (byte) generator.Next(255);
            }
        }
    }
}
