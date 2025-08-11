using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.PDF;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
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
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Content().Column(column =>
                    {

                        column.Spacing(10);
                        column.Item().Element(ReportTitle);
                        column.Spacing(10);
                        column.Item().Element(SvgImages);
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
                    column.Item().Text(ResultCOF.Participant.DisplayName).SemiBold().FontSize(16).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                    column.Item().Text($"{ResultCOF.MatchName} | {StringFormatting.SingleDate(ResultCOF.LocalDate)}").SemiBold().FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                    column.Item().Text($"Course of Fire: {CourseOfFire.CommonName}").SemiBold().FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                    column.Item().Text($"Status: {ResultCOF.Status} | Printed at {StringFormatting.SingleDateTime(DateTime.Now)}").FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                });
            });
        }

        protected void SvgImages(IContainer container)
        {

            // this will be where target images are made, then adding in the NPA maths.
            TargetSVGCreator targetSVG = new TargetSVGCreator();
            //need to have a call for each stage.
            targetSVG.TargetSVGCreatorAsync(480f, "Kneeling", null, this.ResultCOF);
            string svg = null;
            while (svg == null)
            {
                svg = targetSVG.GetSVGMarkup();
            }
            //SO QuestPDF does NOT SUPPORT CSS Styling, all elemnts MUST include Fill and Stroke items.
            svg = targetSVG.SvgWithoutCSS(svg);

            container.Border(2, ScoposColors.BLUE_LIGHTEN_1)
            .Background(ScoposColors.DARK_GREY_LIGHTEN_1)
            .CornerRadius(5)
            .Padding(10)
            .Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Svg(svg).FitArea();
                });
            });
        }
    }
}
