using System.Diagnostics;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class BarcodeLabelEncoding {

        #region Private Variables
        string _competitorNumber = string.Empty;
        string _stageLabel = string.Empty;
        #endregion

        #region Constructors, Factory Methods, and Initializers
        /// <summary>
        /// Public constructor
        /// </summary>
        public BarcodeLabelEncoding() {

        }

        /// <summary>
        /// Generates a list of barcode label encodings for the specified course of fire entries.
        /// </summary>
        /// <param name="courseOfFireEntries">A list of individual course of fire entries for which barcode label encodings will be created. Each entry
        /// must have a non-null CourseOfFireStructure.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of BarcodeLabelEncoding
        /// objects generated from the provided course of fire entries.</returns>
        /// <exception cref="BackwardsPointerException">Thrown if any entry in the list has a null CourseOfFireStructure, indicating that the entry was likely
        /// created outside of a MatchProject.</exception>
        public static async Task<List<BarcodeLabelEncoding>> CreateListAsync( List<CourseOfFireEntryIndividual> courseOfFireEntries ) {

            List<BarcodeLabelEncoding> barcodeLabelEncodings = new List<BarcodeLabelEncoding>();

            foreach (var entry in courseOfFireEntries) {
                if (entry.CourseOfFireStructure is null) {
                    throw new BackwardsPointerException( $"CourseOfFireEntry with ParticipantID {entry.MatchParticipant?.ParticipantID} has a null CourseOfFireStructure. Cannot generate BarcodeLabelEncodings without CourseOfFireStructure. Likely caused by the Entry being created outside of a MatchProject." );
                }

                var cofStructure = entry.CourseOfFireStructure;
                var paperTargetLabel = await cofStructure.GetPaperTargetLabelAsync();

                foreach (var barcodeLabel in paperTargetLabel.Labels) {
                    foreach (var seriesIndex in barcodeLabel.Series.GetAsList()) {
                        barcodeLabelEncodings.Add( new BarcodeLabelEncoding {
                            DisplayName = entry.MatchParticipant?.Participant?.DisplayName ?? string.Empty,
                            CompetitorNumber = entry.MatchParticipant?.Participant.CompetitorNumber ?? string.Empty,
                            TeamName = entry.Team?.DisplayName ?? string.Empty,
                            Club = entry.MatchParticipant?.Participant?.Club ?? string.Empty,
                            CourseOfFireId = entry.CourseOfFireId,
                            StageLabel = barcodeLabel.StageLabel,
                            Series = seriesIndex
                        } );
                    }
                }
            }

            return barcodeLabelEncodings;
        }

        /// <summary>
        /// Decodes a 12-character encoded barcode string into a BarcodeLabel object containing competitor and event
        /// information.
        /// </summary>
        /// <param name="encodedText">The 12-character encoded barcode string to decode. Must not be null and must be exactly 12 characters in
        /// length.</param>
        /// <returns>A BarcodeLabel object containing the decoded competitor number, course of fire ID, stage label, and series.</returns>
        /// <exception cref="ArgumentNullException">Thrown if encodedText is null.</exception>
        /// <exception cref="ArgumentException">Thrown if encodedText is not exactly 12 characters long, or if the course of fire ID or series segments are
        /// not valid integers.</exception>
        public static BarcodeLabelEncoding Decode( string encodedText ) {
            if (encodedText is null) {
                throw new ArgumentNullException( nameof( encodedText ) );
            }
            if (encodedText.Length != 12) {
                throw new ArgumentException( "Encoded text must be exactly 12 characters long." );
            }
            var competitorNumber = encodedText.Substring( 0, 6 ).Trim();
            var courseOfFireIdText = encodedText.Substring( 6, 2 );
            var stageLabel = encodedText.Substring( 8, 2 ).Trim();
            var seriesText = encodedText.Substring( 10, 2 );

            if (!int.TryParse( courseOfFireIdText, out int courseOfFireId )) {
                throw new ArgumentException( "Course of Fire ID must be a valid integer." );
            }

            if (!int.TryParse( seriesText, out int series )) {
                throw new ArgumentException( "Series must be a valid integer." );
            }

            return new BarcodeLabelEncoding {
                CompetitorNumber = competitorNumber,
                CourseOfFireId = courseOfFireId,
                StageLabel = stageLabel,
                Series = series
            };
        }
        #endregion


        #region Data Model Properties
        /// <summary>
        /// The display name of the <see cref="Participant"/>. This is not used in the encoding of the barcode, but is included in the text printed on the physical barcode label to help identify the competitor. 
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The name of the team that this <see cref="Participant"/> is shooting for. This is not used in the encoding of the barcode, but is included in the text printed on the physical barcode label to help identify the competitor.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// The name of the club (aka organization) that this <see cref="Participant"/> is shooting for. This is not used in the encoding of the barcode, but is included in the text printed on the physical barcode label to help identify the competitor.
        /// </summary>
        public string Club { get; set; }

        /// <summary>
        /// The squadding assignment (e.g. relay and firing point) for the competitor. This is not used in the encoding of the barcode, but is included in the text printed on the physical barcode label to help identify the competitor and their assigned squadding.
        /// </summary>
        public string Squadding { get; set; }

        /// <summary>
        /// The Competitor Number of the participant.
        /// <para>In most cases Competitor Numbers are numeric, but may also be alphanumeric.</para>
        /// <para>Competitor Numbers are allowed to be an empty string (although this is poor practice),
        /// but must not be more than 6 characters long.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
        /// <exception cref="CompetitorNumberTooLongException">Thrown if the value is more than 6 characters.</exception>
        public string CompetitorNumber {
            get {
                return _competitorNumber;
            }
            set {
                if (value is null) {
                    throw new ArgumentNullException( nameof( value ) );
                }

                var trimmed = value.Trim();
                if (trimmed.Length > 6) {
                    throw new CompetitorNumberTooLongException( "CompetitorNumber must be a string of 6 characters or less." );
                }

                _competitorNumber = trimmed;
            }
        }

        /// <summary>
        /// The CourseOfFireId is used to link the BarcodeLabel to a specific <see cref="CourseOfFireStructure"/>.
        /// <para>A value of 0 is allowed, and means that the BarcodeLabel is not linked to any specific CourseOfFireStructure.</para>
        /// </summary>
        public int CourseOfFireId { get; set; } = 0;

        /// <summary>
        /// The StageLabel references the <see cref="CourseOfFire"/>'s <see cref="CourseOfFire.Singulars"/> StageLabel property. It is almost
        /// always a single character, but may be 2 characters. It is used to link the BarcodeLabel to a specific stage of the match, and is used in the mapping process of shots to events.
        /// <para>An empty string is allowed, but indicates that the BarcodeLabel is not linked to any specific stage.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
        /// <exception cref="StageLabelTooLongException">Thrown if the value is more than2 characters.</exception>
        public string StageLabel {
            get {
                return _stageLabel;
            }
            set {
                if (value is null) {
                    throw new ArgumentNullException( nameof( value ) );
                }

                var trimmed = value.Trim();
                if (trimmed.Length > 6) {
                    throw new StageLabelTooLongException( "StageLabel must be a string of 6 characters or less." );
                }

                _stageLabel = trimmed;
            }
        }

        /// <summary>
        /// Gets or sets the series index number. It is the series index within the stage (as labeld by the <see cref="StageLabel"/>. 
        /// <para>A value of 0 is allowed, and means that the BarcodeLabel is not linked to any specific series.</para>
        /// </summary>
        public int Series { get; set; } = 0;

        #endregion

        #region Helper Properties
        /// <summary>
        /// The encoded value of this Barcode Label. Which is what will be encoded into a 1D or 2D barcode for printing on the physical barcode labels. The format of the encoded value is as follows:
        /// <list type="bullet">
        /// <item>The first 6 characters are the Competitor Number, left padded with spaces if less than 6 characters.</item>
        /// <item>The next 2 characters are the CourseOfFireId, right padded with 0s.</item>
        /// <item>The next 2 characters are the StageLabel, left padded with spaces if less than 2 characters.</item>
        /// <item>The last 2 characters are the Series, right padded with 0s.</item>
        /// </list>
        /// <para>For example, if the CompetitorNumber is "123", the CourseOfFireId is 4, the StageLabel is "S", and the Series is 2, the encoded value would be "   12304 S02".</para>
        /// </summary>
        public string Encode {
            get {
                var encoded = $"{CompetitorNumber.PadLeft( 6, ' ' )}{CourseOfFireId:00}{StageLabel.PadLeft( 2, ' ' )}{Series:00}";
                Debug.Assert( encoded.Length == 12, "Encoded BarcodeLabel should always be 12 characters long." );
                return encoded;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns a string that represents the competitor, course of fire, stage label, and series in a formatted
        /// manner.
        /// <para>To return the encoded representation of the BarcodeLabel, use the <see cref="Encode"/> property.</para>
        /// </summary>
        /// <returns>A string containing the competitor number, course of fire ID, stage label, and series, separated by hyphens.</returns>
        public override string ToString() {
            return $"{CompetitorNumber} - {CourseOfFireId} - {StageLabel}{Series}";
        }
        #endregion
    }
}
