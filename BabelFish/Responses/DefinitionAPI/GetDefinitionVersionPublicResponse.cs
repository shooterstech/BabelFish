﻿using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.DefinitionAPI {
    public class GetDefinitionVersionPublicResponse : Response<SparseDefinition> {

        public GetDefinitionVersionPublicResponse( GetDefinitionVersionPublicRequest request ) {
            this.Request = request;

            this.SetName = request.SetName;
            this.DefinitionType = request.DefinitionType;
        }

        public SetName SetName { get; private set; }

        public DefinitionType DefinitionType { get; private set; }

        public SparseDefinition SparseDefinition {
            get {
                return Value;
            }
        }

        /// <inheritdoc/>
        protected override void ConvertBodyToValue() {
            Value = Body[SetName.ToString()].ToObject<SparseDefinition>();
        }
    }
}