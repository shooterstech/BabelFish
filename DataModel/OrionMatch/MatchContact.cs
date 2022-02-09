using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Represents the name and contact information for the match directory.
    /// </summary>
    [Obsolete("To be replaced soon with a better method of tracking the match contact.")]
    public class MatchContact {

        public MatchContact() { }

        /// <summary>
        /// The name of the person to contact regarding the match.
        /// </summary>
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
