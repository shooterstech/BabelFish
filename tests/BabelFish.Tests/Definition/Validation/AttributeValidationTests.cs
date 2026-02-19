using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.Definitions;
using Attribute = Scopos.BabelFish.DataModel.Definitions.Attribute;

namespace Scopos.BabelFish.Tests.Definition.Validation {
    [TestClass]
    public class AttributeValidationTests : BaseTestClass {

        [TestMethod]
        public async Task HappyPathAttributeValid() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );

            var candidate = (await client.GetAttributeDefinitionAsync( setName )).Value;

            var validation = new IsAttributeValid();

            var valid = await validation.IsSatisfiedByAsync( candidate );

            Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
        }

        [TestMethod]
        public async Task DefaultFieldValueTests() {
            var fieldTypeClosed = new AttributeFieldString() {
                FieldType = FieldType.CLOSED,
                DefaultValue = "AAAA"
            };

            var fieldTypeOpen = new AttributeFieldString() {
                FieldType = FieldType.OPEN,
                DefaultValue = "AAAA"
            };

            var fieldTypeSuggest = new AttributeFieldString() {
                FieldType = FieldType.SUGGEST,
                DefaultValue = "AAAA"
            };

            fieldTypeClosed.Values.Add( new AttributeValueOption<string>() { Name = "AAAA", Value = "AAAA" } );
            fieldTypeClosed.Values.Add( new AttributeValueOption<string>() { Name = "BBBB", Value = "BBBB" } );
            fieldTypeClosed.Values.Add( new AttributeValueOption<string>() { Name = "CCCC", Value = "CCCC" } );

            fieldTypeSuggest.Values.Add( new AttributeValueOption<string>() { Name = "AAAA", Value = "AAAA" } );
            fieldTypeSuggest.Values.Add( new AttributeValueOption<string>() { Name = "BBBB", Value = "BBBB" } );
            fieldTypeSuggest.Values.Add( new AttributeValueOption<string>() { Name = "CCCC", Value = "CCCC" } );

            var specification = new IsAttributeFieldDefaultValueValid();

            var attr1 = new Attribute();
            attr1.Fields.Add( fieldTypeClosed );
            fieldTypeClosed.DefaultValue = "AAAA";
            Assert.IsTrue( await specification.IsSatisfiedByAsync( attr1 ), "Closed Field with a Default Value that's in the .Values list. Should pass." );

            fieldTypeClosed.DefaultValue = "DDDD";
            Assert.IsFalse( await specification.IsSatisfiedByAsync( attr1 ), "Closed Field with a Default Value that's not in the .Values list. Should fail." );

            var attr2 = new Attribute();
            attr2.Fields.Add( fieldTypeOpen );
            fieldTypeClosed.DefaultValue = "AAAA";
            Assert.IsTrue( await specification.IsSatisfiedByAsync( attr2 ), "Open Field with a Default Value. Should always pass regardless of value." );

            fieldTypeClosed.DefaultValue = "DDDD";
            Assert.IsTrue( await specification.IsSatisfiedByAsync( attr2 ), "Open Field with a Default Value. Should always pass regardless of value." );

            var attr3 = new Attribute();
            attr3.Fields.Add( fieldTypeOpen );
            fieldTypeClosed.DefaultValue = "AAAA";
            Assert.IsTrue( await specification.IsSatisfiedByAsync( attr3 ), "Suggest Field with a Default Value in the list. Should always pass regardless of value." );

            fieldTypeClosed.DefaultValue = "DDDD";
            Assert.IsTrue( await specification.IsSatisfiedByAsync( attr3 ), "Suggest Field with a Default Value not in the list. Should always pass regardless of value." );
        }
    }
}
