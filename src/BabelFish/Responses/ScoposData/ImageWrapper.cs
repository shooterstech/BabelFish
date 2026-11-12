using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Image;

namespace Scopos.BabelFish.Responses.ScoposData {
    public class ImageWrapper : BaseClass {

        public ScoposImage Image { get; set; } = new ScoposImage();
    }
}
