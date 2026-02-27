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

            AttributeValue? attrValue = null;
            foreach (var attrValueToInspect in participant.AttributeValues) {
                if (attrValueToInspect.AttributeDef.Equals( filter.AttributeDef )) {
                    attrValue = attrValueToInspect.AttributeValue;
                    break;
                }
            }

            switch (filter.FilterRule) {

                case AttributeFilterRule.HAVE_ONE:
                    // To pass, the Participant must have one of the values listed in
                    // the AttributeFilterAttributeValue's .Values array.
                    if (attrValue == null) return false;

                    foreach (var expectedValue in filter.Values) {
                        if (HasValue( attrValue, expectedValue )) {
                            return true;
                        }
                    }

                    return false;

                case AttributeFilterRule.HAVE_ALL:
                    // To pass, the Participant must have all of the values listed in
                    // the AttributeFilterAttributeValue's .Values array, but may have additional values.
                    if (attrValue == null) return false;

                    bool foundValue = true;
                    foreach (var expectedValue in filter.Values) {
                        foundValue &= HasValue( attrValue, expectedValue );
                    }
                    return foundValue;

                case AttributeFilterRule.NOT_HAVE_ANY:
                    // To pass, the Participant must not have any of the values listed in
                    // the AttributeFilterAttributeValue's .Values array.
                    if (attrValue == null) return true;

                    foreach (var expectedValue in filter.Values) {
                        if (HasValue( attrValue, expectedValue )) {
                            return false;
                        }
                    }

                    return true;
            }

            //We shouldn't ever get here, but the compiler thinks we might. 
            return false;
        }

        /// <summary>
        /// Tests if the passed in Participant satisfies the conditions specified in the passed in AttributeFilter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public static bool Passes( AttributeFilterAttributeValue filter, IParticipant participant ) {
            return AttributeFilterCalculator.Passes( filter, participant.Participant );
        }

        /// <summary>
        /// Helper method that tests the passed in AttributeValue with the passed in expected value
        /// </summary>
        /// <param name="attrValue"></param>
        /// <param name="expectedAttrValue">Item1 is the FieldName, Item2 is the expected fieldValue</param>
        /// <returns></returns>
        private static bool HasValue( AttributeValue attrValue, ConstantFieldValue expectedAttrValue ) {
            var foundFieldValue = attrValue.GetFieldValue( expectedAttrValue.FieldName );
            return foundFieldValue == expectedAttrValue.Value;
        }
    }
}
