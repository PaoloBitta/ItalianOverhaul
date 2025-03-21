using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianOverhaul
{
    internal class IoRandom
    {
        // This is a simple random number generator that will generate a random number based on the seed.
        // The seed is generated from the current date and time, and is used to generate a random number.
        // This class will be used for everything random in the mod, from generating random company names to random events.

        private Random random;

        public IoRandom()
        {
            random = new Random(DateTime.Now.Millisecond);
        }

        public int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public double NextDouble()
        {
            return random.NextDouble();
        }

        public bool NextBool()
        {
            return random.Next(0, 2) == 1;
        }
    }
}
