using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A ShowWhenVariable is a Show-When expression that evalutes to true or false, based on the run time value of an Condition. 
    /// Common examples would be RESULT_STATUS_INTERMEDIATE, to evalutes to true, if the Result List's status is Intermediate. 
    /// Or, DIMENSION_LARGE if the screen resolution is large (as defined by Bootstrap 5).
    /// </summary>
    public class ShowWhenVariable : ShowWhenBase {

        public static readonly ShowWhenVariable ALWAYS_SHOW = new ShowWhenVariable() {
            Condition = ShowWhenCondition.TRUE
        };

        public static readonly ShowWhenVariable NEVER_SHOW = new ShowWhenVariable() {
            Condition = ShowWhenCondition.FALSE
        };

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ShowWhenVariable() {
            Operation = ShowWhenOperation.VARIABLE;
        }

        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public ShowWhenCondition Condition { get; set; }

        public override string ToString() {
            return $"{Condition}";
        }
    }
}
