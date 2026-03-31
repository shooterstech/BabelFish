using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {


    public interface IResultListFetcher {

        public Task<List<ResultList>> GetResultListsAsync( MergedResultList mergedResultList );

    }
}
