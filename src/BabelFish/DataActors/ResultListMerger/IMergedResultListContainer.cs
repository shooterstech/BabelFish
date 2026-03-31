using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {

    /// <summary>
    /// An IMergedResultListContainer is a class that can contain MergedResultLists. It provides the necessary context and services for
    /// the <see cref="ResultListMergerEngine"/> to merge <see cref="ResultList"/> together. 
    /// </summary>
    public interface IMergedResultListContainer {

        /// <summary>
        /// EventHander that gets involed when a new MergedResultList is added to this container. The MergedResultList is passed in as an argument. 
        /// </summary>
        public event EventHandler<EventArgs<MergedResultList>> OnMergedResultListAdded;

        /// <summary>
        /// The list of MergedResultLists that are contained in this container. The preferred way to add a MergedResultList to this list is through the
        /// <see cref="CreateMergedResultListAsync(string, MergeMethodType)"/> method, as this ensures that the <see cref="OnMergedResultListAdded"/> event gets invoked.
        /// </summary>
        public List<MergedResultList> MergedResultLists { get; set; }

        /// <summary>
        /// Creates a new MergedResultList with the specified name and merge method type, adds it to
        /// the <see cref="MergedResultLists"/> list, and invokes the <see cref="OnMergedResultListAdded"/> event.
        /// </summary>
        /// <param name="resultListName">The name of the new MergedResultList.</param>
        /// <param name="mergeMethodType">The type of merge method to use.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created MergedResultList.</returns>
        public Task<MergedResultList> CreateMergedResultListAsync( string resultListName, MergeMethodType mergeMethodType );

        /// <summary>
        /// The MatchID of the Container. This is necessary for the <see cref="ResultListMergerEngine"/> to fetch the necessary ResultLists to perform the merge.
        /// </summary>
        public MatchID MatchId { get; }

        /// <summary>
        /// The class instance that knows how to retrieve ResultLists of a given <see cref="MergedResultList"/>. This is necessary
        /// for the <see cref="ResultListMergerEngine"/> to fetch the necessary ResultLists to perform the merge.
        /// </summary>
        public IResultListFetcher ResultListFetcher { get; }
    }
}
