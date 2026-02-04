using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {

    public abstract class DynamicResultListFormat<T> {

        public abstract Task<ResultListFormat> GenerateAsync( T item );
    }
}
