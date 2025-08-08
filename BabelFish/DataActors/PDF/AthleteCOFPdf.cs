using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Scopos.BabelFish.DataActors.PDF;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataActors.PDF
{
    class AthleteCOFPdf : PdfGenerator
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

        protected override string Title
        {
            get
            {
                return $"Results for {ResultCOF.Participant.DisplayName}";
            }
        }

        public override QuestPDF.Fluent.Document GeneratePdf(PageSize pageSize, string filePath)
        {
            // this will be where target images are made, then adding in the NPA maths.
            TargetSVGCreator targetSVG = new TargetSVGCreator();
            await targetSVG.TargetSVGCreatorAsync(pageSize.Width / 2D, null, null, null);
            throw new NotImplementedException();
        }

        protected override void ReportTitle(IContainer container)
        {
            container.Border(2, ScoposColors.BLUE_LIGHTEN_1)
            .Background(ScoposColors.DARK_GREY_LIGHTEN_1)
            .CornerRadius(5)
            .Padding(10)
            .Row(row => {
                row.RelativeItem().Column(column => {
                    column.Item().Text(ResultCOF.Participant.DisplayName).SemiBold().FontSize(16).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                    column.Item().Text($"{ResultCOF.MatchName} | {StringFormatting.SingleDate(ResultCOF.LocalDate)}").SemiBold().FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                    column.Item().Text($"Course of Fire: {CourseOfFire.CommonName}").SemiBold().FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                    column.Item().Text($"Status: {ResultCOF.Status} | Printed at {StringFormatting.SingleDateTime(DateTime.Now)}").FontSize(12).FontColor(ScoposColors.LIGHT_GREY_LIGHTEN_3);
                });
            });
        }
    }
}
