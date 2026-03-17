using System.ComponentModel;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A MatchParticipant is a <see cref="Individual"/> or a <see cref="Team"/> participating in a <see cref="Match"/>.
    /// </summary>
    [Serializable]
    public class MatchParticipant : IParticipant, ISaveToFile {

        public MatchParticipant() {
            Participant = new Individual();
        }

        /// <summary>
        /// Gets or sets the name of the match that this Participant participanted in.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string MatchName { get; set; }

        /// <summary>
        /// The unique Match ID that this Participant participanted in.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public MatchID MatchID { get; set; }

        /// <summary>
        /// Unique ID within this match, for this Match Participant.
        /// <para>ParticipantID differs from a Competitor Number in two ways. First, a Team does not have competitor numbers. Second, once created ParticipantId may not be changed while competitor numbers can.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public string ParticipantID { get; set; } = Scopos.BabelFish.Helpers.Common.GenerateUniqueId();

        /// <summary>
        /// UUID formatted Scopos Account user id.
        /// <para>If missing or an empty string, likely means this Participant is either a Team, or is an Individual but does not have a Scopos Account.</para>
        /// </summary>
        [DefaultValue( "" )]
        [G_NS.JsonProperty( Order = 4 )]
        public string UserID { get; set; } = string.Empty;

        /// <summary>
        /// Information about the Participant, which can be either an Individual or a Team. Includes name, attribute values, and if a team, the list of team members.
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public Participant Participant { get; set; }

        /// <summary>
        /// A list of entries (CourseOfFireEntry) for this Participant. Basically say which events this Participant is entered in, and what their squadding assignment is for each event. 
        /// </summary>
        /// <remarks>This property replaced MatchParticipantResults</remarks>
        [G_NS.JsonProperty( Order = 6 )]
        public List<CourseOfFireEntry> Entries { get; set; } = new List<CourseOfFireEntry>();



        /// <summary>
        /// String holding the software (Orion Scoring System) and Version number of the software.
        /// </summary>
        [G_NS.JsonProperty( Order = 97 )]
        public string Creator { get; set; }

        [G_NS.JsonProperty( Order = 99 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime LastUpdated { get; set; }

        #region
        /// <inheritdoc/>
        /// <param name="composite">Expected to be a <see cref="MatchComposite"/> instance.</param>
        public string GetFileName( IBaseName composite ) {
            /*
             * Ideally I would like to use Display Name for the file name. However, the user can change the display name and it's not guaranteed to be unique, 
             * so for now using ParticipantID, which is guaranteed to be unique and not changeable.
             */
            return $"{ParticipantID}.json";
        }

        /// <inheritdoc/>
        /// <param name="composite">Expected to be a <see cref="MatchComposite"/> instance.</param>
        public string GetRelativePath( IBaseName composite ) {
            return Path.Combine( composite.BaseName, this.GetFileName( composite ) );
        }

        /// <inheritdoc/>
        /// <param name="composite">Expected to be a <see cref="MatchComposite"/> instance.</param>
        public string SaveToFile( DirectoryInfo relativeDirectory, IBaseName composite ) {

            if (relativeDirectory == null)
                throw new ArgumentNullException( nameof( relativeDirectory ) );

            string filePath = Path.Combine( relativeDirectory.FullName, GetRelativePath( composite ) );

            var directoryPath = Path.GetDirectoryName( filePath );

            if (!Directory.Exists( directoryPath )) {
                Directory.CreateDirectory( directoryPath );
            }

            string json = G_NS.JsonConvert.SerializeObject( this, Helpers.SerializerOptions.NewtonsoftJsonSerializer );

            File.WriteAllText( filePath, json );

            return filePath;
        }

        /// <inheritdoc/>
        public string SaveToFile( FileInfo fileInfo ) {

            if (fileInfo == null)
                throw new ArgumentNullException( nameof( fileInfo ) );

            string json = G_NS.JsonConvert.SerializeObject( this, Helpers.SerializerOptions.NewtonsoftJsonSerializer );

            File.WriteAllText( fileInfo.FullName, json );

            return fileInfo.FullName;
        }

        /// <inheritdoc/>
        public string SerializeToJson() {
            string json = G_NS.JsonConvert.SerializeObject( this, Helpers.SerializerOptions.NewtonsoftJsonSerializer );

            return json;
        }
        #endregion

        /// <inheritdoc/>
        public override string ToString() {
            return Participant.DisplayName;
        }
    }
}
