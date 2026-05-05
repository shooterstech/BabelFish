using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.DataModel.DefinitionTests {

    [TestClass]
    public class SetNameTests : BaseTestClass {

        /// <summary>
        /// Tests that we can instantiate SetName objects with valid set name strings.
        /// </summary>
        [TestMethod]
        public void HappyPathInstantiation() {

            var aStr = "v1.2:orion:Profile Name";
            var a = SetName.Parse( aStr );
            Assert.IsNotNull( a );
            Assert.AreEqual( 1, a.MajorVersion );
            Assert.AreEqual( 2, a.MinorVersion );
            Assert.AreEqual( "orion", a.Namespace );
            Assert.AreEqual( "Profile Name", a.ProperName );
            Assert.AreEqual( aStr, a.ToString() );
            Assert.AreEqual( "v1.0:orion:Profile Name", a.ToMostRecentMajorVersionString() );
            Assert.AreEqual( "v0.0:orion:Profile Name", a.ToMostRecentString() );

            var b = new SetName( "orion", "Email Address", 2 );
            Assert.IsNotNull( b );
            Assert.AreEqual( 2, b.MajorVersion );
            Assert.AreEqual( 0, b.MinorVersion );
            Assert.AreEqual( "orion", b.Namespace );
            Assert.AreEqual( "Email Address", b.ProperName );
            Assert.AreEqual( "v2.0:orion:Email Address", b.ToString() );
            Assert.AreEqual( "v2.0:orion:Email Address", b.ToMostRecentMajorVersionString() );
            Assert.AreEqual( "v0.0:orion:Email Address", b.ToMostRecentString() );

            var c = new SetName( "orion", "Test Attribute", 2, 3 );
            Assert.IsNotNull( c );
            Assert.AreEqual( 2, c.MajorVersion );
            Assert.AreEqual( 3, c.MinorVersion );
            Assert.AreEqual( "orion", c.Namespace );
            Assert.AreEqual( "Test Attribute", c.ProperName );
            Assert.AreEqual( "v2.3:orion:Test Attribute", c.ToString() );
            Assert.AreEqual( "v2.0:orion:Test Attribute", c.ToMostRecentMajorVersionString() );
            Assert.AreEqual( "v0.0:orion:Test Attribute", c.ToMostRecentString() );
        }

        /// <summary>
        /// Tests that the try parse methods return false if the passed in set names are not in the correct format.
        /// </summary>
        [TestMethod]
        public void BadSetNameFormatOne() {

            string aStr = "not a real set name";
            string bStr = "";
            string cStr = null;
            string dStr = "orion:Profile Name"; //This is a hierachiable name

            var setName = new SetName();
            Assert.IsFalse( SetName.TryParse( aStr, out setName ) );
            Assert.IsFalse( SetName.TryParse( bStr, out setName ) );
            Assert.IsFalse( SetName.TryParse( cStr, out setName ) );
            Assert.IsFalse( SetName.TryParse( dStr, out setName ) );
        }

        /// <summary>
        /// Tests that the parse method throws an exception if the passed in set name is not in the correct format and that the exception is of the correct type.
        /// </summary>
        [TestMethod]
        public void BadSetNameFormatTwo() {

            Assert.Throws<ArgumentException>( () => {

                var setName = SetName.Parse( "not a real set name", true );
            } );
        }

        /// <summary>
        /// Tests that the SetName.Parse method throws an ArgumentNullException when a null value is provided as input.
        /// </summary>
        [TestMethod]
        public void BadSetNameFormatThree() {

            Assert.Throws<ArgumentException>( () => {
                var setName = SetName.Parse( null, true );
            } );
        }

        /// <summary>
        /// Tests that the SetName.Parse method returns the default set name when provided with an invalid set name
        /// string, and throwExceptionOnError is set to false (the default).
        /// </summary>
        [TestMethod]
        public void BadSetNameFormatFour() {

            var setName = SetName.Parse( "not a real set name", false );
            Assert.IsTrue( setName.IsDefault );
        }

        /// <summary>
        /// Tests that the SetName.Parse method returns the default set name when provided with a null value
        /// and throwExceptionOnError is set to false (the default).
        /// </summary>
        [TestMethod]
        public void BadSetNameFormatFive() {

            var setName = SetName.Parse( null, false );
            Assert.IsTrue( setName.IsDefault );
        }

        [TestMethod]
        public void ParsingHierarchicalNameTests() {

            var hnString = "ntparc:Three-Position Air Rifle Type";

            HierarchicalName hn;
            var parseSuccessful = HierarchicalName.TryParse( hnString, out hn );

            Assert.IsTrue( parseSuccessful );
            Assert.AreEqual( "ntparc", hn.Namespace );
            Assert.AreEqual( "Three-Position Air Rifle Type", hn.ProperName );

        }

        [TestMethod]
        /// <summary>
        /// Tests that we can cast SetName objects to strings and that the resulting string is in the correct format.
        /// </summary>
        public void SetNameCastingTests() {
            SetName sn = SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" );

            // Implicit cast
            string snAsString = sn;

            // Explicit cast
            string snAsString2 = (string)sn;

            Assert.AreEqual( "v1.0:ntparc:Three-Position Sporter Air Rifle", snAsString );

            Assert.AreEqual( "v1.0:ntparc:Three-Position Sporter Air Rifle", snAsString2 );
        }

        [TestMethod]
        public void SetNameEqualityTests() {
            var sn1 = SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" );
            var sn2 = SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" );
            var sn3 = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle" );
            Assert.IsTrue( sn1.Equals( sn2 ) );
            Assert.IsFalse( sn1.Equals( sn3 ) );
            Assert.IsTrue( sn1.Equals( "v1.0:ntparc:Three-Position Sporter Air Rifle" ) );
            Assert.IsFalse( sn3.Equals( "v1.0:ntparc:Three-Position Sporter Air Rifle" ) );
        }
    }
}
