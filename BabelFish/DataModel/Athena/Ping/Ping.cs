using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.Ping {
    public class Ping {

        //When a Ping object is created, it is created after a response from a thing
        //responseTime is then the time the response was received from the thing.
        private DateTime responseTime = DateTime.UtcNow;
        private CultureInfo cultureInfo = new CultureInfo("en-US");

        /*
         * In Windows to check the window's clock against an NTP server (change time.nist.gov to what ever NTP server you want)
         * w32tm /stripchart /computer:time.nist.gov /dataonly /samples:3
         * In Linux
         * ntpdate -q time.nist.gov
         */

        public Ping() {
            responseTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Returns a new Ping object indicating that the EST Unit has not yet responded to a Ping (Unknown is set to true)
        /// </summary>
        /// <returns></returns>
        public static Ping GetNullPing() {
            return new Ping() {
                Unknown = true
            };
        }

        /// <summary>
        /// Thing name, who initiated the ping request
        /// </summary>
        public string SentFrom { get; set; }

        /// <summary>
        /// UTC Time ping request was sent. 
        /// </summary>
        public string SentTime { get; set; }

        /// <summary>
        /// Time the remote thing received the ping request
        /// </summary>
        public string ReceivedTime { get; set; }

        /// <summary>
        /// Name of the remote thing
        /// </summary>
        public string ReceivedFrom { get; set; }

        public DateTime GetSentTime() {
            try {
                var st = DateTime.ParseExact(SentTime, ShootersTech.DataModel.Athena.DateTimeFormats.DATETIME_FORMAT, cultureInfo);
                return st.ToUniversalTime();
                //return st;
            } catch (Exception ex) {
                return DateTime.MinValue;
            }
        }

        public DateTime GetReceivedTime() {
            try {
                var rt = DateTime.ParseExact(ReceivedTime, ShootersTech.DataModel.Athena.DateTimeFormats.DATETIME_FORMAT, cultureInfo);
                return rt.ToUniversalTime();
            } catch (Exception ex) {
                return DateTime.MinValue;
            }
        }

        public TimeSpan GetRoundTripTime() {
            return responseTime - GetSentTime();
        }

        public TimeSpan GetSendTime() {
            return GetReceivedTime() - GetSentTime();
        }

        public TimeSpan GetResponseTime() {
            return responseTime - GetReceivedTime();
        }

        /// <summary>
        /// Returns a boolean, indicating if the thing's clock is properly synchroinzed.
        /// Which is determiend if the ReceivedTime is between the SentTime and responseTime.
        /// Which probable isn't a good way to determine this. Since Orion is not going to be on the same time server as the things
        /// </summary>
        /// <returns></returns>
        public bool TimeSynchronized() {
            var receivedDiff = GetReceivedTime() - GetSentTime();
            var responseDiff = responseTime - GetReceivedTime();
            return (Math.Abs(receivedDiff.TotalSeconds) < 4 && Math.Abs( responseDiff.TotalSeconds ) < 4);
        }

        /// <summary>
        /// Return a boolean indicating if the ping's received time is larger than the time inbetween pings. If it is
        /// and this is the "last" received ping from the EST Unit, then the EST Unite is likely offline.
        /// </summary>
        public bool OutOfDate {
            get {
                if (Unknown)
                    return true;

                return (DateTime.UtcNow - GetReceivedTime()).TotalSeconds > 2.0D;
            }
        }

        /// <summary>
        /// If the EST Unit has nto yet responded to a Ping, Unknown will be true.
        /// </summary>
        public bool Unknown { get; private set; }
    }
}
