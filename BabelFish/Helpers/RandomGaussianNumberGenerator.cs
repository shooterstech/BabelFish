using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Helpers {
    public class RandomGaussianNumberGenerator {
        private readonly Random _rand;

        public RandomGaussianNumberGenerator() {
            _rand = new Random();
        }

        // mean = μ, standardDeviation = σ
        public double NextGaussian( double mean = 0, double standardDeviation = 1 ) {
            // Generate two uniform random numbers between (0,1)
            double u1 = 1.0 - _rand.NextDouble(); // avoid 0
            double u2 = 1.0 - _rand.NextDouble();

            // Box-Muller transform
            double randStdNormal = Math.Sqrt( -2.0 * Math.Log( u1 ) ) *
                                   Math.Sin( 2.0 * Math.PI * u2 );

            // Scale and shift
            return mean + standardDeviation * randStdNormal;
        }
    }
}
