using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ScoreHistoryAPI {

    /// <summary>
    /// The GetScoreHistoryAbstractRequest class may be used as the base request class for all GetScoreHistory and
    /// GetScoreAvearge requests. We just choose to name it Get*ScoreHistory* because ... well ... we flipped
    /// a coin and that is what won.
    /// </summary>
    public class GetScoreHistoryAbstractRequest : Request, ITokenRequest {

        private SetName eventStyle = null;
        private List<SetName> stageStyles = new List<SetName>();
        private int limit = 50;

        public enum ScoreHistoryRequestType { SCORE_HISTORY, SCORE_AVERAGE };

        /// <summary>
        /// Public constructor. 
        /// User is encouraged (really you need to do this) to set the Request Properties at time of construction.
        /// </summary>
        public GetScoreHistoryAbstractRequest( string operationId ) : base( operationId ) { }

        /// <summary>
        /// Public constructor. 
        /// User is encouraged (really you need to do this) to set the Request Properties at time of construction.
        /// </summary>
        public GetScoreHistoryAbstractRequest( string operationId, UserAuthentication credentials ) : base( operationId, credentials ) { }


        /// <summary>
        /// Factory method to return a concrete Get Score History Request or Get Score Average Request object based on value of User Authentication.
        /// If it is null, then a public request is returned. If it is not null, then an Authenticated request is returned. 
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public static GetScoreHistoryAbstractRequest Factory( ScoreHistoryRequestType requestType, UserAuthentication credentials ) {
            if (requestType == ScoreHistoryRequestType.SCORE_HISTORY) {
                if (credentials == null)
                    return new GetScoreHistoryPublicRequest();
                else
                    return new GetScoreHistoryAuthenticatedRequest( credentials );
            } else {
				if (credentials == null)
					return new GetScoreAveragePublicRequest();
				else
					return new GetScoreAverageAuthenticatedRequest( credentials );

			}
		}

        /// <summary>
        /// Gets or Sets the SetName of the Event Style for the Score History request.
        /// On Get, a returned value of null, means the EventStyle has not been set yet.
        /// On Set, StageStyles must be an empty list.
        /// Setting both StageStyles and  EventStyle is vorbotten.
        /// Setting neither will return all EventStyles for the athletes in the UserIds list.
        /// </summary>
        /// <exception cref="GetScoreHistoryRequestException">Thrown if called tried to set EventStyle with StageStles previously having one or more values.</exception>
        public SetName EventStyleDef {
            get { return eventStyle; }
            set {
                if (stageStyles.Count == 0) {
                    eventStyle = value;
                } else {
                    throw new GetScoreHistoryRequestException( "Can not set both EventStyle and StageStyles. Prior to this call, StageStyles had one or more values." );
                }
            }
        }

        /// <summary>
        /// Gets or Sets the list of SetNames of the Stage Styles for the Score History request.
        /// On Get, an empty list returned means the StageStyles has not been set yet.
        /// On Set, EventStyle must be null.
        /// Setting both StageStyles and  EventStyle is vorbotten.
        /// Setting neither will return all EventStyles for the athletes in the UserIds list.
        /// </summary>
        /// <exception cref="GetScoreHistoryRequestException">Thrown if called tried to set StageStles previously having one or more values.</exception>
        public List<SetName> StageStyleDefs {
            get { return stageStyles; }
            set {
                if (eventStyle == null || value.Count == 0) {
                    stageStyles = value;
                } else {
                    throw new GetScoreHistoryRequestException( "Can not set both EventStyle and StageStyles. Prior to this call, EventStyle had already been set." );
                }
            }
        }

        /// <inheritdoc />
        public string Token { get; set; }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the set value is outside the expected range of 1 to 100.</exception>
        public int Limit {
            get { return limit; } 
            set { 
                if ( value > 0 && value <= 100 )
                    limit = value;
                else
                    throw new ArgumentOutOfRangeException( $"Limit may only be between the values of 1 and 100 (inclusive). Instead received '{value}'.");
            }
        }

        /// <summary>
        /// The start date of the range of dates to return.
        /// The default value is 7 days prior to Today.
        /// </summary>
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays( -7 );


        /// <summary>
        /// The end date of the range of dates to return.
        /// The default value is Today.
        /// </summary>
        public DateTime EndDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Gets or Sets a boolean indicating if related scores should be included in the returned calculation.
        /// Default value is true.
        /// </summary>
        public bool IncludeRelated { get; set; } = true;

        /// <summary>
        /// Gets or Sets the list of User IDs to retreive their Score History.
        /// On a call .WithAuthentication the UserId is the authenticated caller is automatically included, this value does not have to be included in the list of UserIds.
        /// On a call without .WithAuthentication, at least one UserID on UserIds is required.
        /// User Ids are GUID formatted.
        /// </summary>
        public List<string> UserIds { get; set; } = new List<string>();


        /// <summary>
        /// Gets or Sets the time span format to return data in. Default Value is Day.
        /// </summary>
        public ScoreHistoryFormatOptions Format { get; set; } = ScoreHistoryFormatOptions.DAY;

        /// <inheritdoc />
        /// <exception cref="GetScoreHistoryRequestException">Thrown if call WithAuthentication=false and no UserIds set.</exception>
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (!RequiresCredentials && (UserIds == null || UserIds.Count == 0))
                    throw new GetScoreHistoryRequestException( "UserIds required for Non-Authenticated request." );

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (eventStyle != null)
                    parameterList.Add( "event-style-def", new List<string>() { eventStyle.ToString() } );

                if (stageStyles.Count > 0)
                    parameterList.Add( "stage-style-def", stageStyles.Select( s => s.ToString() ).ToList() );

                if (UserIds != null && UserIds.Count > 0)
                    parameterList.Add( "user-id", UserIds );

                if (! string.IsNullOrEmpty(Token))
                    parameterList.Add( "token", new List<string>() { Token } );

                parameterList.Add( "limit", new List<string>() { Limit.ToString() } );
                parameterList.Add( "start-date", new List<string>() { StartDate.ToString( DateTimeFormats.DATE_FORMAT ) } );
                parameterList.Add( "end-date", new List<string>() { EndDate.ToString( DateTimeFormats.DATE_FORMAT ) } );
                parameterList.Add( "include-related", new List<string>() { IncludeRelated.ToString() } );
                parameterList.Add( "format", new List<string>() { Format.ToString() } );

                return parameterList;
            }
        }
    }
}
