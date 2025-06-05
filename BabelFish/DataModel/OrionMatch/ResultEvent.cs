using System;
using System.Collections.Generic;
using System.ComponentModel;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataActors.OrionMatch;

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
        [G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        public Participant Participant { get; set; } = new Individual();

        /// <summary>
        /// The local Match ID that generated this ResultEvent.
        /// Information on that match may be looked up in the ResultList's Metadata field.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public string MatchID { get; set; }

        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string ResultCOFID { get; set; }

        /// <summary>
        /// The absolute ranking of this competitor, using actual (and not projected) scores fired.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public int Rank { get; set; }

        /// <summary>
        /// RankOrder is very nearly the same as Rank. The difference is if there is an unbreakable tie. In an
        /// unbreakable tie the two partjicipants are given the same Rank but different RankOrder.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public int RankOrder {  get; set; }

		[G_STJ_SER.JsonPropertyOrder( 5 )]
		[G_NS.JsonProperty( Order = 5 )]
		[DefaultValue( 0 )]
		public int RankDelta { get; set; } = 0;

        /// <summary>
        /// For internal use only, to learn which ResultEvents to apply Command Automation to
        /// </summary>
        [G_NS.JsonIgnore]
        public int BottomRank { get; set; }

        /// <summary>
        /// The projected rank of this competitor, using projected scores.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 6 )]
        [G_NS.JsonProperty( Order = 6 )]
        [DefaultValue(0)]
		public int ProjectedRank { get; set; } = 0;


        /// <summary>
		/// ProjectedRankOrder is very nearly the same as ProjectedRank. The difference is if there is an unbreakable tie. In an
		/// unbreakable tie the two partjicipants are given the same ProjectedRank but different ProjectedRankOrder.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 7 )]
        [G_NS.JsonProperty( Order = 7 )]
        [DefaultValue( 0 )]
        public int ProjectedRankOrder { get; set; } = 0;

		[G_STJ_SER.JsonPropertyOrder( 8 )]
		[G_NS.JsonProperty( Order = 8 )]
		[DefaultValue( 0 )]
		public int ProjectedRankDelta {  get; set; } = 0;


        /// <summary>
        /// The Local Date that this score was shot. 
        /// NOTE Local Date is not necessarily the same as the GMT date.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_STJ_SER.JsonPropertyOrder( 9 )]
        [G_NS.JsonProperty( Order = 9 )]
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

        [G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
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
        public Dictionary<string, Athena.Shot.Shot> Shots { get; set; } = new Dictionary<string, Athena.Shot.Shot>();

        /// <inheritdoc />
        [G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]
        public Athena.Shot.Shot ? LastShot { get; set; } = null;


        /// <summary>
        /// If this is a team score, the TeamMembers will be the scores of the team members.If this is an Individual value will be null.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 21 )]
        [G_NS.JsonProperty( Order = 21 )]
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
        public Dictionary<string, Athena.Shot.Shot> GetShotsByEventName() {
            if (shotsByEventName != null)
                return shotsByEventName;

            shotsByEventName = new Dictionary<string, Athena.Shot.Shot>();

            foreach (var t in Shots.Values)
                if (!string.IsNullOrEmpty( t.EventName ))
                    shotsByEventName.Add( t.EventName, t );

            return shotsByEventName;
        }

        /// <inheritdoc />
        public Athena.Shot.Shot? GetLastCompetitionShot()
        {
            Athena.Shot.Shot lastShot = null;

            if (Shots != null) {
                foreach (var shot in Shots) {
                    if (shot.Value.TimeScored > lastShot.TimeScored) {
                        lastShot = shot.Value;
                    }
                }
            }
            return lastShot;
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"ResultEvent for {this.Participant.DisplayName}";
        }

		/// <inheritdoc />
		public ResultStatus GetStatus() {
            foreach( var es in this.EventScores.Values ) {
                if ( es.EventType == "EVENT" ) {
                    return es.Status;
                }
            }

            return ResultStatus.OFFICIAL;
        }
    }
}
