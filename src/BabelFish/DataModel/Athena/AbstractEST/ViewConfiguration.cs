using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST {
    public class ViewConfiguration {

        public const int MAX_VIEW_DEFINITIONS = 8;
        public const int MAX_MARQUEE_MESSAGES = 8;

        public ViewConfiguration() {
            ViewDefinitions = new List<ViewDefinitionConfiguration>();
            MarqueeMessages = new List<MarqueeMessage>();
        }

        public ViewConfiguration( ViewConfiguration viewConfiguration ) {
            this.ConfigName = viewConfiguration.ConfigName;
            this.ViewDefinitions = new List<ViewDefinitionConfiguration>();
            this.MarqueeMessages = new List<MarqueeMessage>();

            foreach (var vd in viewConfiguration.ViewDefinitions)
                this.ViewDefinitions.Add( new ViewDefinitionConfiguration( vd ) );

            foreach (var mm in viewConfiguration.MarqueeMessages)
                this.MarqueeMessages.Add( new MarqueeMessage( mm ) );
        }

        public string ConfigName { get; set; }

        public string Description { get; set; }

        public List<ViewDefinitionConfiguration> ViewDefinitions { get; set; } = new List<ViewDefinitionConfiguration>();

        public List<MarqueeMessage> MarqueeMessages { get; set; } = new List<MarqueeMessage>();

        public override string ToString() {
            return $"{ConfigName} ({ViewDefinitions.Count} View Definitions)";
        }

        /// <summary>
        /// Enforces the maximum length of 8 View Definitions and Marquee Messages
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (ViewDefinitions.Count > MAX_VIEW_DEFINITIONS)
                ViewDefinitions = ViewDefinitions.GetRange( 0, MAX_VIEW_DEFINITIONS );

            if (MarqueeMessages.Count > MAX_MARQUEE_MESSAGES) {
                MarqueeMessages = MarqueeMessages.GetRange( 0, MAX_MARQUEE_MESSAGES );
            }
        }
    }
}