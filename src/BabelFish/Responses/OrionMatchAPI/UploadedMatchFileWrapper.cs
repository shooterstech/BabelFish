using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class UploadedMatchFileWrapper : BaseClass {

        public UploadedMatchFile MatchObjectFile { get; set; } = new UploadedMatchFile();
    }

    public class UploadedMatchFile {
        public string S3Key { get; set; } = string.Empty;
        public string RelativeFileName { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        public int LicenseNumber { get; set; } = 0;
    }
}
