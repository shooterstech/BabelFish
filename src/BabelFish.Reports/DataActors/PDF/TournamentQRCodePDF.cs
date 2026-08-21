using System.Diagnostics;
using System.Reflection;
using NLog;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;
using Document = QuestPDF.Fluent.Document;

namespace Scopos.BabelFish.DataActors.PDF {
    public class TournamentQRCodePDF : PdfGenerator {

        private Logger _logger = LogManager.GetCurrentClassLogger();
        public Tournament? TournamentDetail { get; private set; } = null;

        public TournamentQRCodePDF( Tournament tournamentDetail ) {
            this.TournamentDetail = tournamentDetail;
            this.IncludePageNumberInFooter = false;
            this.IncludeProjectedScoreIndicatorInFooter = false;
        }

        public async Task InitializeAsync() {
            ;
        }

        protected override string Title {
            get {
                return $"Results for {TournamentDetail.TournamentName}";
            }
        }

        public override Document GeneratePdf( PageSize pageSize, string filePath ) {

            var document = QuestPDF.Fluent.Document.Create( container => {
                container.Page( page => {
                    page.Size( pageSize );
                    page.Margin( 1.5f, Unit.Centimetre );
                    page.PageColor( Colors.White );
                    page.DefaultTextStyle( x => x.FontSize( 10 ) );

                    page.Content().Column( column => {

                        column.Spacing( 10 );
                        column.Item().Element( ReportTitle );

                        column.Item().Element( TournamentQRCode );
                    } );

                    page.Footer().Element( Footer );
                } );
            } );

            InsertMetaData( document );

            if (!string.IsNullOrEmpty( filePath ))
                document.GeneratePdf( filePath );

            return document;
        }

        protected override void ReportTitle( IContainer container ) {

            container.Border( 2 )
            .BorderLinearGradient( 45, this.DefaultHeaderBorderColors )
            .BackgroundLinearGradient( 45, this.DefaultHeaderBackgroundColors )
            .CornerRadius( 5 )
            .Padding( 6 )
            .Row( row => {

                row.RelativeItem().Column( column => {
                    column.Item().Text( Title ).SemiBold().FontSize( 14 ).FontColor( this.DefaultHeaderTextColor );
                    column.Item().Text( $"Rezult Homepage" ).SemiBold().FontSize( 11 ).FontColor( this.DefaultHeaderTextColor );
                    column.Item().Text( StringFormatting.SpanOfDates( TournamentDetail.StartDate, TournamentDetail.EndDate ) ).SemiBold().FontSize( 11 ).FontColor( this.DefaultHeaderTextColor );
                } );

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( "Scopos.BabelFish.Resources.Images.scopos_owl_transparent_orange.png" )) {
                    Debug.Assert( stream is not null, "Could not find embedded resource for header image." );

                    row.ConstantItem( 3.0f, Unit.Centimetre )
                   .AspectRatio( 1 )
                   .Padding( 1 )
                   .Image( stream );
                }

            } );
        }

        protected void TournamentQRCode( IContainer container ) {

            var uri = $"https://rezults.scopos.tech/tournament/{TournamentDetail.TournamentId}/?src=tournament-qr-code";

            container.Column( column => {
                column.Spacing( 4 );

                column.Item().AlignCenter().AspectRatio( 1 )
                    .Background( Colors.White )
                    .Svg( size => {
                        var writer = new QRCodeWriter();
                        var qrCode = writer.encode( uri, BarcodeFormat.QR_CODE, (int)size.Width, (int)size.Height );
                        var renderer = new SvgRenderer { FontName = "Lato" };
                        return renderer.Render( qrCode, BarcodeFormat.EAN_13, null ).Content;
                    } );

                column.Item().AlignCenter()
                    .Text( "Scan to see the full Tournament results." ).SemiBold().FontSize( 16 ).FontColor( ScoposColors.DARK_GREY );
            } );
        }
    }
}
