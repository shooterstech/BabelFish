﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.Helpers;
using ShootersTech.DataModel.Definitions;

namespace ShootersTech.Requests.DefinitionAPI
{
    public class GetDefinitionRequest : Request
    {
        public GetDefinitionRequest(SetName? setName = null, DefinitionType definitionType = DefinitionType.ATTRIBUTE )
        {
            SetName = setName;
            DefinitionType = definitionType;
        }

        public SetName? SetName { get; set; } = null;

        public DefinitionType DefinitionType { get; set; } = DefinitionType.ATTRIBUTE;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/definition/{DefinitionType}/{SetName}"; }
        }
    }
}
