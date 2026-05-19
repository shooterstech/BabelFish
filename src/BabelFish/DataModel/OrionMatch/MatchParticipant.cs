using System.Runtime.Serialization;


namespace Scopos.BabelFish.DataModel.OrionMatch {

    [Serializable]
    public class MatchParticipant :
        IParticipant,
        ICheckSum {

        public MatchParticipant() {
            Participant = new Individual();
            RoleList = new List<MatchParticipantRole>();
            MatchParticipantResults = new List<MatchParticipantResult>();
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (RoleList == null)
                RoleList = new List<MatchParticipantRole>();

            if (MatchParticipantResults == null)
                MatchParticipantResults = new List<MatchParticipantResult>();
        }


        public string MatchID { get; set; }

        public string MatchName { get; set; }

        public string ParentID { get; set; }

        /// <summary>
        /// Unique ID within this match, for this Match Participant.
        /// </summary>
        public string ParticipantID { get; set; }

        /// <summary>
        /// UUID formatted Orion Account user id. 
        /// </summary>
        public string UserID { get; set; }

        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime LocalDate { get; set; }

        public Participant Participant { get; set; }

        /// <summary>
        /// Intended to be a list of squadding events that the Participant competed it. However, the 
        /// format is not well organize and needs to be updated.
        /// </summary>
        public List<MatchParticipantResult> MatchParticipantResults { get; set; }

        /// <summary>
        /// A list of Authorization Roles the participant has.
        /// <para>Obsolete as of Orion version 2.25. </para>
        /// </summary>
        [Obsolete( "Use Participant.RoleList instead." )]
        public List<MatchParticipantRole> RoleList { get; set; }

        public override string ToString() {
            return "MatchParticipant for " + Participant.DisplayName;
        }

        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// String holding the software (Orion Scoring System) and Version number of the software.
        /// </summary>
        public string Creator { get; set; }

        /// <inheritdoc />
        public string CheckSum { get; set; }

        /// <inheritdoc />
        public ulong CalculateChecksum() {
            var combined = $"{MatchID}|{MatchName}|{ParticipantID}|{UserID}|{LocalDate.ToString( DateTimeFormats.DATE_FORMAT )}|{Creator}";
            var hash = Helpers.Common.Md5ToUlong( combined );

            if (Participant is not null) {
                hash ^= Participant.CalculateChecksum();
            }

            return hash;
        }
    }
}
