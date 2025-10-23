using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// MatchBase is the soon-to-be replacement of the Match class.
    /// </summary>
    public abstract class MatchBase {

        /// <summary>
        /// Concrete class identifier
        /// </summary>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public MatchType MatchType { get; set; }

        /// <summary>
        /// The Unique Match ID
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        public MatchID MatchId { get; set; }

        /* Order = 3 reserved for concrete classes */

        /// <summary>
        /// Human readable name given to the match. Does not have to be unique (but is helpful if it is).
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        public string MatchName { get; set; }

        //EKA Question Oct 2025: Should we have a ShortMatchName. The idea is, if this match is part of a Tournament, this ShortMatchName can be used as the default column header name.

        /* Order = 5 reserved for concrete classes */

        /// <summary>
        /// The account identifier that owns this Match.
        /// <para>Will be in the form OrionAcct000001 or AtHome000001.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 6 )]
        public string OwnerId { get; set; } = string.Empty;

        /* Order = 7, 8, 9 skipped for future use */

        /// <summary>
        /// The start date of the match
        /// </summary>
        /// <example>2001-01-01</example>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_NS.JsonProperty( Order = 10 )]
        public DateTime StartDate { get; set; } = DateTime.Today;

        /// <summary>
        /// The end date of the match
        /// </summary>
        /// <example>2001-01-01</example>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_NS.JsonProperty( Order = 11 )]
        public DateTime EndDate { get; set; } = DateTime.Today;

        /// <summary>
        /// The visibility (who can see it) given to this match. 
        /// </summary>
        [G_NS.JsonProperty( Order = 12, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;

        /* Order = 13, 14 skipped for future use */

        /// <summary>
        /// Indicates if this match should be include on match search results.
        /// </summary>
        [G_NS.JsonProperty( Order = 15 )]
        public bool IncludeInSearchResults { get; set; } = false;

        /// <summary>
        /// List of officials for this match.
        /// </summary>
        /// <remarks>OCT 2025: Currently unknown what this object will look like.</remarks>
        public List<object> Officials { get; set; } = new List<object>();

        /// <summary>
        /// Newtonsoft helper method to determine if Officials should be part of the serialized json.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeOfficials() {
            return Officials != null && Officials.Count > 0;
        }
    }
}
