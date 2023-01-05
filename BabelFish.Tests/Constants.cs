using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.Tests {
    public static class Constants {

        /// <summary>
        /// x-api-key assigned to BabelFish for unit testing.
        /// </summary>
        public const string X_API_KEY = "uONGn6tHGw14kreLdqbfJ9rwR2C55uS8a9rGnmIf";


        public static Dictionary<string, string> clientParamsTestDev1 = new Dictionary<string, string>()
        {
            {"UserName", "test_dev_1@shooterstech.net"},
            {"PassWord", "abcd1234"}
        };

        public static Dictionary<string, string> clientParamsTestDev7 = new Dictionary<string, string>()
        {
            {"UserName", "test_dev_7@shooterstech.net"},
            {"PassWord", "abcd1234"}
        };
    }
}
