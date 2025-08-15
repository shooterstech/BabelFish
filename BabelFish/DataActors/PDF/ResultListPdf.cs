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
    public class ResultListPdf : PdfGenerator {

        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static IUserProfileLookup _userProfileLookup = new BaseUserProfileLookup();

        public Match Match { get; private set; }
        public ResultList ResultList { get; private set; }

        public ResultListFormat? ResultListFormat { get; private set; } = null;

        public ResultListIntermediateFormatted? RLF { get; private set; } = null;

        public CourseOfFire CourseOfFire { get; private set; } = null;

        public ResultListPdf( Match match, ResultList resultList ) {
            this.ResultList = resultList;
            this.Match = match;
        }

        public async Task InitializeAsync() {

            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( ResultList ).ConfigureAwait( false );
            ResultListFormat = await DefinitionCache.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            CourseOfFire = await DefinitionCache.GetCourseOfFireDefinitionAsync( SetName.Parse( Match.CourseOfFireDef ) );
            RLF = new ResultListIntermediateFormatted( ResultList, ResultListFormat, _userProfileLookup );

            RLF.Engagable = false;
            RLF.ShowSupplementalInformation = false;
            RLF.GetParticipantAttributeRankDeltaPtr = this.RankDeltaFormatting;

            await RLF.InitializeAsync();
        }

        public override QuestPDF.Fluent.Document GeneratePdf( PageSize pageSize, string filePath ) {
            if (ResultListFormat == null)
                throw new InitializeAsyncNotCompletedException();

            var document = QuestPDF.Fluent.Document.Create( container => {
                container.Page( page => {
                    page.Size( pageSize );
                    page.Margin( 2, Unit.Centimetre );
                    page.PageColor( Colors.White );
                    page.DefaultTextStyle( x => x.FontSize( 10 ) );

                    RLF.ResolutionWidth = (int)((pageSize.Width - 114) * 1200 / 500);

                    page.Content().Column( column => {

                        column.Spacing( 10 );
                        column.Item().Element( ReportTitle );

                        column.Item().Element( ResultListTable );
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
            var rezultsUrl = $"https://rezults.scopos.tech/match/{this.Match.ParentID}/";

            container.Border( 2, ScoposColors.BLUE_LIGHTEN_1 )
            .Background( ScoposColors.DARK_GREY_LIGHTEN_1 )
            .CornerRadius( 5 )
            .Padding( 10 )
            .Row( row => {
                row.RelativeItem().Column( column => {
                    column.Item().Text( ResultList.ResultName ).SemiBold().FontSize( 16 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( $"{Match.Name} | {StringFormatting.SpanOfDates( ResultList.StartDate, ResultList.EndDate )}" ).SemiBold().FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( $"Course of Fire: {CourseOfFire.CommonName}" ).SemiBold().FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( $"Status: {ResultList.Status} | Printed at {StringFormatting.SingleDateTime( DateTime.Now )}" ).FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( $"{StringFormatting.Location( Match.Location.City, Match.Location.State, Match.Location.Country )}" ).FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                } );

                if (Match.Visibility == DataModel.Common.VisibilityOption.PUBLIC) {
                    row.ConstantItem( 3, Unit.Centimetre )
                        .AspectRatio( 1 )
                        .Background( Colors.White )
                        .Svg( size => {
                            var writer = new QRCodeWriter();
                            var qrCode = writer.encode( rezultsUrl, BarcodeFormat.QR_CODE, (int)size.Width, (int)size.Height );
                            var renderer = new SvgRenderer { FontName = "Lato" };
                            return renderer.Render( qrCode, BarcodeFormat.QR_CODE, null ).Content;
                        } );
                }
            } );
        }

        protected override string Title {
            get {
                return $"Results for {ResultList.ResultName}";
            }
        }

        private void ResultListTable( IContainer container ) {

            var columnIndexes = RLF.GetShownColumnIndexes();

            container.Table( table => {
                 table.ColumnsDefinition( columns => {
                     foreach (var colIndex in columnIndexes) {
                         var headerCell = RLF.GetColumnHeaderCell( colIndex );

                         if (headerCell.ClassList.Contains( "rlf-col-rank" )
                             || headerCell.ClassList.Contains( "rlf-col-gap" ))
                             columns.RelativeColumn( 1 );
                         else if (headerCell.ClassList.Contains( "rlf-col-participant" ))
                             columns.RelativeColumn( 4 );
                         else
                             columns.RelativeColumn( 2 );

                     }
                 } );

                 table.Header( header => {
                     foreach (var colIndex in columnIndexes) {
                         var headerCell = RLF.GetColumnHeaderCell( colIndex );
                         header.Cell().BorderBottom( 2 ).BorderColor( ScoposColors.BLUE_LIGHTEN_1 ).Padding( 2 ).Text( headerCell.Text ).Bold();
                     }
                 } );

                 foreach (var row in RLF.Rows) {
                     foreach (var colIndex in columnIndexes) {
                         var rowCell = row.GetColumnBodyCell( colIndex );
                         table.Cell().Padding( 2 ).Text( rowCell.Text );
                     }
                 }
             } );
        }

        public string RankDeltaFormatting( IRLIFItem resultEvent, ResultListIntermediateFormatted rlf ) {
            return "";
        }
    }
}
