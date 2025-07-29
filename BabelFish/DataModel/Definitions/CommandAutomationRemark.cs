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
            this.ParticipantRanks = string.Empty;
            this.Condition = ParticipantRemark.BUBBLE;
        }

        /// <summary>
        /// Action describes what should be done to the remark specified, on the participants specified.
        /// </summary>
        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public RemarkVisibility Action { get; set; } = RemarkVisibility.HIDE;

        /// <summary>
        /// The Participant Remark to add to the participants on the ranks given.
        /// </summary>
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public ParticipantRemark Condition { get; set; } = ParticipantRemark.BUBBLE;

        /// <summary>
        /// ValueString object that is the items to apply an action of remark to.
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        [DefaultValue( "" )]
        public string ParticipantRanks { get; set; } = string.Empty;

        /// <summary>
        /// Converts .ParticipantRanks into a list of integers. Each one representing the rank of an athlete
        /// to apply this command automation to. E.g 1 means to apply to first place athlete. 2, means to apply
        /// to second rank athlete.
        /// </summary>
        /// <returns></returns>
        public List<int> GetParticipantRanksAsList() {
            List<int> list = new List<int>();

            ValueSeries vs = new ValueSeries( ParticipantRanks );
            
            for (int i = vs.StartValue; i <= vs.EndValue; i += vs.Step) {
                list.Add( i );
            }

            return list;
        }

        /// <summary>
        /// returns a list of [participant, remark, action] objects allows the medea RL to be updated by itself.
        /// </summary>
        /// <param name="resultList"></param>
        /// <returns></returns>
        public List<ResultEvent> GetParticipantListToApply( ResultList resultList ) {
            List<ResultEvent> resultEvents = new List<ResultEvent>();
            CalculateBottomRank( resultList );

            var ranks = GetParticipantRanksAsList();
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
