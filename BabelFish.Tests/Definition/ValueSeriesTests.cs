using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition {

    [TestClass]
    public class ValueSeriesTests : BaseTestClass {

        /// <summary>
        /// Tests corrrectly formatted ValueSeries
        /// </summary>
        [TestMethod]
        public void ValueSeriesHappyPathTests() {

            var vs1 = new ValueSeries( "1..10" );
            Assert.AreEqual( 1, vs1.StartValue );
            Assert.AreEqual( 10, vs1.EndValue );
            Assert.AreEqual( 1, vs1.Step );
            var intList = vs1.GetAsList();
            var strList = vs1.GetAsList( "P{}" );
            int index = 0;
            for (int i = 1; i <= 10; i++) {
                Assert.AreEqual( i, intList[index] );
                Assert.AreEqual( $"P{i}", strList[index] );
                index++;
            }

            var vs2 = new ValueSeries( "10..1" );
            Assert.AreEqual( 10, vs2.StartValue );
            Assert.AreEqual( 1, vs2.EndValue );
            Assert.AreEqual( 1, vs2.Step );
            intList = vs2.GetAsList();
            strList = vs2.GetAsList( "K{}" );
            index = 0;
            for (int i = 10; i > 0; i--) {
                Assert.AreEqual( i, intList[index] );
                Assert.AreEqual( $"K{i}", strList[index] );
                index++;
            }

            var vs3 = new ValueSeries( "1..10,2" );
            Assert.AreEqual( 1, vs3.StartValue );
            Assert.AreEqual( 10, vs3.EndValue );
            Assert.AreEqual( 2, vs3.Step );
            intList = vs3.GetAsList();
            strList = vs3.GetAsList( "S{}" );
            index = 0;
            for (int i = 1; i <= 10; i += 2) {
                Assert.AreEqual( i, intList[index] );
                Assert.AreEqual( $"S{i}", strList[index] );
                index++;
            }

            //The negative, part of the step value is ignored.
            var vs4 = new ValueSeries( "1..10,-2" );
            Assert.AreEqual( 1, vs4.StartValue );
            Assert.AreEqual( 10, vs4.EndValue );
            Assert.AreEqual( 2, vs4.Step );
            intList = vs4.GetAsList();
            strList = vs4.GetAsList( "PR {}" );
            index = 0;
            for (int i = 1; i <= 10; i += 2) {
                Assert.AreEqual( i, intList[index] );
                Assert.AreEqual( $"PR {i}", strList[index] );
                index++;
            }

            var vs5 = new ValueSeries( "10..1,2" );
            Assert.AreEqual( 10, vs5.StartValue );
            Assert.AreEqual( 1, vs5.EndValue );
            Assert.AreEqual( 2, vs5.Step );
            intList = vs5.GetAsList();
            strList = vs5.GetAsList( "ST {}" );
            index = 0;
            for (int i = 10; i > 0; i -= 2) {
                Assert.AreEqual( i, intList[index] );
                Assert.AreEqual( $"ST {i}", strList[index] );
                index++;
            }

            var vs6 = new ValueSeries( ValueSeries.APPLY_TO_ALL_FORMAT );
            Assert.AreEqual( 1, vs6.StartValue );
            Assert.AreEqual( int.MaxValue, vs6.EndValue );
            Assert.AreEqual( 1, vs6.Step );
        }

        /// <summary>
        /// Tests incorrrectly formatted ValueSeries
        /// </summary>
        [TestMethod]
        public void ValueSeriesSadPathTests() {

            //Each of these shouldn't parse, and thus return a default Value Series.

            var vs1 = new ValueSeries( "abcdefg" );
            Assert.AreEqual( 1, vs1.StartValue );
            Assert.AreEqual( 1, vs1.EndValue );
            Assert.AreEqual( 1, vs1.Step );

            var vs2 = new ValueSeries( "1-2-3" );
            Assert.AreEqual( 1, vs2.StartValue );
            Assert.AreEqual( 1, vs2.EndValue );
            Assert.AreEqual( 1, vs2.Step );

            var vs3 = new ValueSeries( "abcd...123" );
            Assert.AreEqual( 1, vs3.StartValue );
            Assert.AreEqual( 1, vs3.EndValue );
            Assert.AreEqual( 1, vs3.Step );

            var vs7 = new ValueSeries( string.Empty );
            Assert.AreEqual( 1, vs7.StartValue );
            Assert.AreEqual( 1, vs7.EndValue );
            Assert.AreEqual( 1, vs7.Step );

            var vs8 = new ValueSeries( null );
            Assert.AreEqual( 1, vs8.StartValue );
            Assert.AreEqual( 1, vs8.EndValue );
            Assert.AreEqual( 1, vs8.Step );
        }
    }
}
