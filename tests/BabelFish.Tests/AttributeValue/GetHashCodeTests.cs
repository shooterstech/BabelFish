using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.AttributeValue {
    [TestClass]
    public class GetHashCodeTests : BaseTestClass {

        [TestMethod]
        public async Task AttributeValueGetHashCodeTest() {
            SetName setName = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );
            var attributeValue1 = await DataModel.AttributeValue.AttributeValue.CreateAsync( setName );
            var attributeValue2 = await DataModel.AttributeValue.AttributeValue.CreateAsync( setName );

            //Should be equal because they are the same setName and have the same default field values.
            Assert.AreEqual( attributeValue1.GetHashCode(), attributeValue2.GetHashCode() );

            attributeValue1.SetFieldValue( "New Shooter" );
            Assert.AreNotEqual( attributeValue1.GetHashCode(), attributeValue2.GetHashCode() );

            attributeValue2.SetFieldValue( "New Shooter" );
            Assert.AreEqual( attributeValue1.GetHashCode(), attributeValue2.GetHashCode() );
        }

        [TestMethod]
        public async Task AttributeValueDataPacketMatchHashCodeTest() {
            SetName setName = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );
            var attributeValue1 = await DataModel.AttributeValue.AttributeValue.CreateAsync( setName );
            var attributeValue2 = await DataModel.AttributeValue.AttributeValue.CreateAsync( setName );
            var avdpm1 = await AttributeValueDataPacketMatch.CreateAsync( attributeValue1 );
            var avdpm2 = await AttributeValueDataPacketMatch.CreateAsync( attributeValue2 );

            //Should be equal because they are created from the same AttributeValue and have the same default field values.
            Assert.AreEqual( avdpm1.GetHashCode(), avdpm2.GetHashCode() );
            attributeValue1.SetFieldValue( "New Shooter" );
            Assert.AreNotEqual( avdpm1.GetHashCode(), avdpm2.GetHashCode() );
            attributeValue2.SetFieldValue( "New Shooter" );
            Assert.AreEqual( avdpm1.GetHashCode(), avdpm2.GetHashCode() );

            var one = 1;
            var two = 1;
            Console.WriteLine( one.GetHashCode() );
            Console.WriteLine( two.GetHashCode() );
        }

        [TestMethod]
        public async Task SetNameHashCodeTest() {
            SetName setName1 = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );
            SetName setName2 = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );

            Assert.AreEqual( setName1.GetHashCode(), setName2.GetHashCode() );

            SetName setName3 = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle 3x10" );
            Assert.AreNotEqual( setName1.GetHashCode(), setName3.GetHashCode() );
        }
    }
}
