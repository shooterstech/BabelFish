using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.Athena.Shot;
using System.Xml.XPath;

namespace Scopos.BabelFish.DataActors.PDF
{
    class TargetSVGCreator
    {


        //for this example of the child component, I'm not putting in a code behind file, just to show you how can do it not in code behind; I am moving every page to it's own partial class file now, best practice

        /// <summary>
        /// The width and height of the desired viewing box.
        /// </summary>
        public float Dimension { get; set; }


        /// <summary>
        /// The name of the event, within the Result COF to display
        /// </summary>
        public string? EventName { get; set; }

        public Match? Match { get; set; }

        /// <summary>
        /// this stores whatever html we want into a string
        /// </summary>
        private string SvgMarkup { get; set; }

        private bool SVGDone { get; set; } = false;

        private const string SvgNamespace = "http://www.w3.org/2000/svg";

        public ResultCOF? ResultCOF { get; set; }
        private EventComposite EventTree { get; set; }

        private EventComposite EventWeAreLookingFor { get; set; }

        private Dictionary<string, DataModel.Athena.Shot.Shot> ShotsByEventName { get; set; }
        private List<EventComposite> DescendantEventComposites;
        private Logger logger = LogManager.GetCurrentClassLogger();
        private string ErrorMessage = "";

        private List<string> ParentScores { get; set; } = new List<string>();

        private List<string> ChildScores { get; set; } = new List<string>();

        private string ScoreFormatted { get; set; } = string.Empty;

        public async void TargetSVGCreatorAsync(float dimension, string? eventName, Match? match, ResultCOF? resultCoF)
        {
            Dimension = dimension;
            EventName = eventName;
            Match = match;
            ResultCOF = resultCoF;
            SVGDone = false;
            await StartRender().ConfigureAwait(false);
            SVGDone = true;
        }

        public string? GetSVGMarkup()
        {
            if (SVGDone)
                return SvgMarkup;
            else
                return null;
        }

