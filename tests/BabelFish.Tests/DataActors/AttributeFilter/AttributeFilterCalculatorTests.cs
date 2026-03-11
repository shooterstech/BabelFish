using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataActors.AttributeFilter {

    [TestClass]
    public class AttributeFilterCalculatorTests : BaseTestClass {

        /// <summary>
        /// Tests the AttributeFilterCalculator with a single attribute value on the Participant and various filter rules (HAVE_ONE, HAVE_ALL, NOT_HAVE_ANY),
        /// with each filter only have one value that either matches or does not match the Participant's attribute value.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task SingleAttributeValue() {
            Participant participant = new Individual();
            SetName setName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );

            // Create a Participant with the "Three-Position Air Rifle Type" attribute set to "Sporter"
            var attrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setName );
            attrValue.SetFieldValue( "Three-Position Air Rifle Type", "Sporter" );
            participant.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( attrValue ) );

            // Create some Attribute Values to test against 
            var sporterAttrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setName );
            sporterAttrValue.SetFieldValue( "Three-Position Air Rifle Type", "Sporter" );
            var dataPacketMatchSporter = await AttributeValueDataPacketMatch.CreateAsync( sporterAttrValue );

            var precisionAttrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setName );
            precisionAttrValue.SetFieldValue( "Three-Position Air Rifle Type", "Precision" );
            var dataPacketMatchPrecision = await AttributeValueDataPacketMatch.CreateAsync( precisionAttrValue );


            AttributeFilterAttributeValue sporterFilterHasOne = new AttributeFilterAttributeValue();
            sporterFilterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            sporterFilterHasOne.Values.Add( dataPacketMatchSporter );

            AttributeFilterAttributeValue precisionFilterHasOne = new AttributeFilterAttributeValue();
            precisionFilterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            precisionFilterHasOne.Values.Add( dataPacketMatchPrecision );

            AttributeFilterAttributeValue sporterFilterHasAll = new AttributeFilterAttributeValue();
            sporterFilterHasAll.FilterRule = AttributeFilterRule.HAVE_ALL;
            sporterFilterHasAll.Values.Add( dataPacketMatchSporter );

            AttributeFilterAttributeValue precisionFilterHasAll = new AttributeFilterAttributeValue();
            precisionFilterHasAll.FilterRule = AttributeFilterRule.HAVE_ALL;
            precisionFilterHasAll.Values.Add( dataPacketMatchPrecision );

            AttributeFilterAttributeValue sporterFilterHasNone = new AttributeFilterAttributeValue();
            sporterFilterHasNone.FilterRule = AttributeFilterRule.NOT_HAVE_ANY;
            sporterFilterHasNone.Values.Add( dataPacketMatchSporter );

            AttributeFilterAttributeValue precisionFilterHasNone = new AttributeFilterAttributeValue();
            precisionFilterHasNone.FilterRule = AttributeFilterRule.NOT_HAVE_ANY;
            precisionFilterHasNone.Values.Add( dataPacketMatchPrecision );


            Assert.IsTrue( AttributeFilterCalculator.Passes( sporterFilterHasOne, participant ) );
            Assert.IsFalse( AttributeFilterCalculator.Passes( precisionFilterHasOne, participant ) );
            Assert.IsTrue( AttributeFilterCalculator.Passes( sporterFilterHasAll, participant ) );
            Assert.IsFalse( AttributeFilterCalculator.Passes( precisionFilterHasAll, participant ) );
            Assert.IsFalse( AttributeFilterCalculator.Passes( sporterFilterHasNone, participant ) );
            Assert.IsTrue( AttributeFilterCalculator.Passes( precisionFilterHasNone, participant ) );
        }

        [TestMethod]
        public async Task MultipleAttributeValue() {
            Participant participant = new Individual();
            SetName setNameAirRifleType = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );
            SetName setNameNewShooter = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );

            // Create a Participant with the "Three-Position Air Rifle Type" attribute set to "Sporter" and Net Shooter set to "New Shooter"
            var airRifleTypeAttrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setNameAirRifleType );
            airRifleTypeAttrValue.SetFieldValue( "Three-Position Air Rifle Type", "Sporter" );
            var nsAttrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setNameNewShooter );
            nsAttrValue.SetFieldValue( "Three-Position New Shooter", "New Shooter" );
            participant.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( airRifleTypeAttrValue ) );
            participant.AttributeValues.Add( await AttributeValueDataPacketMatch.CreateAsync( nsAttrValue ) );

            // Create some Attribute Values to test against. 
            var sporterAttrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setNameAirRifleType );
            sporterAttrValue.SetFieldValue( "Three-Position Air Rifle Type", "Sporter" );
            var dataPacketMatchSporter = await AttributeValueDataPacketMatch.CreateAsync( sporterAttrValue );

            var precisionAttrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setNameAirRifleType );
            precisionAttrValue.SetFieldValue( "Three-Position Air Rifle Type", "Precision" );
            var dataPacketMatchPrecision = await AttributeValueDataPacketMatch.CreateAsync( precisionAttrValue );

            var newShooterAttrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setNameNewShooter );
            newShooterAttrValue.SetFieldValue( "Three-Position New Shooter", "New Shooter" );
            var dataPacketNewShooter = await AttributeValueDataPacketMatch.CreateAsync( newShooterAttrValue );

            var oldShooterAttrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setNameNewShooter );
            oldShooterAttrValue.SetFieldValue( "Three-Position New Shooter", "Old Shooter" );
            var dataPacketOldShooter = await AttributeValueDataPacketMatch.CreateAsync( oldShooterAttrValue );



            AttributeFilterAttributeValue sporterFilterHasOne = new AttributeFilterAttributeValue();
            sporterFilterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            sporterFilterHasOne.Values.Add( dataPacketMatchSporter );

            AttributeFilterAttributeValue precisionFilterHasOne = new AttributeFilterAttributeValue();
            precisionFilterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            precisionFilterHasOne.Values.Add( dataPacketMatchPrecision );

            AttributeFilterAttributeValue newShooterHasOne = new AttributeFilterAttributeValue();
            newShooterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            newShooterHasOne.Values.Add( dataPacketNewShooter );

            AttributeFilterAttributeValue oldShooterHasOne = new AttributeFilterAttributeValue();
            oldShooterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            oldShooterHasOne.Values.Add( dataPacketOldShooter );

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

            Assert.IsTrue( AttributeFilterCalculator.Passes( sporterAndNewShooter, participant ) );
            Assert.IsTrue( AttributeFilterCalculator.Passes( sporterOrNewShooter, participant ) );
            Assert.IsFalse( AttributeFilterCalculator.Passes( sporterXorNewShooter, participant ) );
            Assert.IsFalse( AttributeFilterCalculator.Passes( precisionAndOldShooter, participant ) );
            Assert.IsFalse( AttributeFilterCalculator.Passes( precisionOrOldShooter, participant ) );
            Assert.IsTrue( AttributeFilterCalculator.Passes( sporterOrPrecision, participant ) );
        }
    }
}
