using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {

    public class GetLeagueTeamsPublicRequest : Request, ITokenRequest {

        public GetLeagueTeamsPublicRequest( string leagueId ) : base( "GetLeagueTeamList" ) {
			if (string.IsNullOrEmpty( leagueId ) ) {
				throw new ArgumentNullException( "Parameter leagueId may not be null or an empty string." );
			}

			LeagueId = leagueId;
        }

        public string LeagueId { get; private set; } = string.Empty;


		/// <inheritdoc />
		public override Dictionary<string, List<string>> QueryParameters {
			get {

				Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
				if ( ! string.IsNullOrEmpty( Division ) )
					parameterList.Add( "division", new List<string>() { Division } );

				if (!string.IsNullOrEmpty( Conference ))
					parameterList.Add( "conference", new List<string>() { Conference } );

				if (Date > DateTime.MinValue )
					parameterList.Add( "date", new List<string>() { Date.ToString( Scopos.BabelFish.DataModel.Athena.DateTimeFormats.DATE_FORMAT ) } );

				if ( Limit > 0 )
					parameterList.Add( "limit", new List<string>() { Limit.ToString() } );

				if (!string.IsNullOrEmpty( Token ))
					parameterList.Add( "token", new List<string>() { Token } );

				return parameterList;
			}
		}
		
		/// <inheritdoc />
		public override string RelativePath {
            get { return $"/league/{LeagueId}/teams"; }
		}

		/// <inheritdoc />
		public string Token { get; set; } = string.Empty;

		/// <inheritdoc />
		public int Limit { get; set; }

		/// <summary>
		/// Adds a filter to returned list of league games. Only games that were competed partially or in whole
		/// after StartDate will be returned.
		/// The default value is to search all dates.
		/// </summary>
		public DateTime Date { get; set; } = DateTime.MinValue;

		/// <summary>
		/// Adds a filter to the returned list of league games. Only games that involved one or both teams in the 
		/// specified Division are returned. Check GetLeagueDetail for a list of valid Divisions within this league.
		/// </summary>
		public string Division { get; set; } = string.Empty;

		/// <summary>
		/// Adds a filter to the returned list of league games. Only games that involved one or both teams in the 
		/// specified Conference are returned. Gheck GetLeagueDetail for a list of valid Conferences within this league.
		/// </summary>
		public string Conference { get; set; } = string.Empty;


		/// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetLeagueTeamsPublicRequest( LeagueId );
            newRequest.Token = this.Token;
            newRequest.Limit = this.Limit;
			
			newRequest.Division = this.Division;
			newRequest.Conference = this.Conference;
			
            return newRequest;
        }
    }
}
