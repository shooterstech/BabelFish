using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.Requests.OrionMatchAPI 
{
    public class GetMatchSearchRequest : Request
    {
        public GetMatchSearchRequest() { }

        public GetMatchSearchRequest(int distanceSerch, string startingDate, string endingDate, string shootingStyle,
            int numberOfMatchesToReturn, double longitude, double latitude)
        {
            DistanceSearch = distanceSerch;
            StartingDate = startingDate;
            EndingDate = endingDate;
            ShootingStyle = shootingStyle;
            NumberOfMatchesToReturn = numberOfMatchesToReturn;
            Longitude = longitude;
            Latitude = latitude;
        }

        public int DistanceSearch { get; set; } = 0;
        
        public string StartingDate { get; set; } = string.Empty;
        
        public string EndingDate { get; set; } = string.Empty;
        
        public string ShootingStyle { get; set; } = string.Empty;
        
        public int NumberOfMatchesToReturn { get; set; } = 100;
        
        public double Longitude { get; set; } = 0;
        
        public double Latitude { get; set; } = 0;

        public override Dictionary<string, List<string>> QueryParameters {
            get
            {
                return new Dictionary<string, List<string>>()
                {
                    { "distanceSearch", new List<string> { DistanceSearch.ToString() } },
                    { "startingDate", new List<string> { StartingDate } },
                    { "endingDate", new List<string> { EndingDate } },
                    { "shootingStyle", new List<string> { ShootingStyle } },
                    { "numberOfMatchesToReturn", new List<string> { NumberOfMatchesToReturn.ToString() } },
                    { "Longitude", new List<string> { Longitude.ToString() } },
                    { "Latitude", new List<string> { Latitude.ToString() } },
                };
            }
        }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/match/search"; }
        }
    }
}
