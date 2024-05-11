using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataActors.EventScoresProjection;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ResultEvent : IParticipant, IEventScoreProjection {

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

		[Obsolete("Use .Participant.DisplayName instead.")]
		public string DisplayName { get; set; }

        [Obsolete("Field is being replaced with the ScoreFormatCollectionDef and ScoreConfigName values. ScoreFormatCollectionDef is found using the CoruseOfFireDef")]
        public string ScoreFormat { get; set; }
        
        public string ResultCOFID { get; set; }

        public int Rank { get; set; }

		/// <summary>
		/// The Local Date that this score was shot. 
		/// NOTE Local Date is not necessarily the same as the GMT date.
		/// </summary>
		[JsonConverter( typeof( DateConverter ) )]
		public DateTime LocalDate { get; set; } = DateTime.Today;

		/// <summary>
		/// If this is a team score, the TeamMembers will be the scores of the team members.If this is an Individual value will be null.
		/// </summary>
		public List<ResultEvent> TeamMembers { get; set; } = new List<ResultEvent>();

		public List<IEventScoreProjection> GetTeamMembersAsIEventScoreProjection() {
			if (TeamMembers == null) {
				return new List<IEventScoreProjection>();
			}

			return TeamMembers.ToList<IEventScoreProjection>();

		}

        public Dictionary<string, Scopos.BabelFish.DataModel.OrionMatch.EventScore> EventScores { get; set; }

    }
}
