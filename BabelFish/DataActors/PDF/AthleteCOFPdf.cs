using NLog.Filters;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.PDF;
using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using SkiaSharp;
using Svg.Skia;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Scopos.BabelFish.DataActors.PDF
{
    public class AthleteCOFPdf : PdfGenerator
    {

        public ResultCOF ResultCOF { get; private set; }

        public EventtType EventtType { get; private set; }

        public CourseOfFire? CourseOfFire { get; private set; } = null;
        public EventComposite TopLevelEvent { get; private set; } = null;

        private List<(string label, string score, byte[] img, List<Shot> shots, GroupAnalysisMaths groupMaths, EventComposite eventComposite)> EventFields { get; set; } = [];

        public AthleteCOFPdf(ResultCOF resultCOF, EventtType et)
        {
            this.ResultCOF = resultCOF;
            this.EventtType = et;
        }

        public async Task InitializeAsync()
        {
            CourseOfFire = await DefinitionCache.GetCourseOfFireDefinitionAsync(SetName.Parse(this.ResultCOF.CourseOfFireDef));
            TopLevelEvent = EventComposite.GrowEventTree(this.CourseOfFire);
        }

        protected override string Title
        {
            get
            {
                return $"Results for {ResultCOF.Participant.DisplayName}";
            }
        }

        public override QuestPDF.Fluent.Document GeneratePdf(PageSize pageSize, string filePath)
        {
            if (CourseOfFire == null)
                throw new InitializeAsyncNotCompletedException();

            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(pageSize);
                    page.Margin(1f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Content().Column(column =>
                    {
                        column.Item().Element(ReportTitle);
                        column.Spacing(5);
                        column.Item().Element(PrettyRcof);
                        //column.Item().Width(50).Height(50).Svg(svg).FitArea();
                    });

                    page.Footer().Element(Footer);
                });
            });

            InsertMetaData(document);

            if (!string.IsNullOrEmpty(filePath))
                document.GeneratePdf(filePath);

            return document;
        }

        protected override void ReportTitle(IContainer container)
        {
            container.Border(2, ScoposColors.BLUE_LIGHTEN_1)
            .Background(ScoposColors.DARK_GREY_LIGHTEN_1)
            .CornerRadius(5)
            .Padding(10)
            .Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().ShrinkVertical().Row(row =>
                    {
                        row.RelativeItem(2).AlignLeft().Text(ResultCOF.Participant.DisplayName).SemiBold().FontSize(16).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                        //FYI need to get the top level even score.
                        row.RelativeItem(1).AlignRight().Text(ResultCOF.EventScores[TopLevelEvent.EventName].ScoreFormatted).SemiBold().FontSize(16).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                    });
                    column.Item().Text($"{ResultCOF.MatchName} | {StringFormatting.SingleDate(ResultCOF.LocalDate)}").SemiBold().FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                    column.Item().Text($"Course of Fire: {CourseOfFire.CommonName}").SemiBold().FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                    column.Item().Text($"Status: {ResultCOF.Status} | Printed at {StringFormatting.SingleDateTime(DateTime.Now)}").FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                });
            });
        }

        protected void PrettyRcof(IContainer container)
        {

            foreach (var eventType in Enum.GetValues(typeof(EventtType)).Cast<EventtType>().ToList())
            {
                if (eventType == EventtType.NONE || eventType == EventtType.SINGULAR)
                    continue;
                var eventsToHighlight = TopLevelEvent.GetEvents(eventType);
                //give me only the most important events.
                foreach (var eventToHighlight in eventsToHighlight)
                {
                    //getting target image will also set a bunch of other data, that we will also get here lol
                    var shots = new List<Shot>();
                    string eventScore = "";
                    GroupAnalysisMaths groupMaths = null;
                    var img = TargetImage(eventToHighlight.EventName, 480, 480, out shots, out eventScore, out groupMaths);

                    EventFields.Add(( eventToHighlight.EventName, eventScore, img, shots, groupMaths, eventToHighlight));
                }
            }

            /*
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    header.Cell().Element(Style).Text("Target");
                    header.Cell().Element(Style).Text("Description");

                    IContainer Style(IContainer container)
                    {
                        return container
                            .Background(Colors.Blue.Lighten5)
                            .Padding(10)
                            .DefaultTextStyle(TextStyle.Default.FontColor(Colors.Blue.Darken4).Bold());
                    }
                });

                foreach ((var name, var score) in EventFields)
                {
                    int size = 100;
                    table.Cell().Element(Style).MaxHeight(size).MaxWidth(size).Element(TargetSVG(name, size));

                    table.Cell().Element(Style).Text($"this is {name}");
                }
                IContainer Style(IContainer container)
                {
                    return container
                    .BorderTop(2)
                    .BorderColor(Colors.Blue.Lighten3)
                    .Padding(10);
                }
            });
            */

            int eventIndex = 0;
            container.Border(2, ScoposColors.BLUE_LIGHTEN_1)
            .CornerRadius(5)
            .Padding(1)
            .Column(column =>
            {
                foreach ( int col in Enumerable.Range( 1, (EventFields.Count()) ) )
                {
                    if (EventFields[eventIndex].eventComposite.EventType != this.EventtType)
                    {
                        //failfirst!
                        eventIndex++;
                        if (eventIndex >= EventFields.Count())
                            break;
                        continue;
                    }
                    int rowHeight = 205;
                    column.Item().Container().Height(rowHeight).Border(2, ScoposColors.BLUE_LIGHTEN_1).Row(row =>
                    {
                        foreach (int ro in Enumerable.Range(1, 2))
                        {
                            if (EventFields[eventIndex].eventComposite.EventType != this.EventtType)
                            {
                                //failfirst!
                                eventIndex++;
                                //need to add filler row.col parts to make it think it's fine.
                                row.RelativeItem(2).Column(col1 =>{});
                                row.RelativeItem(1).Column(col2 =>{});

                                if (eventIndex >= EventFields.Count())
                                    break;
                                continue;
                            }

                            row.RelativeItem(2).Column(col1 =>
                            {
                                //this one is the image and group analysis
                                col1.Item().BorderRight(2).BorderBottom(2).BorderColor(ScoposColors.DARK_GREY_LIGHTEN_2)
                                .Container()
                                .BorderLeft(2).BorderTop(2).BorderColor(ScoposColors.BLUE_LIGHTEN_1)
                                .ScaleToFit().Image(EventFields[eventIndex].img);
                                //this should always be overall score of the shown target.
                                int tinyFontSize = 7;
                                col1.Item().ExtendVertical().BorderRight(2).BorderColor(ScoposColors.DARK_GREY_LIGHTEN_2)
                                .AlignCenter().AlignMiddle().Text(text => 
                                {
                                    text.Span($"Area: {Math.Round(EventFields[eventIndex].groupMaths.GetArea(), 2)}mm").FontSize(tinyFontSize);
                                    text.Span("2").FontSize(tinyFontSize).Superscript();
                                    text.Span($"    Round: {Math.Round(EventFields[eventIndex].groupMaths.GetRoundness(), 2)}\n").FontSize(tinyFontSize);
                                    text.Span($"Center: ({Math.Round(EventFields[eventIndex].groupMaths.GetCenterX(), 2)}mm, {Math.Round(EventFields[eventIndex].groupMaths.GetCenterY(), 2)}mm)").FontSize(tinyFontSize);
                                });
                            });
                            
                            //maybe make this part a table, if less than 10 shots in this list them with (x,y,r) and value?
                            row.RelativeItem(1).BorderRight(2).BorderColor(ScoposColors.BLUE_LIGHTEN_1).Column(col2 =>
                            {
                                col2.Item().Table(table =>
                                {
                                    int tableFontSize = 12;
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(1);
                                        columns.RelativeColumn(1);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(Style).Text("#").FontSize(tableFontSize);
                                        header.Cell().Element(Style).Text("Score").FontSize(tableFontSize);

                                        IContainer Style(IContainer container)
                                        {
                                            return container
                                                .Padding(1)
                                                .DefaultTextStyle(TextStyle.Default.FontColor("#000000").Bold());
                                        }
                                    });

                                    int limit = 20;
                                    List<Shot> shotListCopy = EventFields[eventIndex].shots;

                                    table.ExtendLastCellsToTableBottom();
                                    foreach (var shot in shotListCopy)
                                    {
                                        if (limit == 0)
                                        {
                                            break;
                                        }

                                        table.Cell().Element(Style).Text($"{shot.EventName}").FontSize(tableFontSize);
                                        table.Cell().Element(Style).Text($"{shot.ScoreFormatted}").FontSize(tableFontSize);

                                        //table.Cell().Element(Style).Text($"{shot.Location.GetXToString()}").FontSize(tableFontSize);
                                        //table.Cell().Element(Style).Text($"{shot.Location.GetYToString()}").FontSize(tableFontSize);

                                        //table.Cell().Element(Style).Text($"{shot.Location.GetRadiusToString()}").FontSize(tableFontSize);
                                        //table.Cell().Element(Style).Text($"{Math.Round(shot.Location.GetAngle()*(180/Math.PI))}").FontSize(tableFontSize);

                                        limit--;
                                    }

                                    table.Cell().Element(FooterStyle).ExtendVertical().AlignMiddle().Text("Total").FontSize(tableFontSize);
                                    table.Cell().Element(FooterStyle).ExtendVertical().AlignMiddle().Text($"{EventFields[eventIndex].score}").FontSize(tableFontSize);

                                    IContainer Style(IContainer container)
                                    {
                                        return container
                                            .BorderTop(2)
                                            .BorderColor(ScoposColors.DARK_GREY_LIGHTEN_2)
                                            .Padding(1);
                                    }

                                    IContainer FooterStyle(IContainer container)
                                    {
                                        return container
                                            .BorderTop(2)
                                            .BorderColor(ScoposColors.DARK_GREY_LIGHTEN_2)
                                            .Padding(1)
                                            .DefaultTextStyle(TextStyle.Default.FontColor("#000000").Bold());
                                    }

                                });
                            });
                            eventIndex++;
                            if (eventIndex >= EventFields.Count())
                                break;
                        }
                    });

                    if (eventIndex >= EventFields.Count())
                        break;
                }
            });
        }

        protected byte[] TargetImage(string name, int width, int height, out List<Shot> shot, out string eventScore, out GroupAnalysisMaths groupMaths)
        {
            // this will be where target images are made, then adding in the NPA maths.
            TargetSVGCreator targetSVG = new TargetSVGCreator();
            //need to have a call for each stage.
            targetSVG.TargetSVGCreatorAsync(480f, name, null, this.ResultCOF,true);
            string svg = null;
            while (svg == null)
            {
                svg = targetSVG.GetSVGMarkup();
            }
            //SO QuestPDF does NOT SUPPORT CSS Styling, all elemnts MUST include Fill and Stroke items.
            svg = targetSVG.SvgWithoutCSS(svg, width, height);
            svg = targetSVG.MakePrinterFriendly(svg);
            var png = targetSVG.ConvertSvgToPng(svg, width, height);
            

            eventScore = targetSVG.GetScoreFormatted();
            shot = targetSVG.GetShotListToShow();
            groupMaths = targetSVG.groupMaths;

            return png;
        }
    }
}
