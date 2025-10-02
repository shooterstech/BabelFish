using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Scopos.BabelFish.DataActors.Excel
{
    public abstract class ExcelGenerator
    {
        public abstract ExcelPackage GenerateExcel(List<string> data, string filePath);
    }
}
