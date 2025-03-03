using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.DataModel.Definitions {
    public class ClassSet {

        /*
         * TODO: Liam
         * Impelment as part of the DataModel/Definitions/ ResultListDisplayPartition and ResultListDisplayColumn
         * For bonus points, when you go to implement ClassSet, if the current value of ClassSet is null or empty,
         * but ClassList has a list of classes, convert the list of (css classes) string in ClassList to a list 
         * of ClassSet.
         * 
           "Children": { 
                "RowLinkTo": [],
                "ClassList": [
                    "rlf-row-child"
                ],
                "ClassSet" : [
                    //List of this class.
                    {
                        "Name" : "rlf-row-child",
                        "ShowWhen" : {
                            "Condition": "TRUE",
                            "Operation": "VARIABLE"
                        }
                    },
                    {
                        "Name" : "rlf-row-dns",
                        "ShowWhen" : {
                            "Condition": "HAS_REMARK_DNS",
                            "Operation": "VARIABLE"
                        }
                    }
                ],
                "RowClass": [] //Deprecate this list of strings
            },
        */
    }
}
