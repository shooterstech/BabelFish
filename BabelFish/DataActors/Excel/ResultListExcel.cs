using OfficeOpenXml;
using OfficeOpenXml.Style;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Scopos.BabelFish.DataActors.Excel
{
    public class ResultListExcel : ExcelGenerator
    {

        private ResultList ResultList { get; set; } = null;

        private List<string> resultEventsToInclude = new List<string>();

        private List<string> resultEventsNamesToInclude = new List<string>();

        private EventComposite eventTree = null;

        private ExcelWorksheet? Worksheet1 = null;
        private ExcelWorksheet? Worksheet2 = null;

        public ResultListExcel(ResultList resultList)
        {
            CreateEventTreeAsync(resultList);
            while (this.ResultList == null) ; // what's the right way to do this??
        }

        private async void CreateEventTreeAsync(ResultList resultList)
        {

            #region CreateEventTree

            SetName cofSetName;
            CourseOfFire? courseOfFireDefinition = null;
            string courseOfFireCommonName;
            if (resultList.CourseOfFireDef != "v1.0:orion:unknown" && SetName.TryParse(resultList.CourseOfFireDef, out cofSetName))
            {
                courseOfFireDefinition = await DefinitionCache.GetCourseOfFireDefinitionAsync(cofSetName);
                courseOfFireCommonName = courseOfFireDefinition.CommonName;

                eventTree = EventComposite.GrowEventTree(courseOfFireDefinition);
            }


            if (resultList.Projected)
            {
                CompareResultByRank.CompareMethod method = CompareResultByRank.CompareMethod.RANK_ORDER;
                SortBy sortBy = SortBy.ASCENDING;
                CompareResultByRank compareResultByRank = new CompareResultByRank(method, sortBy);
                resultList.Items.Sort(compareResultByRank);

            }


            //Add to the lists to display, if there are any results
            if (resultList.Items.Count > 0 && eventTree != null)
            {
                //add only the EVENT, STAGE and SERIES events in the event tree to be shown on the sheet
                foreach (var item in eventTree.GetEvents(true, true, true, true, true, false))
                {
                    resultEventsToInclude.Add(item.EventName);
                    resultEventsNamesToInclude.Add(item.EventName + " - DEC");
                    resultEventsNamesToInclude.Add(item.EventName + " - INT");
                    resultEventsNamesToInclude.Add(item.EventName + " - X");
                }
            }
            else
            {
                //we need to throw some error here, though I don't think this will ever be hit if the RL doesn't load.
                ;
            }
            #endregion

            this.ResultList = resultList;
        }

        public override string GenerateExcel(string ? filePath = null)
        {
            using (var package = new ExcelPackage())
            {
                Worksheet1 = package.Workbook.Worksheets.Add(this.ResultList.Status + " ResultList");
                List<string> header = new List<string>();
                if (this.ResultList.Team)
                {
                    //Creates 2 worksheets, #one for teams only, #two for individuals only.
                    Worksheet2 = package.Workbook.Worksheets.Add("Individuals");
                    List<string>? header2 = new List<string>();

                    header.Add("Name");
                    header.Add("Rank");
                    List<string> staticVars = new List<string>();
                    staticVars.AddRange(header);
                    header.AddRange(resultEventsNamesToInclude);
                    Worksheet1 = PopulateHeader(Worksheet1, header);
                    List<ExcelResultEvent> teams = new List<ExcelResultEvent>();
                    foreach (var team in this.ResultList.Items)
                    {
                        Dictionary<string, string> data = new Dictionary<string, string>();
                        data.Add(header[0], team.Participant.DisplayName);
                        data.Add(header[1], team.Rank.ToString());

                        ExcelResultEvent teamExcel = new ExcelResultEvent(team, data);
                        teams.Add(teamExcel);
                    }
                    Worksheet1 = PopulateData(Worksheet1, teams, resultEventsToInclude, staticVars);

                    header2.Add("Name");
                    header2.Add("Competitor Number");
                    header2.Add("Rank");
                    header2.Add("Team");
                    List<string> staticVars2 = new List<string>();
                    staticVars2.AddRange(header2);
                    header2.AddRange(resultEventsNamesToInclude);
                    Worksheet2 = PopulateHeader(Worksheet2, header2);
                    //make a long list of all team members and pass it to this one.
                    // how get what team they are in??
                    //  could populate one team at a time? that could work....
                    //   would need to keep track of start/stop and team name
                    List<ExcelResultEvent> individuals = new List<ExcelResultEvent>();
                    foreach (var team in ResultList.Items)
                    {
                        if (team.TeamMembers != null)
                        {
                            foreach (var teamMember in team.TeamMembers)
                            {
                                Dictionary<string, string> data = new Dictionary<string, string>();
                                data.Add(header2[0], teamMember.Participant.DisplayName);
                                data.Add(header2[1], teamMember.Participant.CompetitorNumber);
                                data.Add(header2[2], team.Rank.ToString()); // choosing to use team rank here, though they are in order of score by team
                                data.Add(header2[3], team.Participant.DisplayName);

                                ExcelResultEvent teamExcel = new ExcelResultEvent(teamMember, data);
                                individuals.Add(teamExcel);
                            }
                        }
                    }

                    Worksheet2 = PopulateData(Worksheet2, individuals, resultEventsToInclude, staticVars2);

                    Worksheet1.Cells[Worksheet1.Dimension.Address].AutoFitColumns();
                    Worksheet1.View.FreezePanes(2, 1);

                    Worksheet2.Cells[Worksheet2.Dimension.Address].AutoFitColumns();
                    Worksheet2.View.FreezePanes(2, 1);
                }
                else
                {
                    //creates only one worksheet for individuals
                    header.Add("Name");
                    header.Add("Competitor Number");
                    header.Add("Rank");
                    List<string> staticVars = new List<string>();
                    staticVars.AddRange(header);
                    header.AddRange(resultEventsNamesToInclude);
                    Worksheet1 = PopulateHeader(Worksheet1, header);
                    List<ExcelResultEvent> individuals = new List<ExcelResultEvent>();
                    foreach (var person in ResultList.Items)
                    {
                        Dictionary<string, string> data = new Dictionary<string, string>();
                        data.Add(header[0], person.Participant.DisplayName);
                        data.Add(header[1], person.Participant.CompetitorNumber);
                        data.Add(header[2], person.Rank.ToString());

                        ExcelResultEvent PersonExcel = new ExcelResultEvent(person, data);
                        individuals.Add(PersonExcel);
                    }
                    Worksheet1 = PopulateData(Worksheet1, individuals, resultEventsToInclude, staticVars);

                    Worksheet1.Column(3).Style.Numberformat.Format = "0.00";
                    Worksheet1.Cells[Worksheet1.Dimension.Address].AutoFitColumns();
                    Worksheet1.View.FreezePanes(2, 1);
                }

                if(filePath != null)
                {
                    package.SaveAs(filePath);
                }
                // Setting the buffer to hold the bytes of the Excel file
                var buffer = package.GetAsByteArray();

                //I think I should simply return the buffer, then the website handles displaying it.
                return Convert.ToBase64String(buffer);
            }
        }

        private ExcelWorksheet PopulateHeader(ExcelWorksheet worksheet, List<string> headers)
        {
            using (var range = worksheet.Cells[1, headers.Count])
            {
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
            }

            int currentCol = 1;
            foreach (string colName in headers)
            {
                worksheet.Cells[1, currentCol].Value = colName;
                currentCol++;
            }
            return worksheet;
        }


        private ExcelWorksheet PopulateData(ExcelWorksheet worksheet, List<ExcelResultEvent> excelResultEvents, List<string> headers, List<string> staticVars)
        {
            //static column names are different than the ones expected in the ResultEvent
            // Adding Data
            int currentCol = 1;
            var currentRow = 2; // Starting from the second row because the first row is for headers
            foreach (var resultEvent in excelResultEvents)
            {
                //fill in the name/rank/compnum/that stuff here
                foreach (var data in staticVars)
                {
                    worksheet.Cells[currentRow, currentCol].Value = resultEvent.Data[data];
                    currentCol++;
                }
                foreach (var resultEventName in headers)
                {
                    if (resultEvent.ResultEvent.EventScores.ContainsKey(resultEventName))
                    {
                        // decimal
                        worksheet.Cells[currentRow, currentCol].Value = resultEvent.ResultEvent.EventScores[resultEventName].Score.D;
                        currentCol++;

                        // integer
                        worksheet.Cells[currentRow, currentCol].Value = resultEvent.ResultEvent.EventScores[resultEventName].Score.I;
                        currentCol++;

                        // inners
                        worksheet.Cells[currentRow, currentCol].Value = resultEvent.ResultEvent.EventScores[resultEventName].Score.X;
                        currentCol++;
                    }
                    else
                    {
                        // decimal
                        worksheet.Cells[currentRow, currentCol].Value = 0.0;
                        currentCol++;

                        // integer
                        worksheet.Cells[currentRow, currentCol].Value = 0;
                        currentCol++;

                        // inners
                        worksheet.Cells[currentRow, currentCol].Value = 0;
                        currentCol++;
                    }
                }
                currentRow++;
                currentCol = 1;
            }
            return worksheet;
        }
    }
    public class ExcelResultEvent
    {
        public ResultEvent ResultEvent { get; set; }

        public Dictionary<string, string> Data = new Dictionary<string, string>();

        public ExcelResultEvent(ResultEvent resultEvent, Dictionary<string, string> data)
        {
            ResultEvent = resultEvent;
            Data = data;
        }
    }
}
