using OfficeOpenXml;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.Excel {
    public class ResultListExcel : ExcelGenerator<ResultList> {

        public ResultList? ResultList { get; private set; }

        public List<ResultListIntermediateFormatted>? WorksheetData { get; private set; }

        /// <summary>
        /// Private constructor, so users have to use the InitializeAsync method.
        /// </summary>
        private ResultListExcel() {
        }

        /// <summary>
        /// Factory method to construct a new instance of ResultListExcel based on the passed in Result List
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="squaddingList"></param>
        /// <returns></returns>
        public static async Task<ResultListExcel> FactoryAsync( ResultList resultList, SquaddingList? squaddingList = null ) {
            var rle = new ResultListExcel();
            rle.ResultList = resultList;

            rle.WorksheetData = new List<ResultListIntermediateFormatted>();

            //For worksheet 1, use the defined RESULT LIST FORMAT for the passed in Result List
            var resultListFormatSetName = await ResultListFormatFactory.FACTORY.GetResultListFormatSetNameAsync( resultList );
            var rlfWs1 = await DefinitionCache.GetResultListFormatDefinitionAsync( resultListFormatSetName );

            ResultListIntermediateFormatted worksheet1 = new ResultListIntermediateFormatted( resultList, rlfWs1, null );
            await worksheet1.InitializeAsync();
            rle.WorksheetData.Add( worksheet1 );

            //For workseet 2, use a dynamically created Essential Data File RESULT LIST FORMAT
            var essentialDataFile = new EssentialDataFile();
            var rlfWs2 = await essentialDataFile.GenerateAsync( resultList );
            ResultListIntermediateFormatted worksheet2 = new ResultListIntermediateFormatted( resultList, rlfWs2, null );
            await worksheet2.InitializeAsync();
            //If the user passed in a squadding list, we can use it. Otherwise, try and load it from REST API
            if (squaddingList is not null)
                worksheet2.LoadSquaddingList( squaddingList );
            else
                await worksheet2.LoadSquaddingListAsync();
            rle.WorksheetData.Add( worksheet2 );

            return rle;
        }

        /// <inheritdoc />
        public override byte[] GenerateExcel( string? filePath = null ) {
            using (var package = new ExcelPackage()) {
                foreach (var rlif in this.WorksheetData) {
                    var worksheet = package.Workbook.Worksheets.Add( $"{this.ResultList.Name}: {rlif.ResultListFormat.CommonName}" );
                    PopulateWorksheet( worksheet, rlif );
                }

                if (filePath != null) {
                    package.SaveAs( filePath );
                }

                // Setting the buffer to hold the bytes of the Excel file
                var buffer = package.GetAsByteArray();

                return buffer;

                /*
                //I think I should simply return the buffer, then the website handles displaying it.
                return Convert.ToBase64String( buffer );
                */
            }
        }

        protected static void PopulateWorksheet( ExcelWorksheet worksheet, ResultListIntermediateFormatted rlif ) {
            int columnIndex = 1;
            int rowIndex = 1;

            foreach (var headerCell in rlif.GetShownHeaderRow()) {
                worksheet.Cells[rowIndex, columnIndex].Value = headerCell.Text;
                worksheet.Cells[rowIndex, columnIndex].Style.Font.Bold = true;
                columnIndex++;
            }

            foreach (var multilineRow in rlif.Rows) {
                foreach (var row in multilineRow) {
                    rowIndex++;
                    columnIndex = 1;
                    foreach (var cell in row.GetShownRow()) {
                        worksheet.Cells[rowIndex, columnIndex].Value = ParseSmart( cell.Text );
                        columnIndex++;
                    }
                }
            }
        }

        protected static object ParseSmart( string input ) {
            if (int.TryParse( input, out int intValue ))
                return intValue;

            if (float.TryParse( input, out float floatValue ))
                return floatValue;

            return input;
        }
    }
}
