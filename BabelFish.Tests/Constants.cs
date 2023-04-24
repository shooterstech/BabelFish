using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Tests {
    public static class Constants {

        /// <summary>
        /// x-api-key assigned to BabelFish for unit testing.
        /// </summary>
        public const string X_API_KEY = "uONGn6tHGw14kreLdqbfJ9rwR2C55uS8a9rGnmIf";


        public static BasicUserCredentials TestDev1Credentials = new BasicUserCredentials() {
            Username = "test_dev_1@shooterstech.net",
            Password = "abcd1234"
        };

        public static BasicUserCredentials TestDev3Credentials = new BasicUserCredentials() {
            Username = "test_dev_3@shooterstech.net",
            Password = "abcd1234"
        };

        public static BasicUserCredentials TestDev7Credentials = new BasicUserCredentials() {
            Username = "test_dev_7@shooterstech.net",
            Password = "abcd1234",
            DeviceKey = "us-east-1_2290d8e2-7574-4271-99c0-059f4d5563ee",
            DeviceGroupKey = "-X1RRYAQO"
        };

        public static BasicUserCredentials TestDev9Credentials = new BasicUserCredentials() {
            Username = "test_dev_9@shooterstech.net",
            Password = "abcd1234"
        };

        public static string TestDev1UserId = "5a90fe66-17a6-4d92-9bfa-f3c7a05e2b95";
        public static string TestDev3UserId = "11d535ed-5bc2-43be-ac94-6776111c0eec";
        public static string TestDev7UserId = "26f32227-d428-41f6-b224-beed7b6e8850";
        public static string TestDev9UserId = "28489692-0a61-470e-aed8-c71b9cfbfe6e";

        public static List<BasicUserCredentials> TestDevCredentialsList = new List<BasicUserCredentials>() {
            TestDev1Credentials, TestDev3Credentials, TestDev7Credentials, TestDev9Credentials
        };
    }

    public class BasicUserCredentials {
        public string Username { get; set;}
        public string Password { get; set;}
        public string DeviceKey { get;  set;}
        public string DeviceGroupKey { get; set;}
    }
}
