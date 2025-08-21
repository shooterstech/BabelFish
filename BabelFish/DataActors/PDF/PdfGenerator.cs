using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile;
using Scopos.BabelFish.DataModel.Athena.ESTUnitCommands;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;

namespace Scopos.BabelFish.DataActors.PDF {
    public abstract class PdfGenerator {

        public abstract QuestPDF.Fluent.Document GeneratePdf(PageSize pageSize, string filePath);

        protected abstract void ReportTitle( IContainer container );

        protected virtual string Title {
            get { return ""; }
        }

        /// <summary>
        /// Optional text to include in the header.
        /// </summary>
        public string SubTitle { get; set; } = string.Empty;

        public void InsertMetaData( QuestPDF.Fluent.Document document ) {
            document.WithMetadata( new DocumentMetadata() {
                Title = this.Title,
                Author = "Scopos",
                Producer = "BabelFish",
                CreationDate = DateTime.UtcNow,
                Language = "en-US"
            } );
        }

        protected virtual void Footer( IContainer container ) {
            foreach (var resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
                Console.WriteLine( resourceName );
            }

            container.Row( row => {
                row.RelativeItem( 1 )
                .AlignLeft()
                .Text( x => {
                    x.Span( "Page " );
                    x.CurrentPageNumber();
                    x.Span( " / " );
                    x.TotalPages();
                } );

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( "BabelFish.Resources.Images.scopos_logo.png" )) {
                    row.RelativeItem( 1 )
                    .AlignCenter()
                    .Padding( 1 )
                    .Height( .75f, Unit.Centimetre )
                    .Image( stream );
                }

                //Empty row item, just so the scopos logo will appear in center of page.
                row.RelativeItem( 1 )
                .AlignRight()
                .Text( "" );
            } );
        }
    }
}
