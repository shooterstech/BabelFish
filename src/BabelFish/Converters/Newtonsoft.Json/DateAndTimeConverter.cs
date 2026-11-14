using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.Converters.Newtonsoft {

    /// <summary>
    /// Custom JSON converter for date only DateTime objects that formats dates in the "yyyy-MM-dd" format. This converter is used to ensure that date values are serialized and deserialized consistently in the specified format when working with JSON data.
    /// </summary>
    /// <remarks>
    /// Typical usage:
    /// <para>[G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]</para>
    /// <para>[G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]</para>
    /// </remarks>
    public class DateConverter : IsoDateTimeConverter {

        /// <summary>
        /// Constructor
        /// </summary>
        public DateConverter() {
            base.DateTimeFormat = DateTimeFormats.DATE_FORMAT;
        }
    }

    /// <summary>
    /// Custom JSON converter for DateTime objects (date plus time) that formats dates in the "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffK" format. This converter is used to ensure that date values are serialized and deserialized consistently in the specified format when working with JSON data.
    /// </summary>
    /// <remarks>
    /// Typical usage:
    /// <para>[G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateTimeConverter ) )]</para>
    /// <para>[G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]</para>
    /// </remarks>
    public class DateTimeConverter : IsoDateTimeConverter {

        /// <summary>
        /// Constructor
        /// </summary>
        public DateTimeConverter() {
            base.DateTimeFormat = DateTimeFormats.DATETIME_FORMAT;
        }
    }
}
