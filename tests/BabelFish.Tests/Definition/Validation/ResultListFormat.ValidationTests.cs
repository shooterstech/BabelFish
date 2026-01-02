using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition.Validation {
    [TestClass]
	public class ResultListFormatValidationTests : BaseTestClass{

		[TestMethod]
		public async Task HappyPathResultListFormatValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:orion:3P Qualification" );

			var candidate = (await client.GetResultListFormatDefinitionAsync( setName )).Value;

			var validation = new IsResultListFormatValid();

			var valid = await validation.IsSatisfiedByAsync( candidate );

			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
        }

        [TestMethod]
        public async Task SpanningColumnHappyPath() {

            var spanningColumnsSpecification = new IsResultListFormatHaveAtMostOneSpanningColumn();

            //Define a RESULT LIST FORMAT, initially without any spanning text
            var RLF = new ResultListFormat();
            var columns = RLF.Format.Columns;

            columns.Add( new ResultListDisplayColumn() {
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Rank}"
                    },
                    new ResultListCellValue() {
                        Text = "{RankDelta}"
                    }
                }
            } );

            columns.Add( new ResultListDisplayColumn() {
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{DisplayName}"
                    },
                    new ResultListCellValue() {
                        Text = "{Hometown}"
                    }
                }
            } );

            columns.Add( new ResultListDisplayColumn() {
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Aggregate}"
                    }
                }
            } );

            //Now test it
            var zeroColumnTest = await spanningColumnsSpecification.IsSatisfiedByAsync( RLF );
            Assert.IsTrue( zeroColumnTest );

            //Modify one of the columns to have Spanning text
            columns[1].Spanning = new ResultListCellValue() {
                Text = "{CompetitorNumber}"
            };

            //Test Again. Should still be OK. 
            var oneColumnTest = await spanningColumnsSpecification.IsSatisfiedByAsync( RLF );
            Assert.IsTrue( oneColumnTest );

            //Add a second column with Spanning Text. 
            columns[2].Spanning = new ResultListCellValue() {
                Text = "Blah blah blah"
            };

            //This should fail.
            var twoColumnTest = await spanningColumnsSpecification.IsSatisfiedByAsync( RLF );
            Assert.IsFalse( twoColumnTest );
        }
    }
}
