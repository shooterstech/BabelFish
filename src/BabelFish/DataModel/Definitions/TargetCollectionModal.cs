using System;
using System.Collections.Generic;
using System.ComponentModel;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class TargetCollectionModal: IReconfigurableRulebookObject, IGetTargetDefinitionList {

        /// <summary>
        /// A human readable name for this Target Collection. Will be used by the COURSE OF FIRE to reference this Target Collection.
        ///
        /// Required, may not be an empty string.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string TargetCollectionName { get; set; } = string.Empty;

        /// <summary>
        /// The list of TARGET definitions that compose this Target Collection Modal. 
        /// 
        /// Required, may not be an empty list. All values must be known TARGET definition set names.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public List<SetName> TargetDefs { get; set; } = new List<SetName>();

        /// <summary>
        /// This Target Collection Modal is intended for use on a range with thie Range Distance.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public string RangeDistance { get; set; } = "10m";

        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public async Task<Dictionary<SetName, Target>> GetTargetDefinitionListAsync() {
            Dictionary<SetName, Target> targets = new();

            foreach (var targetDef in TargetDefs) {
                targets.Add( targetDef, await DefinitionCache.GetTargetDefinitionAsync( targetDef ) );
            }

            return targets;
        }

        /// <inheritdoc/>
        public override string ToString() {
            return $"TargetCollectionModal {TargetCollectionName}";
        }
    }
}
