using System;
using System.Collections.Generic;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NLog;
using Scopos.BabelFish.DataModel.Clubs;
using ZXing.QrCode;
using ZXing;
using ZXing.Rendering;
using System.Reflection;

namespace Scopos.BabelFish.DataActors.PDF {
    public class ClubQRCodePDF : PdfGenerator {

        private Logger _logger = LogManager.GetCurrentClassLogger();
        public ClubDetail ?  ClubDetail { get; private set; } = null;

        public ClubQRCodePDF( ClubDetail clubDetail ) {
            this.ClubDetail = clubDetail;
            this.IncludePageNumberInFooter = false;
        }

        public async Task InitializeAsync() {
            ;
        }

        protected override string Title {
            get {
                return ClubDetail.Name;
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

                        column.Item().Element( ClubQRCode );
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

            var uri = $"https://rezults.scopos.tech/club/{ClubDetail.URLPath}/";

            container.Border( 2 )
            .BorderLinearGradient( 45, [ScoposColors.BLUE_LIGHTEN_2, ScoposColors.DARK_GREY_LIGHTEN_1] )
            .BackgroundLinearGradient( 45, [ScoposColors.DARK_GREY, ScoposColors.BLUE_LIGHTEN_1] )
            .CornerRadius( 5 )
            .Padding( 10 )
            .Row( row => {

                row.RelativeItem().Column( column => {
                    column.Item().Text( ClubDetail.Name ).SemiBold().FontSize( 16 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( $"Rezult Homepage" ).SemiBold().FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( $"{ClubDetail.Hometown}" ).SemiBold().FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( $"{uri}" ).SemiBold().FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                } );


                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( "BabelFish.Resources.Images.scopos_owl_transparent_orange.png" )) {

                    row.ConstantItem( 2.5f, Unit.Centimetre )
                   .AspectRatio( 1 )
                   .Padding( 1 )
                   .Image( stream );

                }

            } );

        }

            protected void ClubQRCode( IContainer container ) {

            var uri = $"https://rezults.scopos.tech/club/{ClubDetail.URLPath}/?source=club-qr-code";

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
                    .Text( "Scan to watch our live competition results." ).SemiBold().FontSize( 16 ).FontColor( ScoposColors.DARK_GREY );
                column.Item().AlignCenter()
                    .Text( "Links on this page will appear when we are shooting a public match." ).FontSize( 12 ).FontColor( ScoposColors.DARK_GREY );
            } );
        }
    }
}
