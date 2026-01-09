using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataActors.PDF {

    /// <summary>
    /// Abstract class containing common methods to generate PDF files.
    /// <para>Generating PDFs requires a license to QuestPDF.</para>
    /// <code>
    /// QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
    /// </code>
    /// </summary>
    public abstract class PdfGenerator {

        public abstract QuestPDF.Fluent.Document GeneratePdf( PageSize pageSize, string filePath );

        /// <summary>
        /// Method to generate the Report Title (also called a header).
        /// <para>Concrete classes are expected to generate this method.</para>
        /// </summary>
        /// <param name="container"></param>
        protected abstract void ReportTitle( IContainer container );

        /// <summary>
        /// The default colors to use as background in the Report Title (also called a header).
        /// </summary>
        protected Color[] DefaultHeaderBackgroundColors { get; private set; } = [ScoposColors.BLUE_LIGHTEN_3, ScoposColors.LIGHT_GREY_LIGHTEN_2];

        /// <summary>
        /// The default colors to use in the boarder of the Report Title (also called a header).
        /// </summary>
        protected Color[] DefaultHeaderBorderColors { get; private set; } = [ScoposColors.DARK_GREY_LIGHTEN_2, ScoposColors.BLUE_LIGHTEN_2];

        /// <summary>
        /// The default color to use with text, in the Report Title (also called a header).
        /// </summary>
        protected Color DefaultHeaderTextColor { get; private set; } = ScoposColors.DARK_GREY_LIGHTEN_1;

        /// <summary>
        /// The text of the Title to print in the header of the PDF.
        /// </summary>
        protected virtual string Title {
            get { return ""; }
        }

        /// <summary>
        /// Gets or sets a boolean, indicating if page numbers should be included in the footer.
        /// Some PDFs shoudl have it, some not.
        /// </summary>
        protected bool IncludePageNumberInFooter { get; set; } = true;

        /// <summary>
        /// Gets or sets a boolean, indicating if the projected score indicator is included in the footer.
        /// Some PDFs shoudl have it, some not.
        /// </summary>
        protected bool IncludeProjectedScoreIndicatorInFooter { get; set; } = true;

        /// <summary>
        /// Optional text to include in the header.
        /// </summary>
        public string SubTitle { get; set; } = string.Empty;

        public void InsertMetaData( QuestPDF.Fluent.Document document ) {
            document.WithMetadata( new DocumentMetadata() {
                Title = this.Title,
                Subject = SubTitle,
                Author = "Scopos",
                Producer = "BabelFish",
                CreationDate = DateTime.UtcNow,
                Language = "en-US"
            } );
        }

        /// <summary>
        /// Creates a common Footer for each PDF.
        /// </summary>
        /// <param name="container"></param>
        protected virtual void Footer( IContainer container ) {

            container.Row( row => {

                if (IncludePageNumberInFooter) {
                    row.RelativeItem( 1 )
                    .AlignLeft()
                    .Text( x => {
                        x.Span( "Page " );
                        x.CurrentPageNumber();
                        x.Span( " / " );
                        x.TotalPages();
                    } );
                } else {
                    row.RelativeItem( 1 )
                    .AlignRight()
                    .Text( "" );
                }

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( "BabelFish.Reports.Resources.Images.scopos_logo.png" )) {
                    row.RelativeItem( 1 )
                    .AlignCenter()
                    .Padding( 1 )
                    .Height( .75f, Unit.Centimetre )
                    .Image( stream );
                }

                if (IncludeProjectedScoreIndicatorInFooter) {
                    row.RelativeItem( 1 )
                    .AlignRight()
                    .Text( "◎ projected score" );
                } else {
                    row.RelativeItem( 1 )
                    .AlignRight()
                    .Text( "" );
                }
            } );
        }
    }
}
