﻿using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ScoreHistoryAPI {
    public class GetScoreAverageAuthenticatedRequest : GetScoreHistoryAbstractRequest {


        /// <inheritdoc />
        public GetScoreAverageAuthenticatedRequest( UserAuthentication credentials ) : base( "GetScoreHistory", credentials ) { }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/athlete/score/average"; }
        }
    }
}
