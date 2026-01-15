using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// An IRLIFItem implements two interfaces ISquadding and IParticipant.
    /// <para>The ResultListIntermediateFormatter is written in such a way that .Squadding may be null. 
    /// If it is, and a field calls upon it, the return value is an empty string.</para>
    /// </summary>
    public interface IRLIFItem : ISquadding, IParticipant {

    }

    /// <summary>
    /// An IRLIFList represents an object, with a list of IRLIFItems, that may be used with the Result List Intermediate
    /// Formatter. The two known objects are SquaddingList and ResultList.
    /// <para>Note: ResultList has a list of ResentEvent (which implement IRLIFItem), but contains many other properties
    /// used by the Result List INtermediate Formatter. Such as Score or Rank.</para>
    /// </summary>
    public interface IRLIFList {

        /// <summary>
        /// Method to return the list of IRLIFItem items. 
        /// <para>Both SquaddingList and ResultList have a List of Squadding or List of ResultEvent already. However there is no
        /// straightforward way to case these lists as List of IRLIFItem. This method bridges that gap.</para>
        /// </summary>
        /// <returns></returns>
        List<IRLIFItem> GetAsIRLItemsList();

        /// <summary>
        /// The default set name of the RESULT LIST FORMAT definition to use to format this IRLIFList object. 
        /// </summary>
        string ResultListFormatDef { get; }

        /// <summary>
        /// The (mostly human readable) name given to this IRLIFLIst object.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The start date that the underlying event started on.
        /// </summary>
        DateTime StartDate { get; }

        /// <summary>
        /// The end date that the underlying event ended on.
        /// </summary>
        DateTime EndDate { get; }

        /// <summary>
        /// The ResultStatus of the underlying event.
        /// </summary>
        ResultStatus Status { get; }

        /// <summary>
        /// The Parent ID of the match
        /// </summary>
        string ParentID { get; set; }

        /// <summary>
        /// On RESULT LIST FORMAT definitions that provided for the option, the user (usually the Match Director) may specify their own
        /// interpolated values for designated fields. These are known as UserDefinedText. There are at most three user defined fields in a
        /// RESULT LIST FORMAT (man definitions do not have any).
        /// <para>The most common example is a demographic spanning text field.</para>
        /// <para>Text values are interpolated with any common field or user defined field. The list is common fields is at
        /// <see href="https://support.scopos.tech/index.html?definition-resultlistfield.html">support.scopos.tech</see></para>
        /// <para>Example text values:</para>
        /// <list type="bullet">
        /// <item>"Competitor Number: {CompetitorNumber}, Hometown: {Hometown}</item>
        /// <item>"Club: {Organization}, Coach: {Coach}</item>
        /// </list>
        /// </summary>
        public Dictionary<UserDefinedFieldNames, string> UserDefinedText { get; set; }
    }
}
