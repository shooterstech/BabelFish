using System.Diagnostics;

namespace Scopos.BabelFish.Helpers {
    /// <summary>
    /// Extends the System.Diagnostics.Stopwatch class to add logging functionality. This class will log the average time taken for a certain number of operations, and can log as an error if the average time exceeds a specified threshold.
    /// </summary>
    public class Stopwatch : System.Diagnostics.Stopwatch {

        private Logger _logger = LogManager.GetCurrentClassLogger();
        private int _count = 0;

        /// <summary>
        /// Construcotr, giving the instance a name.
        /// </summary>
        /// <param name="name"></param>
        public Stopwatch( string name ) : base() {
            this.Name = name;
            this.LogHowOften = 50;
            this.LogAsErrorIfTimeExceeds = 10;
        }

        /// <summary>
        /// The name of this stopwatch instance.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating how many times the stopwatch should be stopped before logging the average time. Default is 50.
        /// </summary>
        public int LogHowOften { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the threshold in milliseconds above which the average time will be logged as an error. Default is 10.
        /// </summary>
        public int LogAsErrorIfTimeExceeds { get; set; }

        /// <summary>
        /// Stops the stopwatch.
        /// </summary>
        public new void Stop() {
            base.Stop();
            _count++;
            if (_count == LogHowOften) {
                Log();
                base.Reset();
                _count = 0;
            }
        }

        /// <summary>
        /// Logs the average time of the stopwatch.
        /// </summary>
        public void Log() {
            long avgTime = this.ElapsedMilliseconds / _count;

            var msg = $"Stopwatch {Name} is averaging {avgTime}ms.";
            Debug.Assert( avgTime > LogAsErrorIfTimeExceeds, msg );

            if (avgTime > LogAsErrorIfTimeExceeds) {
                _logger.Error( msg );
            } else {
                _logger.Info( msg );
            }
        }
    }
}


