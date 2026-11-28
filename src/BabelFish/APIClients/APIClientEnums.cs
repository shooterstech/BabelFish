using System.ComponentModel;
using System.Runtime.Serialization;
using Scopos.BabelFish.Requests.ImageAPI;

namespace Scopos.BabelFish.APIClients {

    /// <summary>
    /// Different REST API environments that may be callsed.
    /// </summary>

    public enum APIClientEnums {
        /// <summary>
        /// Alpha test environment.
        /// </summary>
        [Description( "alpha" )]
        [EnumMember( Value = "alpha" )]
        ALPHA,

        /// <summary>
        /// Beta test environment.
        /// </summary>
        [Description( "beta" )]
        [EnumMember( Value = "beta" )]
        BETA,

        /// <summary>
        /// Represents the prodtest environment.
        /// </summary>
        /// <remarks>Serialized as "prodtest".</remarks>
        [Description( "prodtest" )]
        [EnumMember( Value = "prodtest" )]
        PRODTEST,

        /// <summary>
        /// Production environment
        /// </summary>
        [Description( "production" )]
        [EnumMember( Value = "production" )]
        PRODUCTION
    }

    /// <summary>
    /// When uploading images to S3, viea the <see cref="ImageClient"/> and <see cref="GetPresignedUrlRequest"/>, the request must specify the type of upload.
    /// This enum defines the different types of uploads that are supported.
    /// </summary>
    public enum ImageUploadType {

        /// <summary>
        /// Image file
        /// </summary>
        [Description( "image" )]
        [EnumMember( Value = "image" )]
        IMAGE,

        /// <summary>
        /// A json file within a Match Project. May also be a jsonl file.
        /// </summary>
        [Description( "match" )]
        [EnumMember( Value = "match" )]
        MATCH_PROJECT,

        /// <summary>
        /// Reconfigurable Rulebook definiiton file.
        /// </summary>
        [Description( "definition" )]
        [EnumMember( Value = "definition" )]
        DEFINITION
    }
}
