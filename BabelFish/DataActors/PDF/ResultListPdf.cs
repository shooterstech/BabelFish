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
        public IRLIFList ItemList { get; private set; }

        public ResultListFormat? ResultListFormat { get; private set; } = null;

        public ResultListIntermediateFormatted? RLIF { get; private set; } = null;

        public CourseOfFire CourseOfFire { get; private set; } = null;

        public ResultListPdf( Match match, IRLIFList itemList ) {
            this.ItemList = itemList;
            this.Match = match;
        }

        public async Task InitializeAsync() {

            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( ItemList ).ConfigureAwait( false );
            ResultListFormat = await DefinitionCache.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            CourseOfFire = await DefinitionCache.GetCourseOfFireDefinitionAsync( SetName.Parse( Match.CourseOfFireDef ) );
            RLIF = new ResultListIntermediateFormatted( ItemList, ResultListFormat, _userProfileLookup );

            RLIF.Engagable = false;
            RLIF.ShowSupplementalInformation = false;
            RLIF.GetParticipantAttributeRankDeltaPtr = this.RankDeltaFormatting;

            await RLIF.InitializeAsync();
        }

        public override QuestPDF.Fluent.Document GeneratePdf( PageSize pageSize, string filePath ) {
            if (ResultListFormat == null)
                throw new InitializeAsyncNotCompletedException();

            var document = QuestPDF.Fluent.Document.Create( container => {
                container.Page( page => {
                    page.Size( pageSize );
                    page.Margin( 1.5f, Unit.Centimetre );
                    page.PageColor( Colors.White );
                    page.DefaultTextStyle( x => x.FontSize( 10 ) );

                    RLIF.ResolutionWidth = (int)((pageSize.Width - 114) * 1200 / 500);

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

            string titleText = string.Empty, line1Text = string.Empty, line2Text = string.Empty, line3Text = string.Empty, line4Text = string.Empty;
			
			line1Text = $"{Match.Name} | {StringFormatting.SpanOfDates( ItemList.StartDate, ItemList.EndDate )}";
			line2Text = $"Course of Fire: {CourseOfFire.CommonName}";
			if (string.IsNullOrEmpty( SubTitle ))
				line4Text = $"{StringFormatting.Location( Match.Location.City, Match.Location.State, Match.Location.Country )}";
			else
				line4Text = SubTitle;

			if (ItemList is ResultList) {
                titleText = $"Results for {ItemList.Name}";
                line3Text = $"Status: {ItemList.Status} | Printed at {StringFormatting.SingleDateTime( DateTime.Now )}";
				rezultsUrl = $"https://rezults.scopos.tech/match/{this.Match.ParentID}/?src=result-list";
            } else if (ItemList is SquaddingList) {
				titleText = $"{ItemList.Name} Squadding";
				line3Text = $"Printed at {StringFormatting.SingleDateTime( DateTime.Now )}";
				rezultsUrl = $"https://rezults.scopos.tech/match/{this.Match.ParentID}/?src=squad-list";
			}

            container.Border( 2 )
			//.Border( 2, ScoposColors.BLUE_LIGHTEN_1 )
			//.Background( ScoposColors.DARK_GREY_LIGHTEN_1 )
			.BorderLinearGradient( 45, [ScoposColors.BLUE_LIGHTEN_2, ScoposColors.DARK_GREY_LIGHTEN_1] )
			.BackgroundLinearGradient( 45, [ScoposColors.DARK_GREY, ScoposColors.BLUE_LIGHTEN_1] )
            .CornerRadius( 5 )
            .Padding( 10 )
            .Row( row => {
                row.RelativeItem().Column( column => {
                    column.Item().Text( titleText ).SemiBold().FontSize( 16 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( line1Text ).SemiBold().FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( line2Text ).SemiBold().FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( line3Text ).FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( line4Text ).FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                } );


                if (Match.Visibility == DataModel.Common.VisibilityOption.PUBLIC) {
                    row.ConstantItem( 3, Unit.Centimetre )
                        .AspectRatio( 1 )
                        .Background( Colors.White )
                        .Svg( size => {
                            var writer = new QRCodeWriter();
                            var qrCode = writer.encode( rezultsUrl, BarcodeFormat.QR_CODE, 4 * (int)size.Width, 4 * (int)size.Height );
                            var renderer = new SvgRenderer { FontName = "Lato" };
                            return renderer.Render( qrCode, BarcodeFormat.QR_CODE, null ).Content;
                        } );
                }
            } );
        }

        protected override string Title {
            get {
                return $"Results for {ItemList.Name}";
            }
        }

        private void ResultListTable( IContainer container ) {

            container.Table( table => {
                 table.ColumnsDefinition( columns => {
                     foreach (var headerCell in RLIF.GetShownHeaderRow()) {

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
                     foreach (var headerCell in RLIF.GetShownHeaderRow()) {
                         header.Cell().BorderBottom( 2 ).BorderColor( ScoposColors.BLUE_LIGHTEN_1 ).Padding( 1 ).Text( headerCell.Text ).Bold();
                     }
                 } );

                foreach (var row in RLIF.ShownRows) {
                    foreach (var subRow in row) {
                        foreach (var rowCell in subRow.GetShownRow()) {
                            table.Cell().ColumnSpan( (uint) rowCell.ColumnSpan ).Padding( 1 ).Text( rowCell.Text );
                        }
                    }
                }
             } );
        }

        public string RankDeltaFormatting( IRLIFItem resultEvent, ResultListIntermediateFormatted rlf ) {
            return "";
        }
    }
}
