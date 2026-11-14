using System.Reflection;
using NLog;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.Helpers;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;

namespace Scopos.BabelFish.DataActors.PDF {
    public class ClubQRCodePDF : PdfGenerator {

        private Logger _logger = LogManager.GetCurrentClassLogger();
        private byte[]? _profilePhotoBytes;

        public ClubDetail? ClubDetail { get; private set; } = null;

        public ClubQRCodePDF( ClubDetail clubDetail ) {
            this.ClubDetail = clubDetail;
            this.IncludePageNumberInFooter = false;
            this.IncludeProjectedScoreIndicatorInFooter = false;
        }

        public async Task InitializeAsync() {
            if (ClubDetail?.ProfilePhoto?.IsValid != true)
                return;

            try {
                using var http = new HttpClient();
                _profilePhotoBytes = await http.GetByteArrayAsync( ClubDetail.ProfilePhoto.Url );
            } catch {
                _profilePhotoBytes = null; // fallback path
            }
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

            var uri = ClubDetail.Url;
            var contactList = new List<string>();

            var contactCount = 0;
            foreach (var contact in ClubDetail.ContactList) {
                if (contact.ContactType != ClubContactType.EMAIL
                    && contact.ContactType != ClubContactType.WEBSITE
                    && contact.ContactType != ClubContactType.PHONE_NUMBER
                    && contact.Visibility == DataModel.Common.VisibilityOption.PUBLIC
                    && contact.HasValue) {
                    contactList.Add( $"{contact.ContactType}: {contact.DisplayValue}" );
                    contactCount++;
                    if (contactCount >= 3) {
                        break;
                    }
                }
            }

            container.Border( 2 )
            .BorderLinearGradient( 45, this.DefaultHeaderBorderColors )
            .BackgroundLinearGradient( 45, this.DefaultHeaderBackgroundColors )
            .CornerRadius( 5 )
            .Padding( 6 )
            .Row( row => {

                row.RelativeItem().Column( column => {
                    column.Item().Text( ClubDetail.Name ).SemiBold().FontSize( 14 ).FontColor( this.DefaultHeaderTextColor );
                    column.Item().Text( $"Homepage on Rezults" ).SemiBold().FontSize( 11 ).FontColor( this.DefaultHeaderTextColor );
                    column.Item().Text( $"{ClubDetail.Hometown}" ).SemiBold().FontSize( 11 ).FontColor( this.DefaultHeaderTextColor );
                    column.Item().Text( $"{uri}" ).SemiBold().FontSize( 11 ).FontColor( this.DefaultHeaderTextColor );
                    column.Item().Text( string.Join( ", ", contactList ) ).SemiBold().FontSize( 11 ).FontColor( this.DefaultHeaderTextColor );
                } );

                // Use the Club's Profile Photo if it exists, otherwise use the default Scopos Owl image.
                if (_profilePhotoBytes is not null && _profilePhotoBytes.Length > 0) {
                    row.ConstantItem( 2.5f, Unit.Centimetre )
                       .AspectRatio( 1 )
                       .Padding( 1 )
                       .Image( _profilePhotoBytes );
                } else {
                    using var stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream( "Scopos.BabelFish.Resources.Images.scopos_owl_transparent_orange.png" );

                    row.ConstantItem( 2.5f, Unit.Centimetre )
                       .AspectRatio( 1 )
                       .Padding( 1 )
                       .Image( stream );
                }

            } );

        }

        protected void ClubQRCode( IContainer container ) {

            var uri = $"{ClubDetail.Url}?src=club-qr-code";

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
                    .Text( $"Scan to stay updated of all {ClubDetail.Name}'s news and events." ).SemiBold().FontSize( 14 ).FontColor( ScoposColors.DARK_GREY );
                column.Item().AlignCenter()
                    .Text( "Live competition results will appear at the top of the news feed when our club is shooting." ).FontSize( 12 ).FontColor( ScoposColors.DARK_GREY );
            } );
        }
    }
}
