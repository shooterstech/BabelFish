using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests {
    [TestClass]
    public class DefinitionCacheTests
    {


        // DefinitionCacheHelper defaults to Orion dir if exists, then fails over to user's temp dir
        private static string definitionDir = "\\DEFINITIONS";
        private static string orionDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\My Matches";
        private static string profileDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\appdata\\local\\temp";
        private static string executeDirectory = $"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}";

        private static string mockDirFile = "\\v1.0 orion Date of Birth.json";
        private static string mockData = "{\"Title\":\"\",\"Message\":[],\"ResponseCodes\":[],\"v1.0:orion:Date of Birth\":{\"Discontinued\":false,\"MaxVisibility\":\"PROTECTED\",\"MultipleValues\":false,\"Fields\":[{\"Validation\":{\"MinValue\":\"1900-01-01\",\"MaxValue\":\"2018-01-01\",\"ValidationErrorList\":[],\"ErrorMessage\":\"DateOfBirthisoutsidetheallowablerange.\"},\"DefaultValue\":\"2018-01-01\",\"Required\":true,\"ValueType\":\"DATE\",\"DisplayName\":\"Date of Birth\",\"MultipleValues\":false,\"FieldName\":\"DateOfBirth\",\"FieldType\":\"OPEN\"}],\"Description\":\"A users date of birth\",\"DisplayName\":\"ProfileName\",\"SetName\":\"v1.0:orion:Date of Birth\",\"Type\":\"ATTRIBUTE\",\"HierarchicalName\":\"orion:Date of Birth\",\"Owner\":\"OrionAcct000001\",\"Version\":\"1.2\",\"Designation\":[\"ATHLETE\",\"CLUB\",\"MATCHOFFICIAL\",\"TEAM\",\"TEAMOFFICIAL\",\"USER\",\"USER\",\"ATHLETE\",\"HIDDEN\"]}}";

        private static string defaultDir = "";

        /// <summary>
        /// Mimic DefinitionCacheHelper order of directory selection, with override for tests
        /// </summary>
        /// <param name="overrideTest"></param>
        /// <returns></returns>
        private string determineDefaultDir(string overrideTest = "")
        {
            string returnDir = overrideTest;

            // 1st default to Orion user directory, 2nd iuser temp directory, fail over to installation directory
            if (System.IO.Directory.Exists(orionDirectory) && 
                    (overrideTest != "" && orionDirectory == overrideTest))
                returnDir = $"{orionDirectory}";
            else if (System.IO.Directory.Exists(profileDirectory) &&
                     (overrideTest != "" && profileDirectory == overrideTest))
                returnDir = $"{profileDirectory}";
            else if (overrideTest == "" || executeDirectory == overrideTest)
                returnDir = $"{executeDirectory}";

            if (!System.IO.Directory.Exists($"{returnDir}{definitionDir}"))
                System.IO.Directory.CreateDirectory($"{returnDir}{definitionDir}");

            return returnDir;
        }

        [TestMethod]
        public void NoCurrentParametersSet()
        {
                DefinitionCacheHelper definitionCacheHelperNull = new DefinitionCacheHelper();
                Assert.IsNull(definitionCacheHelperNull.CurrentSetName);
                Assert.IsNull(definitionCacheHelperNull.CurrentType);
        }

        [TestMethod]
        public void DefaultDirectoryComputedAndSet()
        {
            DefinitionCacheHelper definitionCacheHelperNull = new DefinitionCacheHelper();
            Assert.IsTrue(definitionCacheHelperNull.DefaultDirectory != string.Empty);
        }

        [TestMethod]
        public void DefaultDirectoryComputedSuccessfully()
        {
            defaultDir = determineDefaultDir();
            Assert.IsFalse(string.IsNullOrEmpty(defaultDir));
        }

        [TestMethod]
        public void DefinitionCacheAgeValueRetrieved()
        {
            defaultDir = determineDefaultDir();

            System.IO.File.WriteAllText($"{defaultDir}{definitionDir}{mockDirFile}", mockData);
            System.IO.File.SetLastWriteTime($"{defaultDir}{definitionDir}{mockDirFile}", DateTime.Now.AddDays(-5));
            Assert.IsTrue(System.IO.File.Exists($"{defaultDir}{definitionDir}{mockDirFile}"));

            Dictionary<string, string> clientParams = new Dictionary<string, string>()
            {
                {"Definitions_CacheIgnore", "false"},
                {"Definitions_CacheStorageDirctory", defaultDir },
            };
            DefinitionAPIClient _client = new DefinitionAPIClient( Constants.X_API_KEY, clientParams);

            DefinitionCacheHelper definitionCacheHelperDays = new DefinitionCacheHelper();
            Assert.AreEqual(definitionCacheHelperDays.DefinitionCacheAgeInDays(DefinitionType.ATTRIBUTE, SetName.Parse("v1.0:orion:Date of Birth")),5);

            System.IO.File.Delete($"{defaultDir}{definitionDir}{mockDirFile}");
        }

        [TestMethod]
        public void ReturnSpecifiedDefinitionFromFileCache()
        {
            defaultDir = determineDefaultDir();

            System.IO.File.WriteAllText($"{defaultDir}{definitionDir}{mockDirFile}", mockData);
            Assert.IsTrue(System.IO.File.Exists($"{defaultDir}{definitionDir}{mockDirFile}"));

            Dictionary<string, string> clientParams = new Dictionary<string, string>()
            {
                {"Definitions_CacheIgnore", "false"},
                {"Definitions_CacheStorageDirctory", defaultDir },
            };
            DefinitionAPIClient _client = new DefinitionAPIClient( Constants.X_API_KEY, clientParams);

            var setName = SetName.Parse("v1.0:orion:Date of Birth");
            string filename = setName.FileName;
            string heirarchyName = setName.ToHierarchicalNameString();
            string fullname = setName.ToString();
            var response = _client.GetAttributeDefinitionAsync(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.AreEqual(objResponse.Fields.Count, 1);
            Assert.AreEqual(objResponse.Fields[0].FieldName, "DateOfBirth");

            System.IO.File.Delete($"{defaultDir}{definitionDir}{mockDirFile}");
        }

        [TestMethod]
        public void DefinitionSavedToDefaultCacheDirectory()
        {
            var setName = SetName.Parse("v1.0:orion:Profile Name");
            System.IO.File.Delete($"{defaultDir}{definitionDir}\\{setName.FileName}");

            defaultDir = determineDefaultDir();

            Dictionary<string, string> clientParams = new Dictionary<string, string>()
            {
                {"UserName", "test_dev_7@Shooterstech.net"},
                {"PassWord", "abcd1234"},
                {"Definitions_CacheIgnore", "false"},
                {"Definitions_CacheExpireTime", "10"},
                {"Definitions_CacheStorageDirctory", defaultDir },
            };
            DefinitionAPIClient _client = new DefinitionAPIClient( Constants.X_API_KEY, clientParams);

            var response = _client.GetAttributeDefinitionAsync(setName);
            Assert.IsNotNull(response);

            Assert.IsTrue(System.IO.File.Exists($"{defaultDir}{definitionDir}\\{setName.FileName}"));
        }

        [TestMethod]
        public void CourseOfFireTypeCache()
        {
            defaultDir = determineDefaultDir();

            var setName = SetName.Parse("v2.0:ntparc:Three-Position Air Rifle 3x10");
            DefinitionAPIClient _client = new DefinitionAPIClient( Constants.X_API_KEY );

            var response = _client.GetCourseOfFireDefinition(setName);
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.Definition;
            var msgResponse = taskResult.MessageResponse;

            Assert.IsNotNull(objResponse);
            Assert.IsNotNull(msgResponse);

            Assert.AreEqual(objResponse.SetName, setName.ToString());
            Assert.AreEqual(objResponse.Type, taskResult.DefinitionType);
            Assert.IsTrue(objResponse.RangeScripts.Count >= 1);
            Assert.AreEqual(objResponse.Description, setName.ProperName);

            Assert.IsTrue(System.IO.File.Exists($"{defaultDir}{definitionDir}\\{setName.FileName}"));
        }

        [TestMethod]
        public void CompareTimeToRun()
        {
            defaultDir = determineDefaultDir();

            // Run memory pull first just to prove the point
            System.IO.File.WriteAllText($"{defaultDir}{definitionDir}{mockDirFile}", mockData);
            Assert.IsTrue(System.IO.File.Exists($"{defaultDir}{definitionDir}{mockDirFile}"));
            System.Threading.Thread.Sleep(1000);

            Dictionary<string, string> clientParams = new Dictionary<string, string>()
            {
                {"Definitions_CacheIgnore", "false"},
                {"Definitions_CacheStorageDirctory", defaultDir },
            };
            DefinitionAPIClient _client = new DefinitionAPIClient( Constants.X_API_KEY, clientParams);

            var setName = SetName.Parse("v1.0:orion:Date of Birth");
            var response = _client.GetAttributeDefinitionAsync(setName);
            Assert.IsNotNull(response);
            var taskResult = response.Result;
            var runTime1 = taskResult.TimeToRun;

            System.IO.File.Delete($"{defaultDir}{definitionDir}{mockDirFile}");
            System.Threading.Thread.Sleep(1000);

            // Run server pull second
            DefinitionAPIClient _client2 = new DefinitionAPIClient( Constants.X_API_KEY, clientParams);
            var setName2 = SetName.Parse("v1.0:orion:Date of Birth");
            var response2 = _client2.GetAttributeDefinitionAsync(setName2);
            Assert.IsNotNull(response2);
            var taskResult2 = response2.Result;
            var runTime2 = taskResult2.TimeToRun;

            Assert.IsTrue(runTime2 != runTime1);
        }

    }
}
