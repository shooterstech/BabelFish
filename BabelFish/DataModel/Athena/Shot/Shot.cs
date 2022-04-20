using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.DataModel.Athena;
using ShootersTech.DataModel.Athena.Interfaces;
using Medea.Match;
using Newtonsoft.Json;

namespace ShootersTech.DataModel.Athena.Shot {
    public class Shot : IEquatable<Shot>, IPenalty {

        /// <summary>Shot Attribute to indicate the shot was a sighter.</summary>
        public const string SHOT_ATTRIBUTE_SIGHTER = "SIGHTER";
        /// <summary>Shot Attribute to indicate the shot was a frame hit.</summary>
        public const string SHOT_ATTRIBUTE_FRAME_HIT = "FRAME HIT";
        /// <summary>Shot Attribute to indicate the shot was a missed shot.</summary>
        public const string SHOT_ATTRIBUTE_MISSED_SHOT = "MISSED SHOT";
        /// <summary>Shot Attribute to indicate the shot was simulated.</summary>
        public const string SHOT_ATTRIBUTE_SIMULATED = "SIMULATED";
        /// <summary>Shot Attribute to indicate the shot is an empty shot.</summary>
        public const string SHOT_ATTRIBUTE_EMPTY = "EMPTY";
        /// <summary>Shot Attribute to indicate the shot's precise cooredinates are not known.</summary>
        public const string SHOT_ATTRIBUTE_UNKNOWN_COORDINATES = "UNKNOWN COORDINATES";

        /// <summary>
        /// Public constructor
        /// </summary>
        public Shot() {
            UpdateLog = new List<ShotLog>();
            Attributes = new List<string>();
            Score = new Score();
            Penalties = new List<Penalty>();
        }

        /// <summary>
        /// Gets called after a Shot object is deserialized from JSON (or any other deserialziation really).
        /// Makes sure all of the List properties are not null. 
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            //Providate default values if they were not read during deserialization

            if (UpdateLog == null) {
                UpdateLog = new List<ShotLog>();
            }

            if (Attributes == null) {
                Attributes = new List<string>();
            }

            if (Score == null) {
                Score = new Score();
            }

            if (Penalties == null) {
                Penalties = new List<Penalty>();
            }
        }

        public Location Location { get; set; }

        public DateTime TimeScored { get; set; }

        public string RangeTime { get; set; }

        public float BulletDiameter { get; set; }

        public float ScoringDiameter { get; set; }

        public Athena.DataModel.Score Score { get; set; }

        public string TargetSetName { get; set; }

        /// <summary>
        /// The firing point number the shot was fired on. 
        /// </summary>
        public string FiringPoint { get; set; }

        public List<string> Attributes { get; set; }

        public string StageLabel { get; set; }

        /// <summary>
        /// An in order numbering of the shot, for the Result COF
        /// </summary>
        public float Sequence { get; set; }

        /// <summary>
        /// The IoT Thing Name for the Target that scored the shot.
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// GUID formatted string representing the ResultCOF
        /// </summary>
        public string ResultCOFID { get; set; }

        public string MatchID { get; set; }

        public dynamic Meta { get; set; }

        /// <summary>
        /// Returns the x and y coordinates, measured in pixels, of the aiming bull center in the verification photo.
        /// These values are read from the Meta dictionary using VerImgBullXCoor and VerImgBullYCoor.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown if the x and y coorediantes can not be read from the shot's Meta values.</exception>
        /// <returns></returns>
        public Tuple<float, float> GetVerificationImageAimingBullCoordinates() {
            try {
                float x = (float)Meta["VerImgBullXCoor"];
                float y = (float)Meta["VerImgBullYCoor"];
                Tuple<float, float> coordinates = new Tuple<float, float>(x, y);

                return coordinates;
            } catch (Exception e) {
                throw new KeyNotFoundException( "Unable to read the X and Y coorediantes of the aiming bull from the Shot's Meta data." );
            }
        }

