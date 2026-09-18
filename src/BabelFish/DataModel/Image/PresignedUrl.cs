namespace Scopos.BabelFish.DataModel.Image {
    public class PresignedUrl {

        public string Url { get; set; } = string.Empty;

        public string S3Key { get; set; } = string.Empty;

        public override string ToString() {
            return S3Key;
        }
    }
}
