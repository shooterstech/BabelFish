using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using NLog;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.PDF {
    public class ResultListPdf : IGeneratePdf {

        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static IUserProfileLookup _userProfileLookup = new BaseUserProfileLookup();

        public ResultList ResultList { get; private set; }

        public ResultListFormat? ResultListFormat { get; private set; } = null;

        public ResultListIntermediateFormatted? RLF { get; private set; } = null;

        public ResultListPdf( ResultList resultList ) {
            ResultList = resultList;
        }

        public async Task InitializeAsync() {

            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( ResultList ).ConfigureAwait( false );
            ResultListFormat = await DefinitionCache.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            RLF = new ResultListIntermediateFormatted( ResultList, ResultListFormat, _userProfileLookup );
            await RLF.InitializeAsync();

            RLF.Engagable = false;
            RLF.ShowSupplementalInformation = false;
            RLF.GetParticipantAttributeRankDeltaPtr = this.RankDeltaFormatting;
        }

        public void GeneratePdf() {
            if (ResultListFormat == null)
                throw new InitializeAsyncNotCompletedException();

            QuestPDF.Fluent.Document.Create( container => {
                container.Page( page => {
                    page.Size( PageSizes.Letter );
                    page.Margin( 2, Unit.Centimetre );
                    page.PageColor( Colors.White );
                    page.DefaultTextStyle( x => x.FontSize( 10 ) );

                    var width = PageSizes.Letter.Width;

                    page.Header()
                        .Text( ResultList.ResultName )
                        .SemiBold().FontSize( 24 ).FontColor( Colors.Blue.Medium );

                    var columnIndexes = RLF.GetShownColumnIndexes();

                    page.Content().Table( table => {
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
                                header.Cell().BorderBottom( 2 ).Padding( 4 ).Text( headerCell.Text );
                            }
                        } );

                        foreach (var row in RLF.Rows) {
                            foreach (var colIndex in columnIndexes) {
                                var rowCell = row.GetColumnBodyCell( colIndex );
                                table.Cell().Padding( 4 ).Text( rowCell.Text );
                            }
                        }
                        /*
                        foreach (var i in Enumerable.Range( 0, 6 )) {
                            var price = 4.50;

                            table.Cell().Padding( 8 ).Text( $"{i + 1}" );
                            table.Cell().Padding( 8 ).Text( Placeholders.Label() );
                            table.Cell().Padding( 8 ).AlignRight().Text( $"${price}" );
                        }
                        */
                    } );

                    page.Footer()
                        .AlignCenter()
                        .Text( x => {
                            x.Span( "Page " );
                            x.CurrentPageNumber();
                        } );
                } );
            } )
            .GeneratePdf( "c:\\temp\\hello.pdf" );
        }

        public string RankDeltaFormatting( ResultEvent resultEvent, ResultListIntermediateFormatted rlf ) {
            return "";
        }
    }
}
