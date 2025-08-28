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

        public Target TargetDef = null;

        private List<EventInfoObject> EventFields { get; set; } = [];

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

            FillEventFields();

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
                        column.Item().Element(GenericRcof);
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
            container.Border( 2 )
			//.Border( 2, ScoposColors.BLUE_LIGHTEN_1 )
			//.Background( ScoposColors.DARK_GREY_LIGHTEN_1 )
			.BorderLinearGradient( 45, [ScoposColors.BLUE_LIGHTEN_2, ScoposColors.DARK_GREY_LIGHTEN_1] )
			.BackgroundLinearGradient( 45, [ScoposColors.DARK_GREY, ScoposColors.BLUE_LIGHTEN_1] )
            .CornerRadius(5)
            .Padding(10)
            .Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().ShrinkVertical().Row(row =>
                    {
                        row.RelativeItem(2).AlignLeft().Text(ResultCOF.Participant.DisplayName).SemiBold().FontSize(16).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                        row.RelativeItem(1).AlignRight().Text(text =>{
                            text.Span($"{TopLevelEvent.EventName} : {ResultCOF.EventScores[TopLevelEvent.EventName].ScoreFormatted}")
                                .SemiBold()
                                .FontSize(16)
                                .FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                        });
                    });
                    column.Item().Text($"{ResultCOF.MatchName} | {StringFormatting.SingleDate(ResultCOF.LocalDate)}").SemiBold().FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                    column.Item().Text($"Course of Fire: {CourseOfFire.CommonName}").SemiBold().FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                    column.Item().Text($"Status: {ResultCOF.Status} | Printed at {StringFormatting.SingleDateTime(DateTime.Now)}").FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                });
            });
        }

        /// <summary>
        /// This arraingement works best for EventtType.SERIES as that is typically 6 x 10-shot series which displays nicely.
        /// </summary>
        /// <param name="container"></param>
        protected void GenericRcof(IContainer container)
        {
            int eventIndex = 0;

            var ShotTableDetails = new ShotTableDetails
            {
                ShotEventFields = EventFields,
                CurrentEventIndex = eventIndex,
                ShowTotal = false
            };
            var TargetImageDetails = new TargetImageDetails
            {
                TargetDef = TargetDef,
                ImageEventFields = EventFields,
                CurrentEventIndex = eventIndex
            };

            container.Border(2, ScoposColors.BLUE_LIGHTEN_1)
            .CornerRadius(5)
            .Padding(1)
            .Column(column =>
            {
                bool nextRow = false;
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
                        nextRow = false;
                        ShotTableDetails.ShowTotal = false;
                        foreach (int ro in Enumerable.Range(1, 2))
                        {
                            if (EventFields[eventIndex].eventComposite.EventType != this.EventtType || nextRow)
                            {
                                //failfirst!
                                if (!nextRow)
                                {
                                    eventIndex++;
                                    //need to add filler row.col parts to make it think it's fine.
                                    row.RelativeItem(2).Column(col1 => { });
                                    row.RelativeItem(1).Column(col2 => { });
                                }
                                if (eventIndex >= EventFields.Count())
                                    break;
                                continue;
                            }

                            TargetImageDetails.CurrentEventIndex = eventIndex;
                            row.RelativeItem(2).MaxWidth(180).BorderLeft(2).BorderTop(2).BorderColor(ScoposColors.BLUE_LIGHTEN_1).Component(new TargetImage(TargetImageDetails));

                            //maybe make this part a table, if less than 10 shots in this list them with (x,y,r) and value?
                            ShotTableDetails.CurrentEventIndex = eventIndex;
                            var shotTablesNumber = (int)Math.Ceiling((double)EventFields[eventIndex].ShotList.Count() / (double)ShotTableDetails.MaxShotNumber);
                            if (shotTablesNumber > 1)
                            {
                                nextRow = true;
                            }
                            foreach (var number in Enumerable.Range(1, shotTablesNumber))
                            {
                                if(number == 6 || number == shotTablesNumber)
                                {
                                    ShotTableDetails.ShowTotal = true;
                                }
                                if(number <= 6)
                                {
                                    ShotTableDetails.ShotNumberToStartOn = ShotTableDetails.MaxShotNumber * (number - 1);
                                    row.RelativeItem(1).BorderRight(2).BorderColor(ScoposColors.BLUE_LIGHTEN_1)
                                        .Container().BorderLeft(2).BorderColor(ScoposColors.DARK_GREY_LIGHTEN_2)
                                        .Component(new ShotTable(ShotTableDetails));
                                }
                            }
                            //total should be the total of the targets shown, maybe put the total of the target on the target image?
                            eventIndex++;
                            if (nextRow)
                                break;
                            if (eventIndex >= EventFields.Count())
                                break;
                        }
                    });

                    if (eventIndex >= EventFields.Count())
                        break;
                }
            });
        }

        protected byte[] AnalyzeTargetReturnPng(string name, int width, int height, out List<Shot> shot, out string eventScore, out GroupAnalysisMaths groupMaths, out Target targetDef)
        {
            // this will be where target images are made, then adding in the NPA maths.
            TargetAnalysisAndGraphic targetAnalysis = new TargetAnalysisAndGraphic();
            //need to have a call for each stage.
            targetAnalysis.TargetSVGCreatorAsync(480f, name, null, this.ResultCOF,true);
            string svg = null;
            while (svg == null)
            {
                svg = targetAnalysis.GetSVGMarkup();
            }
            //SO QuestPDF does NOT SUPPORT CSS Styling, all elemnts MUST include Fill and Stroke items.
            // ^ Does not matter, we are converting immediately into a PNG
            svg = targetAnalysis.SvgWithoutCSS(svg, width, height);
            svg = targetAnalysis.MakePrinterFriendly(svg);
            var png = targetAnalysis.ConvertSvgToPng(svg, width, height);
            

            eventScore = targetAnalysis.GetScoreFormatted();
            shot = targetAnalysis.GetShotListToShow();
            groupMaths = targetAnalysis.groupMaths;
            targetDef = targetAnalysis.TargetDef;

            return png;
        }

        private void FillEventFields()
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
                    Target targetDef = null;
                    //TODO:
                    // write a new TargetAnalysisAndGraph Class
                    // Construct new Obj, passing in RCOF, event to look at.
                    // event fields will be a list of target analysis.
                    
                    var img = AnalyzeTargetReturnPng(eventToHighlight.EventName, 480, 480, out shots, out eventScore, out groupMaths, out targetDef);
                    TargetDef = targetDef;

                    EventInfoObject newEvent = new EventInfoObject()
                    {
                        eventComposite = eventToHighlight,
                        ObjectImage = img,
                        GroupMaths = groupMaths,
                        ShotList = shots,
                        ScoreFormatted = eventScore,
                        EventLabel = eventToHighlight.EventName
                    };
                    EventFields.Add(newEvent);
                }
            }
        }

    }


    public class ShotTableDetails
    {
        //I think the original is only being instantiated after the EventFields thing is written, but should be same stuff here.
        public string TextColor { get; set; } = "#000000";
        public string InnerBorderColors { get; set; } = ScoposColors.DARK_GREY_LIGHTEN_2;

        public bool ShowTotal { get; set; } = false;

        public int TableFontSize { get; set; } = 12;

        public int MaxShotNumber { get; set; } = 10;

        public int ShotNumberToStartOn { get; set; } = 0;

        public int CurrentEventIndex { get; set; } = 0;

        public List<EventInfoObject> ShotEventFields { get; set; } = [];
    }

    public class ShotTable : IComponent
    {
        public ShotTableDetails Details { get; }

        public ShotTable(ShotTableDetails details)
        {
            Details = details;
        }

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                });

                table.Header(header =>
                {
                    header.Cell().Element(Style).Text("#").FontSize(Details.TableFontSize);
                    header.Cell().Element(Style).Text("Score").FontSize(Details.TableFontSize);

                    IContainer Style(IContainer container)
                    {
                        return container
                            .Padding(1)
                            .DefaultTextStyle(TextStyle.Default.FontColor(Details.TextColor).Bold());
                    }
                });

                List<Shot> shotListCopy = Details.ShotEventFields[Details.CurrentEventIndex].ShotList;
                List<Shot> shotListSection = shotListCopy;
                if (shotListCopy.Count() > Details.MaxShotNumber)
                    shotListSection = shotListCopy.GetRange(Details.ShotNumberToStartOn, Details.MaxShotNumber);

                table.ExtendLastCellsToTableBottom();
                foreach (var shot in shotListSection)
                {
                    table.Cell().Element(Style).Text($"{shot.EventName}").FontSize(Details.TableFontSize);
                    table.Cell().Element(Style).Text($"{shot.ScoreFormatted}").FontSize(Details.TableFontSize);
                }

                if(Details.ShowTotal)
                {
                    table.Cell().Element(FooterStyle).ExtendVertical().AlignMiddle().Text("Total").FontSize(Details.TableFontSize);
                    table.Cell().Element(FooterStyle).ExtendVertical().AlignMiddle().Text($"{Details.ShotEventFields[Details.CurrentEventIndex].ScoreFormatted}").FontSize(Details.TableFontSize);
                }
                else
                {
                    table.Cell().Element(FooterStyle).ExtendVertical().AlignMiddle().Text("").FontSize(Details.TableFontSize);
                    table.Cell().Element(FooterStyle).ExtendVertical().AlignMiddle().Text("").FontSize(Details.TableFontSize);
                }

                IContainer Style(IContainer container)
                {
                    return container
                        .BorderTop(2)
                        .BorderColor(Details.InnerBorderColors)
                        .Padding(1);
                }

                IContainer FooterStyle(IContainer container)
                {
                    return container
                        .BorderTop(2)
                        .BorderColor(Details.InnerBorderColors)
                        .Padding(1)
                        .DefaultTextStyle(TextStyle.Default.FontColor(Details.TextColor).Bold());
                }

            });
        }
    }

    public class TargetImageDetails
    {
        public int GroupInfoFontSize { get; set; } = 7;

        public Target TargetDef { get; set; } = null;

        public int CurrentEventIndex { get; set; } = 0;

        public List<EventInfoObject> ImageEventFields { get; set; } = [];
    }

    public class TargetImage : IComponent
    {
        public TargetImageDetails Details { get; }

        public TargetImage(TargetImageDetails details)
        {
            Details = details;
        }

        public void Compose(IContainer container)
        {
            container.Column(col1 =>
            {
                //this one is the image and group analysis
                col1.Item().BorderRight(2).BorderBottom(2).BorderColor(ScoposColors.DARK_GREY_LIGHTEN_2)
                .ScaleToFit().Image(Details.ImageEventFields[Details.CurrentEventIndex].ObjectImage);
                //this should always be overall score of the shown target.
                col1.Item().ExtendVertical().BorderRight(2).BorderColor(ScoposColors.DARK_GREY_LIGHTEN_2)
                .AlignCenter().AlignMiddle().Text(text =>
                {
                    if(Details.ImageEventFields[Details.CurrentEventIndex].GroupMaths != null)
                    {
                        text.Span($"Area: {Details.ImageEventFields[Details.CurrentEventIndex].GroupMaths.GetArea().ToString("F")}mm").FontSize(Details.GroupInfoFontSize);
                        text.Span("2").FontSize(Details.GroupInfoFontSize).Superscript();
                        text.Span($"    Round: {Details.ImageEventFields[Details.CurrentEventIndex].GroupMaths.GetRoundness().ToString("F")}\n").FontSize(Details.GroupInfoFontSize);
                        if (Details.TargetDef.Distance != null)
                        {
                            var widestMM = Details.ImageEventFields[Details.CurrentEventIndex].GroupMaths.GetDistanceBetweenWidestShots();
                            var distanceInMeter = (double)Details.TargetDef.Distance / 1000D;
                            //we want to use radius of widest circle
                            var spreadMoa = Math.Atan((widestMM / 2D) / distanceInMeter);
                            text.Span($"MOA: {spreadMoa.ToString("F")}    ").FontSize(Details.GroupInfoFontSize);
                        }
                        text.Span($"Center: ({Details.ImageEventFields[Details.CurrentEventIndex].GroupMaths.GetCenterX().ToString("F")}mm, {Details.ImageEventFields[Details.CurrentEventIndex].GroupMaths.GetCenterY().ToString("F")}mm)").FontSize(Details.GroupInfoFontSize);
                    }
                });
            });
        }
    }
}
