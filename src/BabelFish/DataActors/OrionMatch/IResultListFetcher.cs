using Scopos.BabelFish.DataActors.ResultListMerger;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// An IResultListFetcher is needed by the <see cref="MergedResultList"/> to fetch the Result Lists that it needs to merge together.
    /// The IResultListFetcher is used by the <see cref="MergeMethod"/> when it needs to get the Result Lists that will be merge together.
    /// </summary>
    /// <remarks>Implemented as an interface because both <see cref="Tournament"/> and <see cref="MatchStructure"/> can be this containers, and
    /// each of them has a different way of fetching the Result Lists that need to be merged together.</remarks>
    public interface IResultListFetcher {

        /// <summary>
        /// Returns the set of complete ResultList instances that are specified by the MergedResultList. The MergedResultList will specify the
        /// Result Lists that it needs to merge together, and the IResultListFetcher will return those Result Lists as complete instances that can be merged together.
        /// </summary>
        /// <param name="mergedResultList">The MergedResultList that specifies which Result Lists need to be fetched.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of ResultList instances.</returns>
        public Task<List<ResultList>> GetResultListsAsync( MergedResultList mergedResultList );

    }
}
