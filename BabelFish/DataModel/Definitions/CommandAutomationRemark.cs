using Scopos.BabelFish.DataModel.OrionMatch;
using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions
{
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
        /// Condition to add to the participants on the ranks given.
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
        /// Generates a list of Firing Lanes based on FiringLanes specified in property.
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
        /// functionally the same as adding remarks or a participant
        /// </summary>
        /// <param name="resultList"></param>
        public void ShowRemarkOnParticipants(ResultList resultList)
        {
            var ranks = GetParticipantRanksAsList();
            foreach (var item in resultList.Items)
            {
                if ( ranks.Contains(item.Rank) )
                {
                    item.Participant.RemarkList.Add(Condition, "Automatically given by Range Script");
                }
            }
        }

        /// <summary>
        /// Hide a remark that is currently on a participant. if remark is not on participant, do nothing.
        /// </summary>
        /// <param name="resultList"></param>
        public void HideRemarkOnParticipants(ResultList resultList)
        {
            var ranks = GetParticipantRanksAsList();
            foreach (var item in resultList.Items)
            {
                //I am not sure this will work. does contains look for the same exact object? or one that has the same form?
                if (ranks.Contains(item.Rank))
                {
                    item.Participant.RemarkList.Hide(Condition);
                }
            }
        }

        /// <summary>
        /// returns a list of [participant, remark, action] objects allows the medea RL to be updated by itself.
        /// </summary>
        /// <param name="resultList"></param>
        /// <returns></returns>
        public List<CommandAutomationIntermediate> IntermediateCommandAutomationRemarkList(ResultList resultList)
        {
            List<CommandAutomationIntermediate> caIntermediate = new List<CommandAutomationIntermediate>();
            var ranks = GetParticipantRanksAsList();
            foreach (var item in resultList.Items)
            {
                if (ranks.Contains(item.Rank))
                {
                    CommandAutomationIntermediateRemark interRemark = new CommandAutomationIntermediateRemark();
                    interRemark.visibility = Action; // doesn't matter what I am doing, it's gonna get passed off down the line.
                    interRemark.condition = Condition;
                    interRemark.participant = item.Participant;
                    caIntermediate.Add(interRemark);
                }
            }
            return caIntermediate;
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Subject} {Condition} {Action} on {ParticipantRanks}";
        }
    }
}
