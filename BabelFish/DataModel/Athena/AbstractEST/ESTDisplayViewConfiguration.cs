using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.AbstractEST {
    public class ESTDisplayViewConfiguration {

        public ESTDisplayViewConfiguration() {
            this.ViewConfigurations = new List<ViewConfiguration>();
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context) {
            //Providate default values if they were not read during deserialization

            if (ViewConfigurations == null)
                ViewConfigurations = new List<ViewConfiguration>();
        }

        public List<ViewConfiguration> ViewConfigurations { get; set; }

        /// <summary>
        /// Removes all ViewConfigurations from teh list ViewConfigurations
        /// </summary>
        public void Clear() {
            ViewConfigurations.Clear();
        }

        /// <summary>
        /// Adds the passed in ViewConfiguration only if one with the same 
        /// Config names does not already exist in the ViewConfigurations list
        /// </summary>
        /// <param name="vc"></param>
        /// <returns>True if the passed in value was added, false otherwise.</returns>
        public bool AddViewConfiguration(ViewConfiguration vc) {
            bool exists = false;
            foreach (var existingViewConfig in ViewConfigurations) {
                if (existingViewConfig.ConfigName == vc.ConfigName) {
                    exists = true;
                    break;
                }
            }

            if (!exists)
                ViewConfigurations.Add(vc);

            return !exists;
        }
    }
}
