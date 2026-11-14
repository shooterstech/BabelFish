using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Image;

namespace Scopos.BabelFish.Responses.ImageAPI {
    /// <summary>
    /// The named payload returned by image deletion.
    /// </summary>
    public class DeletedImageWrapper : BaseClass {
        /// <summary>
        /// Deletion confirmation, absent when the request fails.
        /// </summary>
        public ScoposImageAbbr? DeletedImage { get; set; }
    }
}
