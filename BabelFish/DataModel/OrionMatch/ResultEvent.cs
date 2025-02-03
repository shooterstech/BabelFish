using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataActors.OrionMatch;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ResultEvent : IEventScoreProjection {

        //Key is the Singular Event Name, Value is the Shot
        private Dictionary<string, Athena.Shot.Shot> shotsByEventName = null;

        public ResultEvent() {
            //Purposefully set TeamMemebers to null so if it is an individual the attribute doesn't get added into the JSON
            TeamMembers = null;
		}

		/// <summary>
		/// Data on the person or team who shot this score.
		/// </summary>
		public Participant Participant { get; set; } = new Individual();

		/// <summary>
		/// The local Match ID that generated this ResultEvent.
		/// Information on that match may be looked up in the ResultList's Metadata field.
		/// </summary>
		public string MatchID { get; set; }
        
        public string ResultCOFID { get; set; }

		/// <summary>
		/// The absolute ranking of this competitor, using actual (and not projected) scores fired.
		/// </summary>
        public int Rank { get; set; }

		/// <summary>
		/// RankOrder is very nearly the same as Rank. The difference is if there is an unbreakable tie. In an
		/// unbreakable tie the two partjicipants are given the same Rank but different RankOrder.
		/// </summary>
		public int RankOrder {  get; set; }

		/// <summary>
		/// The projected rank of this competitor, using projected scores.
		/// </summary>
		[DefaultValue(0)]
		public int ProjectedRank { get; set; } = 0;


        /// <summary>
		/// ProjectedRankOrder is very nearly the same as ProjectedRank. The difference is if there is an unbreakable tie. In an
		/// unbreakable tie the two partjicipants are given the same ProjectedRank but different ProjectedRankOrder.
        /// </summary>
        [DefaultValue( 0 )]
        public int ProjectedRankOrder { get; set; } = 0;


        /// <summary>
        /// The Local Date that this score was shot. 
        /// NOTE Local Date is not necessarily the same as the GMT date.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime LocalDate { get; set; } = DateTime.Today;


        /// <inheritdoc />
		public List<IEventScoreProjection> GetTeamMembersAsIEventScoreProjection() {
			if (TeamMembers == null) {
				return new List<IEventScoreProjection>();
			}

			return TeamMembers.ToList<IEventScoreProjection>();
        }

        /// <inheritdoc />
        public void SetTeamMembersFromIEventScoreProjection( List<IEventScoreProjection> teamMembers ) {
            
            if (TeamMembers == null) 
                TeamMembers = new List<ResultEvent>();

            TeamMembers.Clear();

            foreach( var tm in teamMembers) {
                TeamMembers.Add( (ResultEvent)tm );
            }
        }

        /// <inheritdoc />
        public void ProjectScores( ProjectorOfScores ps ) {
            ps.ProjectEventScores( this );
        }

        [JsonPropertyOrder ( 50)]
        public Dictionary<string, Scopos.BabelFish.DataModel.OrionMatch.EventScore> EventScores { get; set; }

        /// <summary>
        /// Scores for each Singular Event (usually a Shot).
        /// The Key is the sequence number, which is represented here as a string, but is really a float. The Value is the Shot object.
        /// To get a dictionary of Shots by their EventName, use GetShotsByEventName()
        /// In the Result Event object, which is part of a Resuslt List, the Shots dictionary is purposefully not included
        /// to conserve length of data. It is included in ResultEvents because of the IEventScoreProjection interface.
        /// </summary>
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
		[DefaultValue( null ) ]
        public Dictionary<string, Scopos.BabelFish.DataModel.Athena.Shot.Shot> Shots { get; set; } = null;

        /// <inheritdoc />
        public Athena.Shot.Shot LastShot { get; set; } = null;


        /// <summary>
        /// If this is a team score, the TeamMembers will be the scores of the team members.If this is an Individual value will be null.
        /// </summary>
        [JsonPropertyOrder ( 52)]
        public List<ResultEvent> TeamMembers { get; set; } = new List<ResultEvent>();

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize TeamMembers when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeTeamMembers() {
            return (TeamMembers != null && TeamMembers.Count > 0);
        }

        /// <inheritdoc />
        public Dictionary<string, Scopos.BabelFish.DataModel.Athena.Shot.Shot> GetShotsByEventName() {
            if (shotsByEventName != null)
                return shotsByEventName;

            shotsByEventName = new Dictionary<string, Athena.Shot.Shot>();

            foreach (var t in Shots.Values)
                if (!string.IsNullOrEmpty( t.EventName ))
                    shotsByEventName.Add( t.EventName, t );

            return shotsByEventName;
        }

        /// <inheritdoc />
        public Scopos.BabelFish.DataModel.Athena.Shot.Shot? GetLastCompetitionShot()
        {
            Dictionary<string, Scopos.BabelFish.DataModel.Athena.Shot.Shot> shots = Shots;
            Scopos.BabelFish.DataModel.Athena.Shot.Shot lastShot = null;
            foreach (var shot in shots)
            {
                if (shot.Value.TimeScored > lastShot.TimeScored)
                {
                    lastShot = shot.Value;
                }
            }
            return lastShot;
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"ResultEvent for {this.Participant.DisplayName}";
        }
    }
}
