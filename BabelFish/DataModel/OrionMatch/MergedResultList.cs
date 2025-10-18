using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataActors.Tournaments;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class MergedResultList {

        /// <summary>
        /// Globally unique identifier assigned to this MergedResultList.
        /// <para>The SQL table tournament_merged_result_list assigns this value.</para>
        /// <para>A value of 0 means a value hasn't been assigned yet.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public int MergedId { get; set; } = 0;

        /// <summary>
        /// The human readable name of this Result List
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public string ResultName { get; set; } = string.Empty;

        /// <summary>
        /// The Tournament Method identifier to use to merge this Result List.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public string Method {  get; set; } = string.Empty;

        [G_NS.JsonProperty( Order = 4 )]
        public List<ResultListMember> ResultListMembers { get; set; } = new List<ResultListMember>();

        /// <summary>
        /// Method specific Configuration
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        [G_STJ_SER.JsonIgnore] //Ignore until I can figure out what this will look like.
        public MergeConfiguration Configuration { get; set; }
    }
}
