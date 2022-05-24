using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class ActiveViewValues
    {

        public ActiveViewValues()
        {
            ViewDefinitions = new List<string>();
            Warnings = new List<ActiveViewWarning>();
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (Warnings == null)
                Warnings = new List<ActiveViewWarning>();
        }

        public string DisplayEvent { get; set; }

        public string ViewConfiguration { get; set; }

        public List<string> ViewDefinitions { get; set; }

        public string ImageKey { get; set; }

        public string ViewDefinition { get; set; }

        public List<ActiveViewWarning> Warnings { get; set; }
    }
}