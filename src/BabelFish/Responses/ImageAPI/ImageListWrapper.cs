using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Image;

namespace Scopos.BabelFish.Responses.ImageAPI {
    public class ImageListWrapper : BaseClass {

        public ImageList BulkImages { get; set; }
    }
}
