using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// An AttributeFilterCalculator has a series of Passes() methods that test if the
    /// passed in Participant satisfies the conditions specified in the passed in AttributeFilter.
    /// </summary>
    public static class AttributeFilterCalculator {

        /// <summary>
        /// Tests if the passed in Participant satisfies the conditions specified in the passed in AttributeFilter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public static bool Passes( AttributeFilter filter, Participant participant ) {

            if (filter is AttributeFilterAttributeValue)
                return AttributeFilterCalculator.Passes( (AttributeFilterAttributeValue)filter, participant );

            //else filter is AttributeFilterEquation
            return AttributeFilterCalculator.Passes( (AttributeFilterEquation)filter, participant );
        }

        /// <summary>
        /// Tests if the passed in Participant satisfies the conditions specified in the passed in AttributeFilter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public static bool Passes( AttributeFilter filter, IParticipant participant ) {
            return AttributeFilterCalculator.Passes( filter, participant.Participant );
        }

        /// <summary>
        /// Tests if the passed in Participant satisfies the conditions specified in the passed in AttributeFilter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public static bool Passes( AttributeFilterEquation filter, Participant participant ) {

            bool answer = true;
            bool first = true;
            bool apployNot = false;
            bool breakForeach = false;

            foreach (var argument in filter.Arguments) {
                if (first) {
                    answer = AttributeFilterCalculator.Passes( argument, participant );
                    first = false;
                } else {
                    switch (filter.Boolean) {
                        case ShowWhenBoolean.AND:
                            answer &= AttributeFilterCalculator.Passes( argument, participant );
                            //If the answer is already false, we can stop evaluating
                            if (!answer)
                                breakForeach = true;
                            break;
                        case ShowWhenBoolean.OR:
                            answer |= AttributeFilterCalculator.Passes( argument, participant );
                            //If the answer is already true, we can stop evaluating
                            if (answer)
                                breakForeach = true;
                            break;
                        case ShowWhenBoolean.XOR:
                            answer ^= AttributeFilterCalculator.Passes( argument, participant );
                            break;
                        case ShowWhenBoolean.NAND:
                            answer &= AttributeFilterCalculator.Passes( argument, participant );
                            apployNot = true;
                            break;
                        case ShowWhenBoolean.NOR:
                            answer |= AttributeFilterCalculator.Passes( argument, participant );
                            apployNot = true;
                            break;
                        case ShowWhenBoolean.NXOR:
                            answer ^= AttributeFilterCalculator.Passes( argument, participant );
                            apployNot = true;
                            break;
                    }
                }

                if (breakForeach)
                    break;
            }

            return answer ^ apployNot;

        }

        /// <summary>
        /// Tests if the passed in Participant satisfies the conditions specified in the passed in AttributeFilter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public static bool Passes( AttributeFilterEquation filter, IParticipant participant ) {
            return AttributeFilterCalculator.Passes( filter, participant.Participant );
        }

        /// <summary>
        /// Tests if the passed in Participant satisfies the conditions specified in the passed in AttributeFilter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public static bool Passes( AttributeFilterAttributeValue filter, Participant participant ) {

            switch (filter.FilterRule) {

                case AttributeFilterRule.HAVE_ONE:
                    foreach (var expectedValue in filter.Values) {
                        if (HasValue( expectedValue, participant )) {
                            return true;
                        }
                    }

                    return false;

                case AttributeFilterRule.HAVE_ALL:

                    bool foundValue = true;
                    foreach (var expectedValue in filter.Values) {
                        foundValue &= HasValue( expectedValue, participant );
                    }
                    return foundValue;

                case AttributeFilterRule.NOT_HAVE_ANY:

                    foreach (var expectedValue in filter.Values) {
                        if (HasValue( expectedValue, participant )) {
                            return false;
                        }
                    }

                    return true;
            }

            //We shouldn't ever get here, but the compiler thinks we might. 
            return false;
        }

        /// <summary>
        /// Returns true if the passed in Participant has an AttributeValue that's equal to the value specified in the passed in AttributeValueDataPacketMatch filter, and false otherwise.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        private static bool HasValue( AttributeValueDataPacketMatch filter, Participant participant ) {
            AttributeValue? attrValue = null;
            foreach (var attrValueToInspect in participant.AttributeValues) {
                if (attrValueToInspect.AttributeDef.Equals( filter.AttributeDef )) {
                    attrValue = attrValueToInspect.AttributeValue;
                    break;
                }
            }

            if (attrValue == null) return false;

            return attrValue.Equals( filter.AttributeValue );
        }
    }
}
