namespace Scopos.BabelFish.DataModel.Definitions {


    /// <summary>
    /// ShowWhen operations describe logic for when a <seealso cref="ResultListFormat">RESULT LIST FORMAT</seealso>
    /// <seealso cref="ResultListDisplayColumn"/>, <seealso cref="ClassSet"/>, or SpanningText is included and displayed.
    /// <para>
    /// A ShowWhenVariable is a Show-When expression that evalutes to true or false, based on the run time value of an Condition. 
    /// Common examples would be RESULT_STATUS_INTERMEDIATE, to evalutes to true, if the Result List's status is Intermediate. 
    /// Or, DIMENSION_LARGE if the screen resolution is large (as defined by Bootstrap 5).
    /// </para>
    /// </summary>
    public class ShowWhenVariable : ShowWhenBase {

        /// <summary>
        /// Common ShowWhenVariable that evaluates to always showing.
        /// </summary>
        public static readonly ShowWhenVariable ALWAYS_SHOW = new ShowWhenVariable() {
            Condition = ShowWhenCondition.TRUE
        };

        /// <summary>
        /// Common ShowWhenVariable that evaluates to never showing.
        /// </summary>
        public static readonly ShowWhenVariable NEVER_SHOW = new ShowWhenVariable() {
            Condition = ShowWhenCondition.FALSE
        };

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ShowWhenVariable() {
            Operation = ShowWhenOperation.VARIABLE;
        }

        /// <summary>
        /// The ShowWhenCondition to apply. 
        /// </summary>
        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public ShowWhenCondition Condition { get; set; }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Condition}";
        }
    }
}
