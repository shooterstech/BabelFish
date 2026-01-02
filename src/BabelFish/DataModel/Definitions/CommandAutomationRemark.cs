using Scopos.BabelFish.DataModel.OrionMatch;
using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions
{
    /// <summary>
    /// A CommandAutomationRemark is a (SegmentGroupCommand) automation directive. It says to add or hide a Remark (e.g. DSQ) to one or more Participants 
    /// in a Result List, based on their ranks. 
    /// </summary>
    public class CommandAutomationRemark : CommandAutomation { 


        public CommandAutomationRemark()
        {
            this.Subject = CommandAutomationSubject.REMARK;
            this.Action = RemarkVisibility.HIDE;
            this.ParticipantRanks = new ValueSeries( ValueSeries.APPLY_TO_ALL_FORMAT );
            this.Condition = ParticipantRemark.BUBBLE;
        }

        /// <summary>
        /// Action describes what should be done to the remark specified, on the participants specified.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>SHOW: Adds the remark to the Participant and makes the remark visible.</item>
        /// <item>HIDE: Hides the remark, if it exists on the Participant, making it not visible.</item>
        /// <item>DELETE: Deletes the remark from the Participant.</item>
        /// </list>
        /// </remarks>
        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public RemarkVisibility Action { get; set; } = RemarkVisibility.HIDE;

        /// <summary>
        /// The Participant Remark to show, hide, or delete on the participants on the ranks given.
        /// </summary>
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public ParticipantRemark Condition { get; set; } = ParticipantRemark.BUBBLE;

        /// <summary>
        /// ValueString object that is the items to apply an action of remark to.
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        [DefaultValue( "*" )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.ValueSeriesConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ValueSeriesConverter ) )]
        public ValueSeries ParticipantRanks { get; set; } = new ValueSeries( ValueSeries.APPLY_TO_ALL_FORMAT );

        /// <summary>
        /// Converts .ParticipantRanks into a list of integers. Each one representing the rank of an athlete
        /// to apply this command automation to. E.g 1 means to apply to first place athlete. 2, means to apply
        /// to second rank athlete.
        /// </summary>
        /// <returns></returns>
        [Obsolete( "Simply use .PartijcipantRanks.GetAsList() instead.")]
        public List<int> GetParticipantRanksAsList() {
            return ParticipantRanks.GetAsList();
        }

        /// <summary>
        /// returns a list of [participant, remark, action] objects allows the medea RL to be updated by itself.
        /// </summary>
        /// <param name="resultList"></param>
        /// <returns></returns>
        public List<ResultEvent> GetParticipantListToApply( ResultList resultList ) {
            List<ResultEvent> resultEvents = new List<ResultEvent>();
            CalculateBottomRank( resultList );

            var ranks = ParticipantRanks.GetAsList();
            foreach (var item in resultList.Items) {
                if (ranks.Contains( item.Rank ) || ranks.Contains( item.BottomRank ) ) {
                    resultEvents.Add( item );
                }
            }
            return resultEvents;
        }

        private void CalculateBottomRank( ResultList resultList ) {

            bool foundATime = false;
            int currentBottomRank = 0;
            for (int i = resultList.Items.Count - 1; i >= 0; i--) {
                var item = resultList.Items[i];

                if (foundATime) {
                    //If we are currently on a tie, when .Rank equal .RankOrder this is our stop condition
                    if ( item.Rank == item.RankOrder) {
                        foundATime = false;
					}

				} else {
                    //Detect if we found a new tie
                    foundATime = (item.Rank != item.RankOrder);
                    currentBottomRank = item.RankOrder;
				}

				item.BottomRank = currentBottomRank;
			}
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Subject} {Condition} {Action} on {ParticipantRanks}";
        }
    }
}
