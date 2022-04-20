using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.AbstractEST {
    public class ESTDisplayViewDefinition {
        public ESTDisplayViewDefinition() {
            ViewDefinitions = new List<ViewDefinition>();
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context) {
            //Providate default values if they were not read during deserialization

            if (ViewDefinitions == null)
                ViewDefinitions = new List<ViewDefinition>();
        }

        public List<ViewDefinition> ViewDefinitions { get; set; }

        /// <summary>
        /// Removes all ViewDefinitions from the list ViewDefinitions
        /// </summary>
        public void Clear() {
            ViewDefinitions.Clear();
        }

        public bool AddViewDefinition(ViewDefinition vd) {
            bool exists = false;
            foreach (var existingViewDefinition in ViewDefinitions) {
                if (existingViewDefinition.ViewName == vd.ViewName) {
                    exists = true;
                    break;
                }
            }

            if (!exists)
                ViewDefinitions.Add(vd);

            return !exists;
        }

    }
}