        private async Task StartRender()
        {

            try
            {
                //From the Result COF, learn the Course of Fire Def it was based on, and read that from the Rest API.
                var cofSetName = SetName.Parse(ResultCOF.CourseOfFireDef);
                var cof = await APIClients.DefinitionCache.GetCourseOfFireDefinitionAsync(cofSetName);

                //Using BabelFish, generate the EventTree from the Course of fire.
                EventTree = EventComposite.GrowEventTree(cof);
                //Retrieve the EventComposite from the event tree, for the event the caller is asking for.
                EventWeAreLookingFor = EventTree.FindEventComposite(EventName);
                //Generate a dictionary of shots for the event in question. Key is the singular shot name, value is the Shot object
                ShotsByEventName = ResultCOF.GetShotsByEventName();
                try
                {
                    DescendantEventComposites = EventWeAreLookingFor.GetAllSingulars();


                    //Start the drawing
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine($"<svg viewBox=\"0 0 {Dimension} {Dimension}\" xmlns=\"http://www.w3.org/2000/svg\" id=\"somethingUniquePerTargetYouWishToDisplay\">");
                    stringBuilder.AppendLine("<style> ");
                    stringBuilder.AppendLine(".small { ");
                    stringBuilder.AppendLine("font: italic 13px sans-serif; ");
                    stringBuilder.AppendLine("    } ");

                    stringBuilder.AppendLine(".heavy { ");
                    stringBuilder.AppendLine("font: bold 20px sans-serif; ");
                    stringBuilder.AppendLine("    } ");

                    stringBuilder.AppendLine(".shot { ");
                    stringBuilder.AppendLine("fill: rgb(79, 132, 190); ");
                    stringBuilder.AppendLine("    } ");

                    stringBuilder.AppendLine(" </style> ");
                    //Calculate the scale based on the widest shot in the group and the scoring diameter.
                    var widestShot = GetWidestShot();
                    double center = Dimension / 2D;
                    if (widestShot != null && widestShot.Location.GetRadiusSquared() > 0)
                    {
                        SetName targetSetName = SetName.Parse(widestShot.TargetSetName);

                        var shotRadius = widestShot.ScoringDiameter / 2D;

                        //All targets definitions *should* be the same from the Stage level down. Therefore it's safe (maybe) to use the same target definition througout.
                        var Target = await APIClients.DefinitionCache.GetTargetDefinitionAsync(targetSetName);

                        //Using divs, populate the even name and scores of each ancestor
                        PopulateParentScores(EventWeAreLookingFor);
                        //Using divs, populate the event name and score of each direct child.
                        PopulateChildScores();

                        double scale = Dimension / (2 * widestShot.Location.GetRadius() + widestShot.BulletDiameter);

                        //Draw the Aiming Mark
                        var aimingBlackRadius = Target.AimingMarks[0].Dimension / 2D;
                        stringBuilder.AppendLine($"<circle class=\"aiming-black\" cx=\"{center}\" cy=\"{center}\" r=\"{scale * aimingBlackRadius}\" stroke=\"black\" stroke-width=\".5\" fill=\"black\" />");


                        //Draw the shots
                        foreach (var shot in GetShotsToDisplay())
                        {
                            //Checking the shot attributes is the most predictable way to know if it's a missed shot withunkown coordinates.
                            if (shot.Attributes.Contains(Shot.SHOT_ATTRIBUTE_UNKNOWN_COORDINATES)
                                || shot.Attributes.Contains(Shot.SHOT_ATTRIBUTE_MISSED_SHOT)
                                //This is a less reliable way, but is needed for conversion from old version of Orion.
                                || (shot.Location.X == 0 && shot.Location.Y == 0 && shot.Score.I == 0))
                            {
                                //Don't draw the shot
                                ;
                            }
                            else
                            {
                                double x = center + scale * shot.Location.X;
                                double y = center - scale * shot.Location.Y;
                                //This draws the shots in the same way as drawn on the DoW100
                                stringBuilder.AppendLine($"<circle id=\"{shot.EventName.Replace(" ", "_").Trim()}_shot\" class=\"shot\" cx=\"{x}\" cy=\"{y}\" r=\"{scale * 2.25D}\" stroke=\"transparent\" stroke-width=\".5\" />");
                                stringBuilder.AppendLine($"<circle id=\"{shot.EventName.Replace(" ", "_").Trim()}_shotOutline\" cx=\"{x}\" cy=\"{y}\" r=\"{scale * 2.25D}\" class=\"shot-outline\" stroke=\"black\" stroke-width=\".5\" fill=\"transparent\" />");
                            }
                        }


                        //Draw the scoring rings
                        foreach (var scoringRing in Target.ScoringRings)
                        {
                            var stroke = "black";
                            if (scoringRing.Dimension < Target.AimingMarks[0].Dimension)
                                stroke = "white";
                            //TODO adjust stroke width based on dimension of target, bigger targets get bigger stroke widths. Need min of 2.0 so they may be seen.
                            stringBuilder.AppendLine($"<circle class=\"aiming-circle\"  cx=\"{center}\" cy=\"{center}\" r=\"{scale * scoringRing.Dimension / 2D}\" stroke=\"{stroke}\" stroke-width=\"2.0\" fill=\"transparent\" />");
                        }

                        EventScore eventScore;
                        ResultCOF.EventScores.TryGetValue(EventWeAreLookingFor.EventName, out eventScore);
                        if (eventScore != null)
                        {
                            ScoreFormatted = eventScore.ScoreFormatted;
                        }
                    }
                    else
                    {
                        EventScore eventScore;
                        if (ResultCOF.EventScores.TryGetValue(EventWeAreLookingFor.EventName, out eventScore))
                        {
                            if (eventScore.NumShotsFired == 0)
                                stringBuilder.AppendLine($"<text x=\"10\" y=\"{center}\" class=\"heavy\">No shots fired</text>");
                            else
                                //If we get here, could be a BB Gun test, or a hit/miss target.
                                stringBuilder.AppendLine($"<text x=\"10\" y=\"{center}\" class=\"heavy\">{eventScore.ScoreFormatted}</text>");

                            ScoreFormatted = eventScore.ScoreFormatted;
                        }
                        else
                        {
                            //If we get here, likely drawing a shot withunknown coordinates. Not quite sure how to handle this yet.
                            ;
                        }
                    }

                    stringBuilder.AppendLine("</svg>");

                    SvgMarkup = stringBuilder.ToString();

                    var fileName = $"{ResultCOF.MatchID}_{ResultCOF.ResultCOFID}_{EventName}";
                    dynamic seriesName;
                    if (!string.IsNullOrEmpty(ScoreFormatted))
                    {
                        seriesName = $@"{ResultCOF.Participant.DisplayNameShort};;{StringFormatting.ConvertOrdinalsToLowerCase(ResultCOF.MatchName)};;Series: {EventName};;Aggregate: {ScoreFormatted}";
                    }
                    else
                    {
                        seriesName = $@"{ResultCOF.Participant.DisplayNameShort};;{StringFormatting.ConvertOrdinalsToLowerCase(ResultCOF.MatchName)};;Series: {EventName}";
                    }

                    var match = Match;

                    // commenting out, will break direct share but fix coming
                    // this is causing a rendering error that occurs only in the mobile view
                    // if (match != null)
                    // {
                    //     var matchDates = StringFormatting.SpanOfDates(match.StartDate, match.EndDate);
                    //     var location = StringFormatting.Location(Match.Location.City, Match.Location.State, Match.Location.Country);
                    //     var ogDescription = new StringBuilder();
                    //     ogDescription.Append($"<div style='width: 100%;'>");
                    //     ogDescription.Append($"<span style='font-size: 54px; line-height: 60px;'>{ResultCOF.Participant.DisplayName}</span><br />");
                    //     ogDescription.Append($"<br />");
                    //     ogDescription.Append($"<span style='font-size: 36px; line-height: 42px;'>{match.Name}</span><br />");
                    //     ogDescription.Append($"<br />");
                    //     ogDescription.Append($"<span style='font-size: 28px; line-height: 36px;'>{matchDates}</span><br />");
                    //     if (!string.IsNullOrEmpty(location))
                    //     {
                    //         ogDescription.Append($"<span style='font-size: 28px; line-height: 36px;'>{location}</span><br />");
                    //     }
                    //     ogDescription.Append($"<br />");
                    //     ogDescription.Append($"<span style='font-size: 54px; line-height: 60px;'>{EventName}: {ScoreFormatted}</span><br />");
                    //     ogDescription.Append($"</div>");
                    //     float lastShotOutlineY;
                    //     var transformedSvgContent = TransformSvgContent(SvgMarkup, out lastShotOutlineY);

                    //     var outputPath = Path.Combine(WebHostEnvironment.WebRootPath, "Renders", "Stage", $"{fileName}.png");
                    //     var secondColumnPlaceHolder = $@"
                    //                                 <div style='transform: scale(0.6); transform-origin: top center; text-align: center; padding-top: 10px;'>
                    //                             {transformedSvgContent}
                    //                                 </div>";

                    //     var bootstrap = BuildTableWithTwoColumns(ogDescription.ToString(), secondColumnPlaceHolder);

                    //     var imageResponse = await MatchImageService.RenderHtmlAsImage(bootstrap,
                    //         "https://cdn.scopos.tech/headers/rezults-header-blank.png");
                    //     await MatchImageService.SaveImageToDisk(imageResponse, outputPath);

                    // }
                }
                catch
                    (Exception e)
                {
                    logger.Error(e);
                    throw;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ErrorMessage = ex.Message;
            }
        }

        private string BuildTableWithTwoColumns(string firstColumn, string secondColumn)
        {
            var stringBuilder = new StringBuilder();

            // Start of the table
            stringBuilder.AppendLine("<table style='width:100%; border-collapse: collapse; margin-top: 25px;'>");

            // Table row
            stringBuilder.AppendLine("<tr>");

            // First column
            stringBuilder.AppendLine(
                "  <td style='width: 60%; vertical-align: top; padding: 8px; color: white; font-size: 24px; line-height: 30px; font-family: aktiv-grotesk-condensed, sans-serif; font-style: normal; font-weight: 100;'>");
            stringBuilder.AppendLine($"    {firstColumn}");
            stringBuilder.AppendLine("  </td>");

            // Second column
            stringBuilder.AppendLine(
                "  <td style='width: 40%; vertical-align: top; padding: 8px; color: white; font-size: 24px; line-height: 30px; font-family: aktiv-grotesk-condensed, sans-serif; font-style: normal; font-weight: 100;'>");
            stringBuilder.AppendLine($"    {secondColumn}");
            stringBuilder.AppendLine("  </td>");

            // End of the table row
            stringBuilder.AppendLine("</tr>");

            // End of the table
            stringBuilder.AppendLine("</table>");

            return stringBuilder.ToString();
        }

        // Method to transform SVG content
        private string TransformSvgContent(string svgContent, out float lastShotOutlineY)
        {
            lastShotOutlineY = 0f;

            // Load SVG content into an XDocument
            var svgDocument = XDocument.Parse(svgContent);
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("svg", SvgNamespace);

            // Select all elements with a fill attribute
            var filledElements = svgDocument.XPathSelectElements("//*[(@fill)]", namespaceManager);

            // Loop over selected elements and change fill to black
            foreach (var element in filledElements)
            {
                element.SetAttributeValue("fill", "transparent");
            }

            // Select all elements with a black stroke
            var blackStrokedElements = svgDocument.XPathSelectElements("//*[(@stroke='black')]", namespaceManager);

            // Loop over selected elements and change black stroke to white
            foreach (var element in blackStrokedElements)
            {
                element.SetAttributeValue("stroke", "white");
                element.SetAttributeValue("stroke-width", "0.5");
            }

            // Select all elements with class 'shot-outline'
            var shotOutlineElements = svgDocument.XPathSelectElements("//*[@class='shot-outline']", namespaceManager);

            // Loop over selected elements and change fill color to #4f84be and fill opacity to 0.3 (30%)
            foreach (var element in shotOutlineElements)
            {
                element.SetAttributeValue("fill", "#4f84be");
                element.SetAttributeValue("fill-opacity", "0.3");

                // If the element is a circle, update the lastShotOutlineY with its y-coordinate
                if (element.Name.LocalName == "circle" && element.Attribute("cy") != null)
                {
                    lastShotOutlineY = Math.Max(lastShotOutlineY, float.Parse(element.Attribute("cy").Value));
                }
            }

            // Return transformed SVG content
            return svgDocument.ToString();
        }

        public string SvgWithoutCSS(string svgContent, int size)
        {
            // Load SVG content into an XDocument
            var svgDocument = XDocument.Parse(svgContent);
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("svg", SvgNamespace);
            var svgElement = svgDocument.Root;

            svgElement.SetAttributeValue("style", "overflow:hidden");
            svgElement.SetAttributeValue("width", $"{size}");
            svgElement.SetAttributeValue("height", $"{size}");

            // Select all elements with a fill attribute
            var aimingCircleElements = svgDocument.XPathSelectElements("//*[(@class='aiming-circle')]", namespaceManager);

            // Loop over selected elements and change fill to black
            foreach (var element in aimingCircleElements)
            {
                element.SetAttributeValue("stroke", "white");
                element.SetAttributeValue("fill", "white");
                element.SetAttributeValue("fill-opacity", "0.0");
            }

            // Select all elements with a black stroke
            var aimingBlackElements = svgDocument.XPathSelectElements("//*[(@class='aiming-black')]", namespaceManager);

            // Loop over selected elements and change black stroke to white
            foreach (var element in aimingBlackElements)
            {
                element.SetAttributeValue("stroke", "black");
                element.SetAttributeValue("fill", "black");
            }

            // Select all elements with a black stroke
            var shotElements = svgDocument.XPathSelectElements("//*[(@class='shot')]", namespaceManager);

            // Loop over selected elements and change black stroke to white
            foreach (var element in shotElements)
            {
                element.SetAttributeValue("stroke", "#369CCD");
                element.SetAttributeValue("fill", "#369CCD");
            }

            // Select all elements with class 'shot-outline'
            var shotOutlineElements = svgDocument.XPathSelectElements("//*[@class='shot-outline']", namespaceManager);

            // Loop over selected elements and change fill color to #4f84be and fill opacity to 0.3 (30%)
            foreach (var element in shotOutlineElements)
            {
                element.SetAttributeValue("stroke", "#4f84be");
                element.SetAttributeValue("fill", "#4f84be");
                element.SetAttributeValue("fill-opacity", "0.3");
            }

            return svgDocument.ToString();
        }


        private List<Scopos.BabelFish.DataModel.Athena.Shot.Shot> GetShotsToDisplay()
        {

            List<Scopos.BabelFish.DataModel.Athena.Shot.Shot> shots = new List<Scopos.BabelFish.DataModel.Athena.Shot.Shot>();

            if (EventWeAreLookingFor != null)
            {

                Scopos.BabelFish.DataModel.Athena.Shot.Shot shot;
                foreach (var ec in DescendantEventComposites)
                {

                    if (ShotsByEventName.TryGetValue(ec.EventName, out shot))
                    {
                        shots.Add(shot);
                    }
                }
            }

            return shots;

        }

        private Scopos.BabelFish.DataModel.Athena.Shot.Shot GetWidestShot()
        {

            Scopos.BabelFish.DataModel.Athena.Shot.Shot widestShot = null;
            Scopos.BabelFish.DataModel.Athena.Shot.Shot shot = null;
            double maxDistance = 0;
            foreach (var ec in DescendantEventComposites)
            {

                if (ShotsByEventName.TryGetValue(ec.EventName, out shot)
                && shot.Location != null
                && shot.Location.GetRadiusSquared() > maxDistance)
                {
                    maxDistance = shot.Location.GetRadiusSquared();
                    widestShot = shot;
                }
            }

            return widestShot;
        }

        private void PopulateParentScores(EventComposite ec)
        {
            //Use recursion to populate a list of ancestor's event name and scores.
            EventScore eventScore;
            if (ResultCOF.EventScores.TryGetValue(ec.EventName, out eventScore))
            {
                ParentScores.Insert(0, $"{ec.EventName}: {eventScore.ScoreFormatted}");
            }
            if (ec.HasParent)
            {
                PopulateParentScores(ec.Parent);
            }
        }

        private void PopulateChildScores()
        {

            foreach (var child in EventWeAreLookingFor.Children)
            {
                EventScore eventScore;
                if (ResultCOF.EventScores.TryGetValue(child.EventName, out eventScore))
                {
                    ChildScores.Add($"{child.EventName}: {eventScore.ScoreFormatted}");
                }
            }
        }
    }
}
