using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Scopos.BabelFish.DataActors.PDF {
    public abstract class PdfGenerator {

        public abstract QuestPDF.Fluent.Document GeneratePdf( PageSize pageSize, string filePath );

        protected abstract void ReportTitle( IContainer container );

        protected virtual string Title {
            get { return ""; }
        }

        protected bool IncludePageNumberInFooter { get; set; } = true;

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

                //Empty row item, just so the scopos logo will appear in center of page.
                row.RelativeItem( 1 )
                .AlignRight()
                .Text( "◎ projected score" );
            } );
        }
    }
}
