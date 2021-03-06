using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.BabelFish.DataModel.Definitions;

namespace ShootersTech.BabelFish.Requests.ScoreHistoryAPI
{
    public class GetScoreHistoryAbstractRequest : Request
    {

        private SetName eventStyle = null;
        private List<SetName> stageStyles = new List<SetName>();

        /// <summary>
        /// Public constructor. 
        /// User is encouraged (really you need to do this) to set the Request Properties at time of construction.
        /// </summary>
        public GetScoreHistoryAbstractRequest() { }

        /// <summary>
        /// Gets or Sets the SetName of the Event Style for the Score History request.
        /// On Get, a returned value of null, means the EventStyle has not been set yet.
        /// On Set, StageStyles must be an empty list, as either EventStyle is required, or
        /// StageStyles is required, but both are not allowed.
        /// </summary>
        /// <exception cref="GetScoreHistoryRequestException">Thrown if called tried to set EventStyle with StageStles previously having one or more values.</exception>
        public SetName EventStyle {
            get { return eventStyle; }
            set {
                if ( stageStyles.Count == 0 ) {
                    eventStyle = value;
                } else {
                    throw new GetScoreHistoryRequestException( "Can not set both EventStyle and StageStyles. Prior to this call, StageStyles had one or more values." );
                }
            }
        }

        /// <summary>
        /// Gets or Sets the list of SetNames of the Stage Styles for the Score History request.
        /// On Get, an empty list returned means the StageStyles has not been set yet.
        /// On Set, EventStyle must be null, as either EventStyle is required, or
        /// StageStyles is required, but both are not allowed.
        /// </summary>
        /// <exception cref="GetScoreHistoryRequestException">Thrown if called tried to set StageStles previously having one or more values.</exception>
        public List<SetName> StageStyles {
            get { return stageStyles; }
            set {
                if (eventStyle == null) {
                    stageStyles = value;
                } else {
                    throw new GetScoreHistoryRequestException( "Can not set both EventStyle and StageStyles. Prior to this call, EventStyle had already been set." );
                }
            }
        }

        /// <summary>
        /// The maximum number of items to return;
        /// </summary>
        public uint Limit { get; set; } = 100;

        /// <summary>
        /// Submit ContinuationToken from Response when using Limit parameter
        /// </summary>
        public override string ContinuationToken { get => base.ContinuationToken; set => base.ContinuationToken = value; }

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
        public List<string> UserIds { get; set; }


        /// <summary>
        /// Gets or Sets the time span format to return data in. Default Value is Day.
        /// </summary>
        public ScoreHistoryFormatOptions Format { get; set; } = ScoreHistoryFormatOptions.DAY;

        /// <inheritdoc />
        /// <exception cref="GetScoreHistoryRequestException">Thrown if call WithAuthentication=false and no UserIds set.</exception>
        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {
                if (!WithAuthentication && (UserIds == null || UserIds.Count == 0))
                    throw new GetScoreHistoryRequestException("UserIds required for Non-Authenticated request.");

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (eventStyle != null)
                    parameterList.Add("event-style-def", new List<string>() { eventStyle.ToString() });

                if (stageStyles.Count > 0)
                    parameterList.Add("stage-style-def", stageStyles.Select(s => s.ToString()).ToList());

                if (UserIds != null && UserIds.Count > 0)
                    parameterList.Add("user-id", UserIds);

                if (ContinuationToken != null && ContinuationToken != string.Empty)
                    parameterList.Add("continuation-token", new List<string>() { ContinuationToken });

                parameterList.Add("limit", new List<string>() { Limit.ToString() });
                parameterList.Add("start-date", new List<string>() { StartDate.ToString(ShootersTech.BabelFish.DataModel.Athena.DateTimeFormats.DATE_FORMAT) });
                parameterList.Add("end-date", new List<string>() { EndDate.ToString(ShootersTech.BabelFish.DataModel.Athena.DateTimeFormats.DATE_FORMAT) });
                parameterList.Add("include-related", new List<string>() { IncludeRelated.ToString() });
                parameterList.Add("format", new List<string>() { Format.ToString() });

                return parameterList;
            }
        }
    }
}
