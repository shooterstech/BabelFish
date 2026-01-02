using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class POC {

        /// <summary>
        /// The Cognito User ID, formatted as a GUID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// The Cognito account Email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The email address to display
        /// </summary>
        public string DisplayEmail { get; set; }

        /// <summary>
        /// The full name to display
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The phone number to display.
        /// </summary>
        public string DisplayPhone { get; set; }
    }
}
