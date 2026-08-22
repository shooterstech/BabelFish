using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.ScoposData;

namespace Scopos.BabelFish.Responses.ScoposData {
    public class ImageWrapper : BaseClass {

        public ScoposImage Image { get; set; } = new ScoposImage();
    }
}
