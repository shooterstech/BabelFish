﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetSquaddingListPublicResponse : GetSquaddingListAbstractResponse {

        public GetSquaddingListPublicResponse( GetSquaddingListPublicRequest request ) : base() {
            this.Request = request;
        }
    }
}
