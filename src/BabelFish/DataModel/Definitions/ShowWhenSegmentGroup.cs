namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// ShowWhen operations describe logic for when a <seealso cref="ResultListFormat">RESULT LIST FORMAT</seealso>
    /// <seealso cref="ResultListDisplayColumn"/>, <seealso cref="ClassSet"/>, or SpanningText is included and displayed.
    /// <para>A ShowWhenSegmentGroup is a Show-When expression that evalutes to true or false, based on the value of the SegmentGroupName
    /// stored in teh ResultList's MetaData.</para>
    /// </summary>
    public class ShowWhenSegmentGroup : ShowWhenBase {

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ShowWhenSegmentGroup() {
            Operation = ShowWhenOperation.SEGMENT_GROUP;
        }

        /// <summary>
        /// the name of the SegmentGroup (based on the Coruse of Fire's Range Script), that the competition must be in 
        /// in order for this ShowWhen to be true. It is stored in the Result List's metadata.
        /// </summary>
        public string SegmentGroupName { get; set; }

        /// <inheritdoc />
        public override string ToString() {
            return $"{SegmentGroupName}";
        }
    }
}
