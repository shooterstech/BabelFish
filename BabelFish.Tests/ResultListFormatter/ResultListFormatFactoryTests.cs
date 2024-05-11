using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scopos.BabelFish.Tests.ResultListFormatter {
    [TestClass]
    public class ResultListFormatFactoryTests {

        [TestInitialize]
        public void InitializeTest() {

            var rlf = ResultListFormatFactory.FACTORY;

            
        }

        [TestMethod]
        [Ignore]
        public void ReadingResultListFormatFiles() {
            var rlf = ResultListFormatFactory.FACTORY;
            /* Ignoring as these tests may no longer be valid
            dynamic inv3PFormat = rlf.GetResultListFormatSetName( "v2.0:ntparc:Three-Position Air Rifle 3x10", "Individual" );

            //Test that with a real COF and Event, we get back the expected Result List Format definition
            Assert.AreEqual( "v1.0:orion:Default Individual", inv3PFormat.ToString() );

            //Test that with either a phoney COF or a phony Event, we get back the default ... and no exceptions are thrown.
            var notARealCOF = rlf.GetResultListFormatSetName( "Not a real course of fire", "Individual" );
            var notARealEvent = rlf.GetResultListFormatSetName( "v2.0:ntparc:Three-Position Air Rifle 3x10", "Not a real event" );

            Assert.AreEqual( "v1.0:orion:Not a real event Individual", notARealEvent.ToString() );
            Assert.AreEqual( "v1.0:orion:Default Individual", notARealCOF.ToString() );
            */
        }
    }
}
