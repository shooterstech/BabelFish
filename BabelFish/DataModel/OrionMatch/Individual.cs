using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ShootersTech.DataModel.OrionMatch {
    /// <summary>
    /// Any Individual person who is participating in a match. These include athletes, coaches,
    /// and match officials. Does not include Teams.
    /// </summary>
    [Serializable]
    public class Individual : Participant {

        public const int CONCRETE_CLASS_ID = 1;

        public Individual() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// Individual's Given or First Name
        /// </summary>
        public string GivenName { get; set; } = string.Empty;

        [JsonIgnore]
        public string FirstName {
            get { return this.GivenName; }
            set { this.GivenName = value; }
        }

        /// <summary>
        /// Individual's middle or secondary Given name
        /// </summary>
        public string MiddleName { get; set; } = string.Empty;

        /// <summary>
        /// Individual's Family or Last name
        /// </summary>
        public string FamilyName { get; set; } = string.Empty;

        public string LastName {
            get { return this.FamilyName; }
            set { this.FamilyName = value; }
        }

        public string UserID { get; set; } = string.Empty;
    }
}
