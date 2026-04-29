using Scopos.BabelFish.DataActors.PDF;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatchTests {

    [TestClass]
    public class BarcodeLabelTests : BaseTestClass {

        /// <summary>
        /// Tests that the BarcodeLabelEncoding class correctly encodes and decodes the competitor number,
        /// course of fire ID, stage label, and series into a string format, and that the decoded
        /// values match the original input values.
        /// </summary>
        [TestMethod]
        public void BarcodeLabelEncodingTests() {

            var barcodeLabel = new BarcodeLabelEncoding {
                CompetitorNumber = "1234",
                CourseOfFireId = 2,
                StageLabel = "S",
                Series = 4
            };

            Assert.AreEqual( "  123402 S04", barcodeLabel.Encode );

            var barcodeLabel2 = BarcodeLabelEncoding.Decode( barcodeLabel.Encode );
            Assert.AreEqual( barcodeLabel.CompetitorNumber, barcodeLabel2.CompetitorNumber );
            Assert.AreEqual( barcodeLabel.CourseOfFireId, barcodeLabel2.CourseOfFireId );
            Assert.AreEqual( barcodeLabel.StageLabel, barcodeLabel2.StageLabel );
            Assert.AreEqual( barcodeLabel.Series, barcodeLabel2.Series );
        }

        [TestMethod]
        public void PdfTest() {
            List<BarcodeLabelEncoding> barcodes = new List<BarcodeLabelEncoding>();
            for (int i = 0; i < 15; i++) {
                barcodes.Add( new BarcodeLabelEncoding {
                    DisplayName = "John Doe",
                    TeamName = "Team A",
                    Club = "Org X",
                    Squadding = "Relay 1 FP 2",
                    CompetitorNumber = "1234",
                    CourseOfFireId = 2,
                    StageLabel = "S",
                    Series = 4
                } );
                barcodes.Add( new BarcodeLabelEncoding {
                    DisplayName = "Jane Smith",
                    TeamName = "Team with a really long name because reasons",
                    Club = "Org X",
                    Squadding = "Relay 1 FP 2",
                    CompetitorNumber = "5678",
                    CourseOfFireId = 3,
                    StageLabel = "T",
                    Series = 5
                } );
                barcodes.Add( new BarcodeLabelEncoding {
                    DisplayName = "Bob Johnson",
                    TeamName = "Team A",
                    CompetitorNumber = "9012",
                    CourseOfFireId = 4,
                    StageLabel = "U",
                    Series = 6
                } );
                barcodes.Add( new BarcodeLabelEncoding {
                    DisplayName = "Alice Williams",
                    Club = "Org Extra long Name because reasons",
                    CompetitorNumber = "3456",
                    CourseOfFireId = 5,
                    StageLabel = "V",
                    Series = 7
                } );
                barcodes.Add( new BarcodeLabelEncoding {
                    DisplayName = "Charlie Brown",
                    TeamName = "Team A",
                    CompetitorNumber = "7890",
                    CourseOfFireId = 6,
                    StageLabel = "W",
                    Series = 8
                } );
            }
            BarcodePdfGeneration.GenerateQrLabelPdf( "c://temp//labels.pdf", barcodes, 0, 0 );
        }
    }
}
