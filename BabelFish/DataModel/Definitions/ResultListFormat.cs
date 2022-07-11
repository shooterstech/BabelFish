using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Definitions {
    public class ResultListFormat {

        public enum LinkToOption {
            /*
             * In order to get an Enum to serialize / deserialize to values with spaces have to do a couple of things.
             * First use the EnumMember(Value = "   ") attribute. This is what does the serialzing / deserialzing.
             * In order to get the Descriptions in code, have to use the Description attribute in conjunction with the
             * ExtensionMethod .Description() (Located in the ExtensionMethods.cs class).
            */
            /// <summary>
            /// Indicates that the Cell should link to the ResultCOF Page (sometimes called Individual Score Page). 
            /// </summary>
            [Description( "ResultCOF" )]
            [EnumMember( Value = "ResultCOF" )]
            ResultCOF
        }
        //TODO Define the ResultListFormat definition.
    }
}
