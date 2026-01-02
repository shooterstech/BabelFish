using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class CompareResultStatus : IComparer<ResultStatus> {

        public static CompareResultStatus COMPARER = new CompareResultStatus();

        public int Compare( ResultStatus x, ResultStatus y ) {

            switch (x) {
                case ResultStatus.FUTURE:
                    switch (y) {
                        case ResultStatus.FUTURE:
                        default:
                            return 0;
                        case ResultStatus.INTERMEDIATE:
                        case ResultStatus.UNOFFICIAL:
                        case ResultStatus.OFFICIAL:
                            return -1;
                    }
                case ResultStatus.INTERMEDIATE:
                    switch (y) {
                        case ResultStatus.FUTURE:
                            return 1;
                        case ResultStatus.INTERMEDIATE:
                        default:
                            return 0;
                        case ResultStatus.UNOFFICIAL:
                        case ResultStatus.OFFICIAL:
                            return -1;
                    }
                case ResultStatus.UNOFFICIAL:
                    switch (y) {
                        case ResultStatus.FUTURE:
                        case ResultStatus.INTERMEDIATE:
                            return 1;
                        case ResultStatus.UNOFFICIAL:
                        default:
                            return 0;
                        case ResultStatus.OFFICIAL:
                            return -1;
                    }
                case ResultStatus.OFFICIAL:
                    switch (y) {
                        case ResultStatus.FUTURE:
                        case ResultStatus.INTERMEDIATE:
                        case ResultStatus.UNOFFICIAL:
                            return 1;
                        case ResultStatus.OFFICIAL:
                        default:
                            return 0;
                    }
                default: 
                    return 0;
            }
        }
    }
}
