using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {

    public class GetLeagueGamesPublicRequest : Request, ITokenRequest {

        public GetLeagueGamesPublicRequest( string leagueId ) : base( "GetLeagueGames" ) {
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

				if (StartDate > DateTime.MinValue )
					parameterList.Add( "start-date", new List<string>() { StartDate.ToString( DateTimeFormats.DATE_FORMAT ) } );

				if (EndDate < DateTime.MaxValue )
					parameterList.Add( "end-date", new List<string>() { EndDate.ToString( DateTimeFormats.DATE_FORMAT ) } );

				if (TeamId > 0)
					parameterList.Add( "team-id", new List<string> { TeamId.ToString() } );

				if (!string.IsNullOrEmpty( GameId ))
                    parameterList.Add( "game-id", new List<string>() { GameId } );

                if ( Limit > 0 )
					parameterList.Add( "limit", new List<string>() { Limit.ToString() } );

				if (!string.IsNullOrEmpty( Token ))
					parameterList.Add( "token", new List<string>() { Token } );

				var virtualList = new List<string>();
				if (GamesNotSet)
					virtualList.Add( "Virtual" );

				if (LocalLeagueGames)
					virtualList.Add( "Not Set" );

				if (VirtualLeagueGames)
					virtualList.Add( "Local" );

				if (ByeWeeks)
					virtualList.Add( "Bye Week" );

				if (ForcedByeWeeks)
					virtualList.Add( "Forced Bye Week" );

				if (CancelledGames)
					virtualList.Add( "Cancelled" );

				if (virtualList.Count > 0)
					parameterList.Add( "virtual", virtualList );

				return parameterList;
			}
		}
		
		/// <inheritdoc />
		public override string RelativePath {
            get { return $"/league/{LeagueId}/games"; }
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
		public DateTime StartDate { get; set; } = DateTime.MinValue;

		/// <summary>
		/// Adds a filter to returned list of league games. Only games that were competed partially or in whole
		/// before EndDate will be returned.
		/// The default value is to search all dates.
		/// </summary>
		public DateTime EndDate { get; set; } = DateTime.MaxValue;

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

		/// <summary>
		/// Adds a filter to the returned list of league games. Only games that involved the team with the
		/// specified TeamId are returned. Check GetLeagueTeams for a list of valid teams and team ids.
		/// </summary>
		public int TeamId { get; set; } = 0;

		/// <summary>
		/// Returns only the League Game specified by GameId.
		/// </summary>
		public string GameId { get; set; } = string.Empty;

		/// <summary>
		/// Adds a filter to the returned list. Will include games that have not yet been set to Virtual, Local, or Cancelled.
		/// If .GamesNotSet, .VirtualLeagueGames, .LocalLeagueGames, .ByeWeeks, .ForcedByeWeeks, and .CancelledGames are all
		/// false, then the default set of GamesNotSet, VirtualLeagueGames, and LocalLeagueGames will be returned.
		/// </summary>
		public bool GamesNotSet { get; set; } = false;

		/// <summary>
		/// Adds a filter to the returned list. Will include games that have been set to Virtual.
		/// If .GamesNotSet, .VirtualLeagueGames, .LocalLeagueGames, .ByeWeeks, .ForcedByeWeeks, and .CancelledGames are all
		/// false, then the default set of GamesNotSet, VirtualLeagueGames, and LocalLeagueGames will be returned.
		/// </summary>
		public bool VirtualLeagueGames { get; set; } = false;

		/// <summary>
		/// Adds a filter to the returned list. Will include games that have been set Local.
		/// If .GamesNotSet, .VirtualLeagueGames, .LocalLeagueGames, .ByeWeeks, .ForcedByeWeeks, and .CancelledGames are all
		/// false, then the default set of GamesNotSet, VirtualLeagueGames, and LocalLeagueGames will be returned.
		/// </summary>
		public bool LocalLeagueGames { get; set; } = false;

		/// <summary>
		/// Adds a filter to the returned list. Will include games that are ByeWeeks.
		/// If .GamesNotSet, .VirtualLeagueGames, .LocalLeagueGames, .ByeWeeks, .ForcedByeWeeks, and .CancelledGames are all
		/// false, then the default set of GamesNotSet, VirtualLeagueGames, and LocalLeagueGames will be returned.
		/// </summary>
		public bool ByeWeeks { get; set; } = false;

		/// <summary>
		/// Adds a filter to the returned list. Will include games that are Forced Bye Weeks.
		/// If .GamesNotSet, .VirtualLeagueGames, .LocalLeagueGames, .ByeWeeks, .ForcedByeWeeks, and .CancelledGames are all
		/// false, then the default set of GamesNotSet, VirtualLeagueGames, and LocalLeagueGames will be returned.
		/// </summary>
		public bool ForcedByeWeeks { get; set; } = false;

		/// <summary>
		/// Adds a filter to the returned list. Will include games that are Cancelled.
		/// If .GamesNotSet, .VirtualLeagueGames, .LocalLeagueGames, .ByeWeeks, .ForcedByeWeeks, and .CancelledGames are all
		/// false, then the default set of GamesNotSet, VirtualLeagueGames, and LocalLeagueGames will be returned.
		/// </summary>
		public bool CancelledGames { get; set; } = false;

		/// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetLeagueGamesPublicRequest( LeagueId );
            newRequest.Token = this.Token;
            newRequest.Limit = this.Limit;
			newRequest.StartDate = this.StartDate;
			newRequest.EndDate = this.EndDate;

			newRequest.Division = this.Division;
			newRequest.Conference = this.Conference;
			newRequest.TeamId = this.TeamId;
			newRequest.GamesNotSet = this.GamesNotSet;
			newRequest.VirtualLeagueGames = this.VirtualLeagueGames;
			newRequest.LocalLeagueGames = this.LocalLeagueGames;
			newRequest.ByeWeeks = this.ByeWeeks;
			newRequest.ForcedByeWeeks = this.ForcedByeWeeks;
			newRequest.CancelledGames = this.CancelledGames;

            return newRequest;
        }
    }
}
