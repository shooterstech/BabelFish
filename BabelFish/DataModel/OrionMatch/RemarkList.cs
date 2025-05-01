
namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// A list of Remarks, each holding a RemarkName, reason, and a status (show/don't show)
    /// </summary>
    [Serializable]
    public class RemarkList : List<Remark> {
		//public List<Remark> remarks = new List<Remark>();

		public readonly List<ParticipantRemark> PriorityOfRemarks = new List<ParticipantRemark>() {
					ParticipantRemark.DSQ,
					ParticipantRemark.DNS,
					ParticipantRemark.DNF,
					ParticipantRemark.FIRST,
					ParticipantRemark.SECOND,
					ParticipantRemark.THIRD,
					ParticipantRemark.ELIMINATED,
					ParticipantRemark.BUBBLE,
					ParticipantRemark.LEADER};

		public void Add(ParticipantRemark remark, string reason)
        {
            var newRemark = new Remark();
            newRemark.ParticipantRemark = remark;
            newRemark.Reason = reason;
            newRemark.Visibility = RemarkVisibility.SHOW;
            this.Add(newRemark);
            this.SortRemarks();
        }
        public void Hide(ParticipantRemark remark)
        {
            Remark match = new Remark() { ParticipantRemark = remark, Visibility = RemarkVisibility.SHOW };
            SortRemarks();
            if (this.Count() > 0)
            {
                var thing = Find(match);
                if (thing != null)
                {
                    this[(int)thing].Visibility = RemarkVisibility.HIDE;
                }
            }
        }
        public void Remove(int spot)
        {
            this.RemoveAt(spot);
            this.SortRemarks();
        }

        /// <summary>
        /// Returns a boolean indicating of this Participant has the passed in ParticiapntRemark in it's RemarkList
        /// </summary>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool HasRemark( ParticipantRemark remark ) {
            foreach (var re in this) {
                if (re.ParticipantRemark == remark)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the user has one of the following remarks: DNS, DSQ, DNF, or ELIMINATED. 
        /// Returns flase if they do not (and would then mean that they are still competing).
        /// </summary>
        /// <returns></returns>
        public bool HasNonCompletionRemark() {

            foreach (var re in this) {
                switch (re.ParticipantRemark) {
                    case ParticipantRemark.DNS:
                    case ParticipantRemark.DNF:
                    case ParticipantRemark.DSQ:
                    case ParticipantRemark.ELIMINATED:
                        return true;
                    default:
                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// returns a boolean indicating the passed in remark is the last(most important) one. if require visible is true then it will return true when it is the last remark, but the visibility is HIDDEN
        /// </summary>
        /// <param name="remark"></param>
        /// <param name="requiredVisible"></param>
        /// <returns></returns>
        public bool IsLastRemark( ParticipantRemark remark, bool requiredVisible = true ) {
            SortRemarks();
            if (this.Count() > 0) {
                var lastRemark = this.Last();
                if (lastRemark.ParticipantRemark == remark) {
                    if (requiredVisible) {
                        if (lastRemark.Visibility == RemarkVisibility.SHOW) {
                            return true;
                        }
                    } else {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Sorts remark table of this participant, most important remarks are the last item on the list. Refer to Remark.Visibility if it should be displayed.
        /// </summary>
        public void SortRemarks() {
            this.OrderByDescending(y => (int)y.Visibility).OrderByDescending(x => (int)x.ParticipantRemark).ToList();
        }

        /// <inheritdoc />
        public override string ToString() {
            List<string> list = new List<string>();

            foreach (var pr in this) {
                if (pr.Visibility == RemarkVisibility.SHOW) {
                    list.Add( pr.ParticipantRemark.Description() );
                }
            }

            return string.Join( ", ", list );
        }

        /// <summary>
        /// Returns the most important Remark. If the participant has the Remark more than once, it indicates it on the return string.
        /// </summary>
        /// <remarks>Same(ish) as python's RemarkHelperFunctions.GetMostImportantRemark()</remarks>
        public string Summarize {
            get {

                //Key is the ParticipantRemark, value is the number of times it is included in this Remark List. Counts both Hidden and Shown remarks.
                Dictionary<ParticipantRemark, int> countOfRemarks = new Dictionary<ParticipantRemark, int>();

                //Count up each of the remarks.
                foreach (var pr in this) {
                    if (countOfRemarks.ContainsKey( pr.ParticipantRemark )) {
                        countOfRemarks[pr.ParticipantRemark] += 1;
                    } else {
                        countOfRemarks[pr.ParticipantRemark] = 1;
                    }
                }

                //Return something
                foreach( var pr in PriorityOfRemarks ) {
                    if ( countOfRemarks.ContainsKey( pr )) {
                        var count = countOfRemarks[pr];
                        if (count == 1)
                            return $"{pr}";
                        if (count > 1)
                            return $"{pr} x {count}";
                    }
                }

                return "";
            }
        }

        /// <summary>
        /// Find the location of the exact remark given, with no regard to the Reason string
        /// </summary>
        /// <param name="remark"></param>
        /// <returns></returns>
        internal int? Find(Remark remark)
        {
            SortRemarks();
            int spot = 0;
            foreach (var mark in this)
            {
                if(mark.ParticipantRemark == remark.ParticipantRemark &&
                    mark.Visibility == remark.Visibility)
                {
                    return spot;
                }
                spot++;
            }
            return null;
        }
    }
}
