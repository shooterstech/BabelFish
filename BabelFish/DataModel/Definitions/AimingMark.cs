using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// 
    /// </summary>

    [Serializable]
    public class AimingMark : ScoringShapeDimension {

        /// <summary>
        /// Public constructor
        /// </summary>
        public AimingMark() { }

		/// <summary>
		/// the color of the aiming mark
		/// </summary>
		[G_NS.JsonProperty( DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		public AimingMarkColor Color { get; set; } = AimingMarkColor.BLACK;
    }
}
