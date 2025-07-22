using System;
using System.Collections.Generic;
using System.Text;
using QuestPDF.Helpers;

namespace Scopos.BabelFish.DataActors.PDF {
    public interface IGeneratePdf {

        void GeneratePdf(PageSize pageSize);
    }
}
