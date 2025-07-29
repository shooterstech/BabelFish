using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile;
using Scopos.BabelFish.DataModel.Athena.ESTUnitCommands;
using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;

namespace Scopos.BabelFish.DataActors.PDF {
    public class ResultCOFPdf : PdfGenerator {

        public ResultCOF ResultCOF { get; private set; }

        public EventtType EventtType { get; private set; }

        public CourseOfFire ? CourseOfFire { get; private set; } = null;
        public EventComposite TopLevelEvent { get; private set; } = null;

        public ResultCOFPdf( ResultCOF resultCOF, EventtType et ) {
            this.ResultCOF = resultCOF;
            this.EventtType = et;
        }

        public async Task InitializeAsync() {
            CourseOfFire = await DefinitionCache.GetCourseOfFireDefinitionAsync( SetName.Parse( this.ResultCOF.CourseOfFireDef ) );
            TopLevelEvent = EventComposite.GrowEventTree( this.CourseOfFire );
        }

        protected override void ReportTitle( IContainer container ) {

            container.Border( 2, ScoposColors.BLUE_LIGHTEN_1 )
            .Background( ScoposColors.DARK_GREY_LIGHTEN_1 )
            .CornerRadius( 5 )
            .Padding( 10 )
            .Row( row => {
                row.RelativeItem().Column( column => {
                    column.Item().Text( ResultCOF.Participant.DisplayName ).SemiBold().FontSize( 16 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( $"{ResultCOF.MatchName} | {StringFormatting.SingleDate( ResultCOF.LocalDate )}" ).SemiBold().FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( $"Course of Fire: {CourseOfFire.CommonName}" ).SemiBold().FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                    column.Item().Text( $"Status: {ResultCOF.Status} | Printed at {StringFormatting.SingleDateTime( DateTime.Now )}" ).FontSize( 12 ).FontColor( ScoposColors.LIGHT_GREY_LIGHTEN_3 );
                } );
            } );
        }

        protected override string Title {
            get {
                return $"Results for {ResultCOF.Participant.DisplayName}";
            }
        }

        protected void ResultCOFTable( IContainer container ) {

            

            container.Column( column => {
                column.Spacing( 10 );

                foreach (var eventType in Enum.GetValues( typeof( EventtType ) ).Cast<EventtType>().ToList()) {
                    if (eventType == EventtType.NONE || eventType == EventtType.SINGULAR)
                        continue;

                    var eventsToHighlight = TopLevelEvent.GetEvents( eventType );

                    foreach (var eventToHighlight in eventsToHighlight) {
                        if (this.ResultCOF.EventScores.TryGetValue( eventToHighlight.EventName, out EventScore es )
                            && es.NumShotsFired > 0) {
                            column.Item().Background( ScoposColors.BLUE_LIGHTEN_2 )
                            .CornerRadius( 4 )
                            .Padding( 4 )
                            .Row( row => {
                                row.RelativeItem( 1 )
                                .AlignLeft()
                                .Text( eventToHighlight.EventName );

                                row.RelativeItem( 1 )
                                .AlignLeft()
                                .Text( $"Score: {es.ScoreFormatted}" );

                                row.RelativeItem( 1 )
                                .AlignLeft()
                                .Text( $"{es.NumShotsFired} shots" );

                                if (!string.IsNullOrEmpty( es.EventStyleDef )
                                    && DefinitionCache.TryGetEventStyleDefinition( SetName.Parse( es.EventStyleDef ), out EventStyle eventStyle )) {
                                    row.RelativeItem( 2 )
                                    .AlignLeft()
                                    .Text( eventStyle.CommonName );
                                } else if (!string.IsNullOrEmpty( es.StageStyleDef )
                                    && DefinitionCache.TryGetStageStyleDefinition( SetName.Parse( es.StageStyleDef ), out StageStyle stageStyle )) {
                                    row.RelativeItem( 2 )
                                    .AlignLeft()
                                    .Text( stageStyle.CommonName );
                                } else {
                                    row.RelativeItem( 2 )
                                    .AlignLeft()
                                    .Text( "" );
                                }

                            } );

                            if (eventType == this.EventtType) {
                                column.Item().Table( table => {
                                    table.ColumnsDefinition( columns => {
                                        columns.RelativeColumn( 1 ); //Event Name
                                        columns.RelativeColumn( 1 ); //Score
                                        columns.RelativeColumn( 2 ); //Time Scored
                                        columns.RelativeColumn( 3 ); //Attributes
                                    } );

                                    table.Header( header => {
                                        header.Cell().BorderBottom( 2 ).BorderColor( ScoposColors.BLUE_LIGHTEN_1 ).Padding( 1 ).Text( "Name" ).FontSize( 8 ).Bold();
                                        header.Cell().BorderBottom( 2 ).BorderColor( ScoposColors.BLUE_LIGHTEN_1 ).Padding( 1 ).Text( "Score" ).FontSize( 8 ).Bold();
                                        header.Cell().BorderBottom( 2 ).BorderColor( ScoposColors.BLUE_LIGHTEN_1 ).Padding( 1 ).Text( "Time Scored" ).FontSize( 8 ).Bold();
                                        header.Cell().BorderBottom( 2 ).BorderColor( ScoposColors.BLUE_LIGHTEN_1 ).Padding( 1 ).Text( "Attributes" ).FontSize( 8 ).Bold();
                                    } );

                                    var singulars = eventToHighlight.GetAllSingulars();
                                    foreach( var singular in singulars ) {
                                        if (ResultCOF.GetShotsByEventName().TryGetValue( singular.EventName, out Shot shot ) ) {
                                            table.Cell().Padding( 1 ).Text( shot.EventName ).FontSize( 8 );
                                            table.Cell().Padding( 1 ).Text( StringFormatting.FormatScore( "{m}{d}{X}", shot.Score ) ).FontSize( 8 );
                                            table.Cell().Padding( 1 ).Text( shot.TimeScored.ToString( "yyyy-MM-dd HH:mm:ss.F" ) ).FontSize( 8 );
                                            table.Cell().Padding( 1 ).Text( string.Join( ", ", shot.Attributes ?? Enumerable.Empty<string>() ) ).FontSize( 8 );
                                        }
                                    }

                                } );
                            }
                        }
                    }
                }
            } );
        }

        public override QuestPDF.Fluent.Document GeneratePdf( PageSize pageSize, string filePath ) {
            if (CourseOfFire == null)
                throw new InitializeAsyncNotCompletedException();

            var document = QuestPDF.Fluent.Document.Create( container => {
                container.Page( page => {
                    page.Size( pageSize );
                    page.Margin( 2, Unit.Centimetre );
                    page.PageColor( Colors.White );
                    page.DefaultTextStyle( x => x.FontSize( 10 ) );

                    page.Content().Column( column => {

                        column.Spacing( 10 );
                        column.Item().Element( ReportTitle );

                        column.Item().Element( ResultCOFTable );
                    } );

                    page.Footer().Element( Footer );
                } );
            } );

            InsertMetaData( document );

            if (!string.IsNullOrEmpty( filePath ))
                document.GeneratePdf( filePath );

            return document;
        }
    }
}
