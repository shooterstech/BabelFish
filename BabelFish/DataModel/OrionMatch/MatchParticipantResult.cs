﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    [Obsolete("Will be replaced soon with a more appropriate match authorization list.")]
	public class MatchParticipantResult {

        public MatchParticipantResult() {
            UserID = null;
        }

        public string UserID { get; set; }

        public string EventName { get; set; }

        public string ResultCOFID { get; set; }
    }
}