        /// <summary>
        /// Returns the Dot per mm value (pixes per mm) of the verification image.
        /// This value is read from the Meta dictionary using VerImgDPMM
        /// </summary>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public float GetVerificationImageDPMM() {
            try {
                return (float) Meta["VerImgDPMM"];
            } catch (Exception e) {
                throw new KeyNotFoundException( "Unable to read the DPMM value from the Shot's Meta data." );
            }
        }

        /// <summary>
        /// Return the rotation of the verification image around the center of the aiming bull.
        /// Measured in degrees (not radians)
        /// </summary>
        /// <returns></returns>
        public float GetVerificationImageRotation() {
            try {
                return (float) Meta["VerImgRotation"];
            } catch (Exception e) {
                //If the VerImgRotation tag is not there, just assume there isn't any rotation
                return 0;
            }
        }

        /// <summary>
        /// Links to some other object that is related to this Shot.
        /// Most likely a VIS.Shot
        /// Non serializable property.
        /// </summary>
        [JsonIgnore]
        public object DataObjectTag { get; set; }

        public int Update { get; set; }

        public List<ShotLog> UpdateLog { get; set; }

        /// <summary>
        /// URL of the image showing the scored shot
        /// </summary>
        public string ValidationPhoto { get; set; }

        public bool Equals(Shot other) {
            return this.GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode() {
            return $"{this.ResultCOFID}-{this.Sequence}-{this.Update}".GetHashCode();
        }

        public override bool Equals(object obj) {
            return this.GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Helper property that indicates if this shot is marked as a deleted shot.
        /// A Deleted shot has "DELETED" as an Attribute
        /// </summary>
        [JsonIgnore]
        public bool IsADeletedShot {
            get {
                return this.Attributes.Contains( "DELETED" );
            }
        }

        /// <summary>
        /// Helper property that indicates if this shot is marked as a sighter shot.
        /// A Deleted shot has "SIGHTER" as an Attribute
        /// </summary>
        [JsonIgnore]
        public bool IsASighter {
            get {
                return this.Attributes.Contains( "SIGHTER" );
            }
        }

        /// <inheritdoc/>
        public List<Penalty> Penalties { get; set; }

        public override string ToString() {
            var sighter = IsASighter ? " SS" : "";
            var deleted = IsADeletedShot ? " DEL" : "";
            var updated = Update > 0 ? " UP" : "";
            return $"{Score.D.ToString("F1")} {ResultCOFID.Substring(0, 4)}: {StageLabel}-{Sequence}{sighter}{deleted}{updated}";
        }

        /// <inheritdoc/>
        public void AddPenalty( Penalty penalty ) {
            Penalties.Add(penalty);
        }

        /// <inheritdoc/>
        public bool HasPenalty( Penalty penalty ) {
            return Penalties.Contains(penalty);
        }

        /// <inheritdoc/>
        public bool HasPenalty( string penaltyID ) {
            foreach (var p in Penalties) {
                if (p.PenaltyID == penaltyID)
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void RemovePenalty( Penalty penalty ) {
            Penalties.Remove(penalty);
        }

        /// <inheritdoc/>
        public void RemovePenalty( string penaltyID ) {
            Penalty penaltyToRemove = null;
            foreach(var p in Penalties)
                if (p.PenaltyID == penaltyID) {
                    penaltyToRemove = p;
                    break;
                }

            if (penaltyToRemove != null)
                Penalties.Remove(penaltyToRemove);
        }

        /// <inheritdoc/>
        public List<Penalty> GetPenalties() {
            return Penalties;
        }

        /// <inheritdoc/>
        public void SetPenalties( List<Penalty> penalties ) {
            Penalties = penalties;
        }

        /// <inheritdoc/>
        public float GetSumPenaltyPoints() {
            float sum = 0;
            foreach (var p in Penalties)
                sum += p.PenaltyPoints;

            return sum;
        }
    }
}
