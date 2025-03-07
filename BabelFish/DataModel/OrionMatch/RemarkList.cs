using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch
{
    /// <summary>
    /// A list of Remarks, each holding a RemarkName, reason, and a status (show/don't show)
    /// </summary>
    [Serializable]
    public class RemarkList : List<Remark>
    {
        //public List<Remark> remarks = new List<Remark>();

        /*
        public void Add(ParticipantRemark remark, string reason)
        {
            var newRemark = new Remark();
            newRemark.ParticipantRemark = remark;
            newRemark.Reason = reason;
            newRemark.Visibility = RemarkVisibility.SHOW;
            remarks.Add(newRemark);
            this.SortRemarks();
        }
        public void Remove(int spot)
        {
            remarks.RemoveAt(spot);
            this.SortRemarks();
        }*/

        /// <summary>
        /// Returns a boolean indicating of this Participant has the passed in ParticiapntRemark in it's RemarkList
        /// </summary>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool HasRemark(ParticipantRemark remark)
        {
            foreach (var re in this)
            {
                if (re.ParticipantRemark == remark)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// returns a boolean indicating the passed in remark is the last(most important) one. if require visible is true then it will return true when it is the last remark, but the visibility is HIDDEN
        /// </summary>
        /// <param name="remark"></param>
        /// <param name="requiredVisible"></param>
        /// <returns></returns>
        public bool IsLastRemark(ParticipantRemark remark, bool requiredVisible = true)
        {
            SortRemarks();
            if (this.Count() > 0)
            {
                var lastRemark = this.Last();
                if (lastRemark.ParticipantRemark == remark)
                {
                    if (requiredVisible)
                    {
                        if (lastRemark.Visibility == RemarkVisibility.SHOW)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Sorts remark table of this participant, most important remarks are the last item on the list. Refer to Remark.Visibility if it should be displayed.
        /// </summary>
        public void SortRemarks()
        {
            this.OrderByDescending(x => (int)x.ParticipantRemark).ToList();
        }

    }
}
