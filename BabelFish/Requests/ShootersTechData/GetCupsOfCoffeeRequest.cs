﻿using Scopos.BabelFish.Requests.OrionMatchAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ShootersTechData {
    public class GetCupsOfCoffeeRequest : Request {


        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/coffee"; }
        }
    }
}
