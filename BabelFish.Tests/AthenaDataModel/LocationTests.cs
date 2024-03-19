using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.DataModel.Athena.Shot;

namespace Scopos.BabelFish.Tests.AthenaDataModel {

    [TestClass]
    public class LocationTests {

        

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

            foreach ( var testCase in  testCases ) {
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
    }
}
