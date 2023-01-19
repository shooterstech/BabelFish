using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI
{
    public class GetMatchSearchPublicRequest : Request
    {
        /// <summary>
        /// Public constructor. 
        /// User is encouraged (really you need to do this) to set the Request Properties at time of construction.
        /// </summary>
        public GetMatchSearchPublicRequest() : base( "MatchSearch")  { }

        /// <summary>
        /// Distance in miles to search.
        /// The default is 10 miles.
        /// </summary>
        public int DistanceSearch { get; set; } = 10;

        /// <summary>
        /// The start date of the match dates to search.
        /// The default value is the first day of the current month.
        /// </summary>
        public DateTime StartDate { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        /// <summary>
        /// The end date of the match dates to search.
        /// The default value is the last day of the current month.
        /// </summary>
        public DateTime EndDate { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);

        /// <summary>
        /// The shooting style to search or unassigned for all.
        /// The default value is Air Rifle.
        /// </summary>
        public List<string> ShootingStyle { get; set; } = new List<string>() { "Air Rifle" };

        /// <summary>
        /// The number of matches to return.
        /// The default is 100.
        /// </summary>
        public int NumberOfMatchesToReturn { get; set; } = 100;

        /// <summary>
        /// The Logitude of an area to search.
        /// If > default of 0, Latitude must also be > 0.
        /// </summary>
        public double Longitude { get; set; } = 0;

        /// <summary>
        /// The Latitude of an area to search.
        /// If > default of 0, Longitude must also be > 0.
        /// </summary>
        public double Latitude { get; set; } = 0;

        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {
                if (Latitude == 0 || Longitude == 0)
                    throw new GetOrionMatchRequestException("Longitude and Latitude are required and must be > 0.");
                if (ShootingStyle == null || ShootingStyle.Count == 0)
                    throw new GetOrionMatchRequestException("Shooting Style is required for search.");

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add("distanceSearch", new List<string>() { DistanceSearch.ToString() });
                parameterList.Add("startingDate", new List<string>() { StartDate.ToString(Scopos.BabelFish.DataModel.Athena.DateTimeFormats.DATE_FORMAT) });
                parameterList.Add("endingDate", new List<string>() { EndDate.ToString(Scopos.BabelFish.DataModel.Athena.DateTimeFormats.DATE_FORMAT) });
                parameterList.Add("numberOfMatchesToReturn", new List<string>() { NumberOfMatchesToReturn.ToString() });
                parameterList.Add("shootingStyle", ShootingStyle);
                parameterList.Add("Longitude", new List<string>() { Longitude.ToString() });
                parameterList.Add("Latitude", new List<string>() { Latitude.ToString() });

                return parameterList;
            }
        }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/match/search"; }
        }
    }
}
