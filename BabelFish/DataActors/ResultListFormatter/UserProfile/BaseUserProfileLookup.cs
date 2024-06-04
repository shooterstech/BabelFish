using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using NLog;
using Scopos.BabelFish.DataModel.AttributeValue;

namespace Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile {

    /// <summary>
    /// An in memory only implementation of the IUserProfileLookup interface. 
    /// Intended for
    /// a) Unit testing
    /// b) Consumers that do not have access to the database that stores
    /// c) As a base class for a database implementatioon. 
    /// the User Profile data.
    /// </summary>
    public class BaseUserProfileLookup : IUserProfileLookup {

        //Key is the user's cognito user id
        protected ConcurrentDictionary<string, UserProfile> UserProfileByUserIdCache = new ConcurrentDictionary<string, UserProfile>();
        //Key is the user's account url, aka rezults url
        protected ConcurrentDictionary<string, UserProfile> UserProfileByAccountUrlCache = new ConcurrentDictionary<string, UserProfile>();

        protected Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <inheritdoc/>
        public string AccountURLLookUp( string userId ) {
            if (!string.IsNullOrEmpty( userId )) {
                UserProfile userProfile;

                if (UserProfileByUserIdCache.TryGetValue( userId, out userProfile )
                    && userProfile.Visibility == VisibilityOption.PUBLIC)
                    return userProfile.AccountURL;
            }

            return null;
        }

        /// <inheritdoc/>
        public UserProfile GetUserProfile( string userId ) {

            if (!string.IsNullOrEmpty( userId )) {
                UserProfile userProfile;

                if (UserProfileByUserIdCache.TryGetValue( userId, out userProfile )
                    && userProfile.Visibility == VisibilityOption.PUBLIC)
                    return userProfile;
            }

            return null;
        }

        /// <inheritdoc/>
        public bool HasPublicProfile( string id ) {

            if (!string.IsNullOrEmpty( id )) {
                UserProfile userProfile;

                //As id may be either a user id or account url, look up both caches
                if (UserProfileByAccountUrlCache.TryGetValue( id, out userProfile ))
                    return userProfile.Visibility == VisibilityOption.PUBLIC;

                if (UserProfileByUserIdCache.TryGetValue( id, out userProfile ))
                    return userProfile.Visibility == VisibilityOption.PUBLIC;
            }

            return false;
        }

        /// <inheritdoc/>
        public virtual Task RefreshUserProfileVisibilityAsync() {
            //As this is an in memory only implementation, there is nothing to refresh.
            return Task.CompletedTask;
        }

        /// <summary>
        /// Clears all records from the cache.
        /// </summary>
        public virtual void ClearCache() {
            UserProfileByAccountUrlCache.Clear();
            UserProfileByUserIdCache.Clear();
        }

        /// <inheritdoc/>
        public void SetUserProfile( string userId, string accountURL, VisibilityOption visibility ) {
            var userProfile = new UserProfile() {
                UserId = userId,
                AccountURL = accountURL,
                Visibility = visibility
            };

            //Before we add in the new value, need to attempt to remove the old accountURL from cache.
            UserProfile oldProfile;
            if (UserProfileByUserIdCache.TryGetValue( userId, out oldProfile )) {
                if (oldProfile.AccountURL != accountURL) {
                    UserProfileByAccountUrlCache.TryRemove( oldProfile.AccountURL, out oldProfile );
                }
            }

            //Now we can add in the updated UserProfile values
            UserProfileByUserIdCache[userId] = userProfile;
            UserProfileByAccountUrlCache[accountURL] = userProfile;
        }

        /// <inheritdoc/>
        public string UserIdLookUp( string accountUrl ) {

            if (!string.IsNullOrEmpty( accountUrl )) {
                UserProfile userProfile;

                if (UserProfileByAccountUrlCache.TryGetValue( accountUrl, out userProfile )
                    && userProfile.Visibility == VisibilityOption.PUBLIC)
                    return userProfile.UserId;
            }

            return null;
        }
    }
}
