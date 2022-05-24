using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class ActiveViewWarning
    {

        public ActiveViewWarning()
        {

        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (Warnings == null)
                Warnings = new List<string>();
        }

        public string ViewName { get; set; }

        public List<string> Warnings { get; set; }
    }
}