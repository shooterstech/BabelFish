using System.ComponentModel;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.ScoposData {

    /* 
     * If you add an enum here, it is usually best to also add it to the SerializerOptions.cs file.
     */

    public enum ApplicationName {

        [Description( "orion" )]
        [EnumMember( Value = "orion" )]
        ORION,

        [Description( "athena" )]
        [EnumMember( Value = "athena" )]
        ATHENA,

        [Description( "greengrassv2Deployment" )]
        [EnumMember( Value = "greengrassv2Deployment" )]
        GREENGRASSDEPLOYMENT
    }

    /// <summary>
    /// ImageCategory helps to categorize images based on their association with different entities within Rezults.
    /// It specifies whether the image is related to a Club, Match, League, or User profile.
    /// </summary>
    public enum ImageCategory {
        /// <summary>
        /// A Club image is an image associated with a club (aka Orion Account), such as a logo or a photo of the club's facilities.
        /// </summary>
        [Description( "Club" )]
        [EnumMember( Value = "Club" )]
        CLUB,

        /// <summary>
        /// A Match image is an image associated with a Match, such as a photo of the players or the award ceremony.
        /// </summary>
        [Description( "Match" )]
        [EnumMember( Value = "Match" )]
        MATCH,

        /// <summary>
        /// A League image is an image associated with a league, such as a logo or a header image for the League.
        /// </summary>
        [Description( "League" )]
        [EnumMember( Value = "League" )]
        LEAGUE,

        /// <summary>
        /// A User image is an image associated with a user profile (aka Scopos Account), such as a profile picture or a cover photo.
        /// </summary>
        [Description( "User" )]
        [EnumMember( Value = "User" )]
        USER
    }

    /// <summary>
    /// Allowed file types for images uploaded to Rezults. This enum specifies the supported image formats, which are JPEG and PNG.
    /// </summary>
    public enum ImageFileType {
        /// <summary>
        /// The JPEG file type is a commonly used method of lossy compression for digital images, particularly for those images produced by digital photography.
        /// </summary>
        [Description( "jpeg" )]
        [EnumMember( Value = "jpeg" )]
        JPEG,

        /// <summary>
        /// The PNG file type is a raster-graphics file format that supports lossless data compression. It is widely used for web images and supports transparency.
        /// </summary>
        [Description( "png" )]
        [EnumMember( Value = "png" )]
        PNG
    }

    /// <summary>
    /// ImageGroupKey helps to categorize images based on their intended use within Rezults. It specifies where the image is
    /// meant to be displayed, such as in headers, profiles, or as part of a bulk collection of images.
    /// </summary>
    public enum ImageGroupKeyType {
        /// <summary>
        /// The Header group key means the image is intended to be a wide image displayed in the Club Page header, Match Page header, League Page header, or user profile page header.
        /// <para>It is expected that there is only one header image per Club / Match / User Profile / League.</para>
        /// </summary>
        [Description( "Header" )]
        [EnumMember( Value = "Header" )]
        HEADER,

        /// <summary>
        /// The Profile group key means the image is intended to be Club / Match / User Profile / League profile photo. For a Club, Match, or League this is likely a logo.
        /// For a User Profile, this is likely a profile picture (head shot).
        /// <para>It is expected there is only one profile image per Club / Match / User Profile / League.</para>
        /// </summary>
        [Description( "Profile" )]
        [EnumMember( Value = "Profile" )]
        PROFILE,

        /// <summary>
        /// The Bulk group key means the image is one of many photos associated with a Club / Match / User Profile / League.
        /// </summary>
        [Description( "Bulk" )]
        [EnumMember( Value = "Bulk" )]
        BULK
    }

    /// <summary>
    /// ImageSafeToShowStatus represents the moderation status of an image uploaded to Rezults. It indicates whether the
    /// image is new and pending review, approved for display, rejected due to inappropriate content, or flagged for further review.
    /// </summary>
    public enum ImageSafeToShowStatus {

        /// <summary>
        /// Image is new and has not yet been reviewed for safety. It is pending moderation and should not be displayed publicly until it has been approved.
        /// </summary>
        [Description( "New" )]
        [EnumMember( Value = "New" )]
        NEW,

        /// <summary>
        /// Image has been reviewed and approved for display. It is considered safe to show publicly.
        /// </summary>
        [Description( "Approved" )]
        [EnumMember( Value = "Approved" )]
        APPROVED,

        /// <summary>
        /// Image has been reviewed and rejected due to inappropriate content. It should not be displayed publicly.
        /// </summary>
        [Description( "Rejected" )]
        [EnumMember( Value = "Rejected" )]
        REJECTED,

        /// <summary>
        /// Image has been flagged for further review. It may require additional moderation before a final decision is
        /// made regarding its display status. It should not be displayed publicly until it has been reviewed and approved.
        /// </summary>
        [Description( "Flagged" )]
        [EnumMember( Value = "Flagged" )]
        FLAGGED
    }

    public enum ReleasePhase {

        [Description( "alpha" )]
        [EnumMember( Value = "alpha" )]
        ALPHA,

        [Description( "beta" )]
        [EnumMember( Value = "beta" )]
        BETA,

        [Description( "integrated" )]
        [EnumMember( Value = "integrated" )]
        INTEGRATED,

        [Description( "production" )]
        [EnumMember( Value = "production" )]
        PRODUCTION
    }
}
