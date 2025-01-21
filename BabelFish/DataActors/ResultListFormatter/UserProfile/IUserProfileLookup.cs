using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile
{

    /// <summary>
    /// Interface class that describes functions that looks up and stores data 
    /// about if a user profile, identified by an ID (usually GUID), if they have
    /// a public scopos profile.
    /// 
    /// Intended to fulfil the Dependency Injection pattern.
    /// https://www.geeksforgeeks.org/dependency-injectiondi-design-pattern/#what-is-the-dependency-injection-method-design-pattern
    /// </summary>
    public interface IUserProfileLookup {

        /// <summary>
        /// Refreshes and updates the cache copy of the user_profile table. 
        /// It is a best practice to call this method after constructing a UserProfileDB instance.
        /// </summary>
        Task RefreshUserProfileVisibilityAsync();

        /// <summary>
        /// Temporarily overrides the user profile values in the cache for the passed in user id.
        /// These values will remain active, until the next time the cache is updated from the database, which is every 15 mins or so.
        /// </summary>
        /// <param name="userId">Cognito UserID. UUID formatted</param>
        /// <param name="accountURL">User's account url</param>
        /// <param name="visibility"></param>
        void SetUserProfile( string userId, string accountURL, VisibilityOption visibility );

        /// <summary>
        /// Returns true, if the passed in id has a public profile page.
        /// User should call RefreshUserProfileVisibilityAsync() prior to calling this method to help ensure cache is udpated.
        /// </summary>
        /// <param name="id">May either be the User Id or Account URL</param>
        /// <returns></returns>
        bool HasPublicProfile( string id );

        /// <summary>
        /// Returns the passed in user's Account URL based on the passed in User Id.
        /// Returns null if the user doesn't have one, or if the Visibility is not public.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string AccountURLLookUp( string userId );

        /// <summary>
        /// Returns the passed in user's User Id based on the passed in Account URL.
        /// Returns null if the user doesn't have one, or if the Visibility is not public.
        /// </summary>
        /// <param name="accountUrl"></param>
        /// <returns></returns>
        public string UserIdLookUp( string accountUrl );

        /// <summary>
        /// Returns the complete User Profile Object, based on either the passed in Account URL or User ID (either values are accepted)
        /// Returns null if the user doesn't have one, or if the Visibility is not public.
        /// </summary>
        /// <param name="id">May either be the User Id or Account URL</param>
        /// <returns></returns>
        public UserProfile GetUserProfile( string userId );
    }
}
