using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.APIClients {
    public class APIClientStatistics {

        private DateTime _firstApiCallTime = DateTime.Now;
        private int _numberOfApiCalls = 0;

        public static APIClientStatistics SITEWIDE = new APIClientStatistics();

        public APIClientStatistics() {

        }

        /// <summary>
        /// Returns the total number of API calls made since start up. Is the total of all types of API Clients.
        /// </summary>
        public int NumberOfApiCalls {
            get {
                return _numberOfApiCalls;
            }
        }

        /// <summary>
        /// Returns the overall rate of API calls, in calls per second.
        /// </summary>
        public double RateOfApiCalls {
            get {
                var totalTime = DateTime.Now - _firstApiCallTime;
                return _numberOfApiCalls / totalTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Increments both the instance and static SITEWIDE instance
        /// </summary>
        public void Increment() {
            _numberOfApiCalls++;
            SITEWIDE._numberOfApiCalls++;
        }
    }
}
