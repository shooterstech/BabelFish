using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.Athena.Shot;
using System.Xml.XPath;
using SkiaSharp;
using Svg.Skia;

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

        public bool includeGroupAnalysis { get; set; } = false;

        public GroupAnalysisMaths? groupMaths { get; set; } = null;

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
        private List<Shot> ShotListToShow { get; set; }
        private List<EventComposite> DescendantEventComposites;
        private Logger logger = LogManager.GetCurrentClassLogger();
        private string ErrorMessage = "";

        private List<string> ParentScores { get; set; } = new List<string>();

        private List<string> ChildScores { get; set; } = new List<string>();

        private string ScoreFormatted { get; set; } = string.Empty;

        public async void TargetSVGCreatorAsync(float dimension, string? eventName, Match? match, ResultCOF? resultCoF, bool includeGroupAnalysis)
        {
            this.includeGroupAnalysis = includeGroupAnalysis;
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
        public string? GetScoreFormatted()
        {
            return ScoreFormatted;
        }
        public List<Shot>? GetShotListToShow()
        {
            return ShotListToShow;
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
                        ShotListToShow = GetShotsToDisplay();
                        foreach (var shot in ShotListToShow)
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

                        //Draw the Shot Group Analysis Stuff here.
                        groupMaths = new GroupAnalysisMaths(ShotListToShow);
                        //if you want the stuff added on there, this is where to to it.
                        if (includeGroupAnalysis)
                        {
                            double WHATTHEFUCK = groupMaths.GetAngle();
                            int crossLength = 15;
                            double x  = center + scale * groupMaths.GetCenterX();
                            double xr = scale * groupMaths.GetMajorAxis();
                            double y  = center - scale * groupMaths.GetCenterY();
                            double yr = scale * groupMaths.GetMinorAxis();
                            double a  = WHATTHEFUCK * (180D / Math.PI);
                            stringBuilder.AppendLine($"<g fill=\"transparent\" transform=\"rotate({a} {x} {y})\">\n" +
                                $"\t<ellipse cx=\"{x}\" cy=\"{y}\" rx=\"{xr}\" ry=\"{yr}\" stroke=\"#{ScoposColors.ORANGE_LIGHTEN_2}\" stroke-width=\"1\" fill=\"#{ScoposColors.ORANGE_LIGHTEN_2}\" fill-opacity=\"0.5\"/>\n" +
                                $"\t<line x1=\"{x}\" y1=\"{y-yr- crossLength}\" x2=\"{x}\" y2=\"{y+yr+ crossLength}\" stroke=\"black\" stroke-width=\"2\" />" + // Vertical 
                                $"\t<line x1=\"{x-xr- crossLength}\" y1=\"{y}\" x2=\"{x+xr+ crossLength}\" y2=\"{y}\" stroke=\"black\" stroke-width=\"2\" />" + // Horizontal
                                $"</g>");
                            //stringBuilder.AppendLine($"<circle id=\"{shot.EventName.Replace(" ", "_").Trim()}_shot\" class=\"shot\" cx=\"{x}\" cy=\"{y}\" r=\"{scale * 2.25D}\" stroke=\"transparent\" stroke-width=\".5\" />");
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

        public string SvgWithoutCSS(string svgContent, int width, int height)
        {
            // Load SVG content into an XDocument
            var svgDocument = XDocument.Parse(svgContent);
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("svg", SvgNamespace);
            var svgElement = svgDocument.Root;

            svgElement.SetAttributeValue("style", "overflow:hidden");
            //svgElement.SetAttributeValue("width", $"{width}");
            //svgElement.SetAttributeValue("height", $"{height}");

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
                element.SetAttributeValue("fill-opacity", "1.0");
            }

            // Select all elements with class 'shot-outline'
            var shotOutlineElements = svgDocument.XPathSelectElements("//*[@class='shot-outline']", namespaceManager);

            // Loop over selected elements and change fill color to #4f84be and fill opacity to 0.3 (30%)
            foreach (var element in shotOutlineElements)
            {
                element.SetAttributeValue("stroke", "black");
                element.SetAttributeValue("stroke-width", "1.5");
                element.SetAttributeValue("fill", "#369CCD");
                element.SetAttributeValue("fill-opacity", "0.1");
            }

            return svgDocument.ToString();
        }
        public string MakePrinterFriendly(string svgContent)
        {
            // Load SVG content into an XDocument
            var svgDocument = XDocument.Parse(svgContent);
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("svg", SvgNamespace);
            var svgElement = svgDocument.Root;

            svgElement.SetAttributeValue("style", "overflow:hidden");
            //svgElement.SetAttributeValue("width", $"{width}");
            //svgElement.SetAttributeValue("height", $"{height}");

            // Select all elements with a fill attribute
            var aimingCircleElements = svgDocument.XPathSelectElements("//*[(@class='aiming-circle')]", namespaceManager);

            // Loop over selected elements and change fill to black
            foreach (var element in aimingCircleElements)
            {
                element.SetAttributeValue("stroke", "black");
                element.SetAttributeValue("fill", "white");
                element.SetAttributeValue("fill-opacity", "0.0");
            }

            // Select all elements with a black stroke
            var aimingBlackElements = svgDocument.XPathSelectElements("//*[(@class='aiming-black')]", namespaceManager);

            // Loop over selected elements and change black stroke to white
            foreach (var element in aimingBlackElements)
            {
                element.SetAttributeValue("stroke", "white");
                element.SetAttributeValue("fill", "white");
                element.SetAttributeValue("fill-opacity", "1.0");
            }

            // Select all elements with a black stroke
            var shotElements = svgDocument.XPathSelectElements("//*[(@class='shot')]", namespaceManager);

            // Loop over selected elements and change black stroke to white
            foreach (var element in shotElements)
            {
                element.SetAttributeValue("fill-opacity", "0.0");
            }

            // Select all elements with class 'shot-outline'
            var shotOutlineElements = svgDocument.XPathSelectElements("//*[@class='shot-outline']", namespaceManager);

            // Loop over selected elements and change fill color to #4f84be and fill opacity to 0.3 (30%)
            foreach (var element in shotOutlineElements)
            {
                element.SetAttributeValue("stroke", "#"+ScoposColors.DARK_GREY.ToString());
                element.SetAttributeValue("stroke-width", "1.5");
                element.SetAttributeValue("fill", "#"+ScoposColors.DARK_GREY.ToString());
                element.SetAttributeValue("fill-opacity", "0.2");
            }

            return svgDocument.ToString();
        }

        public byte[] ConvertSvgToPng(string svgMarkup, int targetWidth, int targetHeight)
        {
            // Load SVG into SKSvg
            var svg = new SKSvg();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(svgMarkup)))
            {
                svg.Load(stream);
            }

            // Create a bitmap with desired dimensions
            using var bitmap = new SKBitmap(targetWidth, targetHeight);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.Transparent);

            // Calculate scale to fit SVG into target dimensions
            var scaleX = targetWidth / svg.Picture.CullRect.Width;
            var scaleY = targetHeight / svg.Picture.CullRect.Height;
            var matrix = SKMatrix.CreateScale(scaleX, scaleY);

            // Draw the SVG picture onto the canvas
            canvas.DrawPicture(svg.Picture, ref matrix);

            // Encode to PNG
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
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
