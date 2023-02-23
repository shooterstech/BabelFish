using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition {

    [TestClass]
    public class SetNameTests {

        /// <summary>
        /// Tests that we can instantiate SetName objects with valid set name strings.
        /// </summary>
        [TestMethod]
        public void HappyPathInstantiation () {

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

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void BadSetNameFormatTwo() {

            var setName = SetName.Parse( "not a real set name" );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void BadSetNameFormatThree() {

            var setName = SetName.Parse( null );
        }

    }
}
