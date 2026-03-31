using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {
    public interface IMergedResultListContainer {

        public event EventHandler<EventArgs<MergedResultList>> OnMergedResultListAdded;

        public List<MergedResultList> MergedResultLists { get; set; }

        public Task<MergedResultList> CreateMergedResultListAsync( string resultListName, MergeMethodType mergeMethodType );

        public MatchID MatchId { get; }

        public IResultListFetcher ResultListFetcher { get; }
    }
}
