using System.Text.Json;
using Scopos.BabelFish.DataModel.Athena.Shot;

namespace Scopos.BabelFish.Tests.AthenaDataModel {

    [TestClass]
    public class LocationTests : BaseTestClass {



        [TestMethod]
        public void TestLocation() {
            //x, y, expected radial distance, expected angle, expected formatted cartesian coordinate, expected formatted string radial distance
            List<Tuple<float, float, float, float, string, string, string>> testCases = new List<Tuple<float, float, float, float, string, string, string>>();

            testCases.Add( new Tuple<float, float, float, float, string, string, string>( 1.00f, 1.00f, 1.41421f, .78539f, "1.00", "1.00", "1.41" ) );
            testCases.Add( new Tuple<float, float, float, float, string, string, string>( -1.00f, 1.00f, 1.41421f, 2.35619f, "-1.00", "1.00", "1.41" ) );
            testCases.Add( new Tuple<float, float, float, float, string, string, string>( -1.00f, -1.00f, 1.41421f, 3.92699f, "-1.00", "-1.00", "1.41" ) );
            testCases.Add( new Tuple<float, float, float, float, string, string, string>( 1.00f, -1.00f, 1.41421f, 5.49778f, "1.00", "-1.00", "1.41" ) );

            testCases.Add( new Tuple<float, float, float, float, string, string, string>( 1.00f, 0.00f, 1.0000f, 0f, "1.00", "0.00", "1.00" ) );
            testCases.Add( new Tuple<float, float, float, float, string, string, string>( 0.00f, 1.00f, 1.0000f, 1.57079f, "0.00", "1.00", "1.00" ) );
            testCases.Add( new Tuple<float, float, float, float, string, string, string>( -1.00f, 0.00f, 1.0000f, 3.14159f, "-1.00", "0.00", "1.00" ) );
            testCases.Add( new Tuple<float, float, float, float, string, string, string>( 0.00f, -1.00f, 1.0000f, 4.71239f, "0.00", "-1.00", "1.00" ) );

            testCases.Add( new Tuple<float, float, float, float, string, string, string>( 0.00f, 0.00f, 0f, 0f, "0.00", "0.00", "0.00" ) );

            foreach (var testCase in testCases) {
                Location l = new Location() {
                    X = testCase.Item1,
                    Y = testCase.Item2
                };

                Assert.IsTrue( Math.Abs( l.GetRadius() - testCase.Item3 ) < .001, $"Coordinates ({testCase.Item1}, {testCase.Item2}), expecting radius of {testCase.Item3}, instead got {l.GetRadius()}" );
                Assert.IsTrue( Math.Abs( l.GetAngle() - testCase.Item4 ) < .001, $"Coordinates ({testCase.Item1}, {testCase.Item2}), expecting angle of {testCase.Item4}, instead got {l.GetAngle()}" );

                Assert.AreEqual( l.GetXToString(), testCase.Item5, $"Coordinates ({testCase.Item1}, {testCase.Item2}), expecting X location of {testCase.Item5}, instead got {l.GetXToString()}" );
                Assert.AreEqual( l.GetYToString(), testCase.Item6, $"Coordinates ({testCase.Item1}, {testCase.Item2}), expecting X location of {testCase.Item6}, instead got {l.GetYToString()}" );
                Assert.AreEqual( l.GetRadiusToString(), testCase.Item7, $"Coordinates ({testCase.Item1}, {testCase.Item2}), expecting radius value of {testCase.Item7}, instead got {l.GetRadiusToString()}" );
            }
        }

        [TestMethod]
        public void EriksPlayground() {

            var json = "{\"Location\":{\"X\":-1.1258,\"Y\":-3.2211},\"TimeScored\":\"2024-05-30T17:14:34.212431-04:00\",\"RangeTime\":\"-28:53\",\"BulletDiameter\":4.5,\"ScoringDiameter\":4.5,\"Score\":{\"X\":0,\"D\":9.6,\"I\":9},\"TargetSetName\":\"v1.0:issf:10m Air Rifle\",\"FiringPoint\":\"2\",\"Attributes\":[\"SIMULATED\",\"FIRED AFTER COMMAND STOP\"],\"StageLabel\":\"S\",\"Sequence\":16.0,\"TargetName\":\"000001-Target-002\",\"ResultCOFID\":\"3d142c83-2387-49cb-86b7-09597d76b94a\",\"MatchID\":\"1.1.2024053010593480.0\",\"Meta\":{\"type\":\"NCShot\",\"VerImgBullXCoor\":400,\"VerImgBullYCoor\":400,\"VerImgShotXCoor\":395.56790515830664,\"VerImgShotYCoor\":412.6816486358357,\"VerImgDPMM\":3.9370078740157481,\"Override\":\"{}\"},\"EventName\":null,\"ScoreFormatted\":null,\"Update\":0,\"UpdateLog\":[],\"ValidationPhoto\":\"http://10.0.10.48/logs/images/1.1.2024053010593480.0/2024-05-30T17:14:34.212958.jpg\",\"Penalties\":[]}";

            var shot = JsonSerializer.Deserialize<Shot>( json, Scopos.BabelFish.Helpers.SerializerOptions.SystemTextJsonDeserializer );

            var foo = shot.GetVerificationImageAimingBullCoordinates();

	}
}
}
