using System.Threading.Tasks;
using BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataActors.AttributeFilter {

    [TestClass]
    public class AttributeFilterCalculatorTests : BaseTestClass {

        [TestMethod]
        public async Task SingleAttributeValue() {
            Participant participant = new Individual();
            SetName setName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );
            var attrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setName );
            attrValue.SetFieldValue( "Three-Position Air Rifle Type", "Sporter" );
            participant.AttributeValues.Add( new AttributeValueDataPacketMatch( attrValue ) );

            AttributeFilterAttributeValue sporterFilterHasOne = new AttributeFilterAttributeValue();
            sporterFilterHasOne.AttributeDef = setName;
            sporterFilterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            sporterFilterHasOne.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Sporter" ) );

            AttributeFilterAttributeValue precisionFilterHasOne = new AttributeFilterAttributeValue();
            precisionFilterHasOne.AttributeDef = setName;
            precisionFilterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            precisionFilterHasOne.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Precision" ) );

            AttributeFilterAttributeValue sporterFilterHasAll = new AttributeFilterAttributeValue();
            sporterFilterHasAll.AttributeDef = setName;
            sporterFilterHasAll.FilterRule = AttributeFilterRule.HAVE_ALL;
            sporterFilterHasAll.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Sporter" ) );

            AttributeFilterAttributeValue precisionFilterHasAll = new AttributeFilterAttributeValue();
            precisionFilterHasAll.AttributeDef = setName;
            precisionFilterHasAll.FilterRule = AttributeFilterRule.HAVE_ALL;
            precisionFilterHasAll.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Precision" ) );

            AttributeFilterAttributeValue sporterFilterHasNone = new AttributeFilterAttributeValue();
            sporterFilterHasNone.AttributeDef = setName;
            sporterFilterHasNone.FilterRule = AttributeFilterRule.NOT_HAVE_ANY;
            sporterFilterHasNone.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Sporter" ) );

            AttributeFilterAttributeValue precisionFilterHasNone = new AttributeFilterAttributeValue();
            precisionFilterHasNone.AttributeDef = setName;
            precisionFilterHasNone.FilterRule = AttributeFilterRule.NOT_HAVE_ANY;
            precisionFilterHasNone.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Precision" ) );

            AttributeFilterCalculator calculator = new AttributeFilterCalculator();

            Assert.IsTrue( calculator.Passes( sporterFilterHasOne, participant ) );
            Assert.IsFalse( calculator.Passes( precisionFilterHasOne, participant ) );
            Assert.IsTrue( calculator.Passes( sporterFilterHasAll, participant ) );
            Assert.IsFalse( calculator.Passes( precisionFilterHasAll, participant ) );
            Assert.IsFalse( calculator.Passes( sporterFilterHasNone, participant ) );
            Assert.IsTrue( calculator.Passes( precisionFilterHasNone, participant ) );
        }

        [TestMethod]
        public async Task MultipleAttributeValue() {
            Participant participant = new Individual();
            SetName setNameAirRifleType = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );
            SetName setNameNewShooter = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );
            var airRifleTypeAttrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setNameAirRifleType );
            airRifleTypeAttrValue.SetFieldValue( "Three-Position Air Rifle Type", "Sporter" );
            var newShooterAttrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setNameNewShooter );
            newShooterAttrValue.SetFieldValue( "Three-Position New Shooter", "New Shooter" );
            participant.AttributeValues.Add( new AttributeValueDataPacketMatch( airRifleTypeAttrValue ) );
            participant.AttributeValues.Add( new AttributeValueDataPacketMatch( newShooterAttrValue ) );

            AttributeFilterAttributeValue sporterFilterHasOne = new AttributeFilterAttributeValue();
            sporterFilterHasOne.AttributeDef = setNameAirRifleType;
            sporterFilterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            sporterFilterHasOne.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Sporter" ) );

            AttributeFilterAttributeValue precisionFilterHasOne = new AttributeFilterAttributeValue();
            precisionFilterHasOne.AttributeDef = setNameAirRifleType;
            precisionFilterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            precisionFilterHasOne.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Precision" ) );

            AttributeFilterAttributeValue newShooterHasOne = new AttributeFilterAttributeValue();
            newShooterHasOne.AttributeDef = setNameNewShooter;
            newShooterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            newShooterHasOne.Values.Add( new Tuple<string, dynamic>( "Three-Position New Shooter", "New Shooter" ) );

            AttributeFilterAttributeValue oldShooterHasOne = new AttributeFilterAttributeValue();
            oldShooterHasOne.AttributeDef = setNameNewShooter;
            oldShooterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            oldShooterHasOne.Values.Add( new Tuple<string, dynamic>( "Three-Position New Shooter", "Old Shooter" ) );

            AttributeFilterEquation sporterAndNewShooter = new AttributeFilterEquation();
            sporterAndNewShooter.Boolean = ShowWhenBoolean.AND;
            sporterAndNewShooter.Arguments.Add( sporterFilterHasOne );
            sporterAndNewShooter.Arguments.Add( newShooterHasOne );

            AttributeFilterEquation sporterOrNewShooter = new AttributeFilterEquation();
            sporterOrNewShooter.Boolean = ShowWhenBoolean.OR;
            sporterOrNewShooter.Arguments.Add( sporterFilterHasOne );
            sporterOrNewShooter.Arguments.Add( newShooterHasOne );

            AttributeFilterEquation sporterXorNewShooter = new AttributeFilterEquation();
            sporterXorNewShooter.Boolean = ShowWhenBoolean.XOR;
            sporterXorNewShooter.Arguments.Add( sporterFilterHasOne );
            sporterXorNewShooter.Arguments.Add( newShooterHasOne );

            AttributeFilterEquation precisionAndOldShooter = new AttributeFilterEquation();
            precisionAndOldShooter.Boolean = ShowWhenBoolean.AND;
            precisionAndOldShooter.Arguments.Add( precisionFilterHasOne );
            precisionAndOldShooter.Arguments.Add( oldShooterHasOne );

            AttributeFilterEquation precisionOrOldShooter = new AttributeFilterEquation();
            precisionOrOldShooter.Boolean = ShowWhenBoolean.OR;
            precisionOrOldShooter.Arguments.Add( precisionFilterHasOne );
            precisionOrOldShooter.Arguments.Add( oldShooterHasOne );

            AttributeFilterEquation sporterOrPrecision = new AttributeFilterEquation();
            sporterOrPrecision.Boolean = ShowWhenBoolean.OR;
            sporterOrPrecision.Arguments.Add( precisionFilterHasOne );
            sporterOrPrecision.Arguments.Add( sporterFilterHasOne );

            AttributeFilterCalculator calculator = new AttributeFilterCalculator();

            Assert.IsTrue( calculator.Passes( sporterAndNewShooter, participant ) );
            Assert.IsTrue( calculator.Passes( sporterOrNewShooter, participant ) );
            Assert.IsFalse( calculator.Passes( sporterXorNewShooter, participant ) );
            Assert.IsFalse( calculator.Passes( precisionAndOldShooter, participant ) );
            Assert.IsFalse( calculator.Passes( precisionOrOldShooter, participant ) );
            Assert.IsTrue( calculator.Passes( sporterOrPrecision, participant ) );
        }
    }
}
