using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public static class CommandAutomationExtensions {

        /// <summary>
        /// functionally the same as adding remarks or a participant
        /// </summary>
        /// <param name="resultList"></param>
        public static void ShowRemarkOnParticipants(this CommandAutomationRemark remark, ResultList resultList ) {
            var ranks = remark.GetParticipantRanksAsList();
            foreach (var item in resultList.Items) {
                if (ranks.Contains( item.Rank )) {
                    item.Participant.RemarkList.Add( remark.Condition, "Automatically given by Range Script" );
                }
            }
        }

        /// <summary>
        /// hide remark on participants that it exists on.
        /// </summary>
        /// <param name="remark"></param>
        /// <param name="resultList"></param>
        public static void HideRemarkOnParticipants(this CommandAutomationRemark remark, ResultList resultList ) {
            var ranks = remark.GetParticipantRanksAsList();
            foreach (var item in resultList.Items) {
                //I am not sure this will work. does contains look for the same exact object? or one that has the same form?
                if (ranks.Contains( item.Rank )) {
                    item.Participant.RemarkList.Hide( remark.Condition );
                }
            }
        }

        /// <summary>
        /// return a list of command automation intermediate objects [participant, remark, visibility] typically. mostly for medea to update itself.
        /// </summary>
        /// <param name="remark"></param>
        /// <param name="resultList"></param>
        /// <returns></returns>
        public static List<CommandAutomationIntermediate> IntermediateCommandAutomationRemarkList(this CommandAutomationRemark remark, ResultList resultList ) {
            List<CommandAutomationIntermediate> caIntermediate = new List<CommandAutomationIntermediate>();
            var ranks = remark.GetParticipantRanksAsList();
            foreach (var item in resultList.Items) {
                if (ranks.Contains( item.Rank )) {
                    CommandAutomationIntermediateRemark interRemark = new CommandAutomationIntermediateRemark();
                    interRemark.visibility = remark.Action; // doesn't matter what I am doing, it's gonna get passed off down the line.
                    interRemark.condition = remark.Condition;
                    interRemark.participant = item.Participant;
                    caIntermediate.Add( interRemark );
                }
            }
            return caIntermediate;
        }
    }
}
