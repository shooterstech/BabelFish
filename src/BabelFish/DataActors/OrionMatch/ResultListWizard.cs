using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class ResultListWizard {

        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public ResultListWizard( Match match ) {
            Match = match;
        }

        public Match Match { get; private set; }

        public async Task<List<ResultListAbbr>> GenerateAsync( int courseOfFireId ) {
            CourseOfFireStructure? cofStructure = this.Match.MatchStructure.CoursesOfFire.Find( x => x.CourseOfFireId == courseOfFireId );

            if (cofStructure is null)
                throw new ArgumentOutOfRangeException( nameof( courseOfFireId ), $"No MatchStructure found with CourseOfFireId {courseOfFireId}." );

            var resultLists = new List<ResultListAbbr>();
            var courseOfFire = await cofStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( courseOfFire );
            var stageEvents = topLevelEvent.GetEvents( EventtType.STAGE );

            //Bin the the global Attributes in this match, and the Attributes in the EventStructure by the GroupByPriority
            //There are four possible levels of grouping attributes. Level 0 is implied to be (and can only be) Individual and Teams.
            var attributeBins = new Dictionary<int, List<AttributeConfiguration>>() {
                { 1, new List<AttributeConfiguration>() },
                { 2, new List<AttributeConfiguration>() },
                { 3, new List<AttributeConfiguration>() }
            };

            //Add in the globally shared attributes
            foreach (var attributeConfig in this.Match.MatchStructure.SharedAttributes) {
                var attribute = await attributeConfig.GetAttributeDefinitionAsync();
                attributeBins[attribute.GroupByPriority].Add( attributeConfig );
            }

            //Add in the attributes specific to this Course of Fire Structure.
            foreach (var attributeConfig in cofStructure.Attributes) {
                var attribute = await attributeConfig.GetAttributeDefinitionAsync();
                attributeBins[attribute.GroupByPriority].Add( attributeConfig );
            }

            // If any bin does not have a AttributeConfiguration, add in the default. By doing this, none of the
            // Forloops below will get skipped.
            var defaultAttributeValue = await AttributeValue.CreateAsync( SetName.DEFAULT );
            if (attributeBins[1].Count == 0)
                attributeBins[1].Add( await AttributeConfiguration.FactoryAsync( defaultAttributeValue ) );
            if (attributeBins[2].Count == 0)
                attributeBins[2].Add( await AttributeConfiguration.FactoryAsync( defaultAttributeValue ) );
            if (attributeBins[3].Count == 0)
                attributeBins[3].Add( await AttributeConfiguration.FactoryAsync( defaultAttributeValue ) );

            List<Tuple<string, bool>> teamEvents = new List<Tuple<string, bool>>() {
                new Tuple<string, bool>( "Individual", false),
                new Tuple<string, bool>( "Team", true )
            };

            foreach (var team in teamEvents) {
                resultLists.Add( new ResultListAbbr() {
                    EventName = topLevelEvent.EventName,
                    ResultName = $"{team.Item1} - All",
                    Team = team.Item2,
                    RankingRuleDef = topLevelEvent.RankingRuleMapping.GetRankingRuleDef( cofStructure.ScoreConfigName ),
                    ResultListFormatDef = topLevelEvent.ResultListFormatDef,
                    ScoreConfigName = cofStructure.ScoreConfigName,
                    CourseOfFireId = courseOfFireId
                } );

                //Level 1
                foreach (var attrConfig1 in attributeBins[1]) {
                    var attribute1 = await attrConfig1.GetAttributeDefinitionAsync();

                    //NOTE: attribute1 must be a SimpleStringAttribute (which is checked for in the AttributeConfiguration constructor), so it is safe to case it as a AttributeFieldString.
                    foreach (var value1 in ((AttributeFieldString)attribute1.Fields[0]).Values) {
                        var attrValue1 = await AttributeValue.CreateAsync( attribute1.SetName );
                        attrValue1.SetFieldValue( value1.Value );

                        if (!attribute1.SetName.IsDefault) {
                            resultLists.Add( new ResultListAbbr() {
                                EventName = topLevelEvent.EventName,
                                ResultName = $"{team.Item1} - {value1.Value}",
                                Team = team.Item2,
                                RankingRuleDef = topLevelEvent.RankingRuleMapping.GetRankingRuleDef( cofStructure.ScoreConfigName ),
                                ResultListFormatDef = topLevelEvent.ResultListFormatDef,
                                ScoreConfigName = cofStructure.ScoreConfigName,
                                CourseOfFireId = courseOfFireId,
                                AttributeFilters = new List<AttributeFilter>() {
                                 await AttributeFilterAttributeValue.FactoryAsync( attrValue1 )
                            }
                            } );
                        }

                        //Level 2
                        foreach (var attrConfig2 in attributeBins[2]) {
                            var attribute2 = await attrConfig2.GetAttributeDefinitionAsync();

                            //NOTE: attribute2 must be a SimpleStringAttribute (which is checked for in the AttributeConfiguration constructor), so it is safe to case it as a AttributeFieldString.
                            foreach (var value2 in ((AttributeFieldString)attribute2.Fields[0]).Values) {
                                var attrValue2 = await AttributeValue.CreateAsync( attribute2.SetName );
                                attrValue2.SetFieldValue( value2.Value );


                                if (!attribute2.SetName.IsDefault) {
                                    resultLists.Add( new ResultListAbbr() {
                                        EventName = topLevelEvent.EventName,
                                        ResultName = $"{team.Item1} - {value2.Value}",
                                        Team = team.Item2,
                                        RankingRuleDef = topLevelEvent.RankingRuleMapping.GetRankingRuleDef( cofStructure.ScoreConfigName ),
                                        ResultListFormatDef = topLevelEvent.ResultListFormatDef,
                                        ScoreConfigName = cofStructure.ScoreConfigName,
                                        CourseOfFireId = courseOfFireId,
                                        AttributeFilters = new List<AttributeFilter>() {
                                            new AttributeFilterEquation() {
                                                Boolean = ShowWhenBoolean.AND,
                                                Arguments = new List<AttributeFilter>() {
                                                    await AttributeFilterAttributeValue.FactoryAsync( attrValue2 )
                                                }
                                            }
                                        }
                                    } );
                                }

                                if (!attribute1.SetName.IsDefault && !attribute2.SetName.IsDefault) {
                                    resultLists.Add( new ResultListAbbr() {
                                        EventName = topLevelEvent.EventName,
                                        ResultName = $"{team.Item1} - {value1.Value} - {value2.Value}",
                                        Team = team.Item2,
                                        RankingRuleDef = topLevelEvent.RankingRuleMapping.GetRankingRuleDef( cofStructure.ScoreConfigName ),
                                        ResultListFormatDef = topLevelEvent.ResultListFormatDef,
                                        ScoreConfigName = cofStructure.ScoreConfigName,
                                        CourseOfFireId = courseOfFireId,
                                        AttributeFilters = new List<AttributeFilter>() {
                                            new AttributeFilterEquation() {
                                                Boolean = ShowWhenBoolean.AND,
                                                Arguments = new List<AttributeFilter>() {
                                                    await AttributeFilterAttributeValue.FactoryAsync( attrValue1 ),
                                                    await AttributeFilterAttributeValue.FactoryAsync( attrValue2 )
                                                }
                                            }
                                        }
                                    } );
                                }

                                //Level 3
                                foreach (var attrConfig3 in attributeBins[3]) {
                                    var attribute3 = await attrConfig3.GetAttributeDefinitionAsync();

                                    //NOTE: attribute3 must be a SimpleStringAttribute (which is checked for in the AttributeConfiguration constructor), so it is safe to case it as a AttributeFieldString.
                                    foreach (var value3 in ((AttributeFieldString)attribute3.Fields[0]).Values) {
                                        var attrValue3 = await AttributeValue.CreateAsync( attribute3.SetName );
                                        attrValue3.SetFieldValue( value3.Value );

                                        if (!attribute3.SetName.IsDefault) {
                                            resultLists.Add( new ResultListAbbr() {
                                                EventName = topLevelEvent.EventName,
                                                ResultName = $"{team.Item1} - {value3.Value}",
                                                Team = team.Item2,
                                                RankingRuleDef = topLevelEvent.RankingRuleMapping.GetRankingRuleDef( cofStructure.ScoreConfigName ),
                                                ResultListFormatDef = topLevelEvent.ResultListFormatDef,
                                                ScoreConfigName = cofStructure.ScoreConfigName,
                                                CourseOfFireId = courseOfFireId,
                                                AttributeFilters = new List<AttributeFilter>() {
                                                    new AttributeFilterEquation() {
                                                        Boolean = ShowWhenBoolean.AND,
                                                        Arguments = new List<AttributeFilter>() {
                                                            await AttributeFilterAttributeValue.FactoryAsync( attrValue3 )
                                                        }
                                                    }
                                                }
                                            } );

                                            if (!attribute2.SetName.IsDefault) {
                                                resultLists.Add( new ResultListAbbr() {
                                                    EventName = topLevelEvent.EventName,
                                                    ResultName = $"{team.Item1} - {value2.Value} - {value3.Value}",
                                                    Team = team.Item2,
                                                    RankingRuleDef = topLevelEvent.RankingRuleMapping.GetRankingRuleDef( cofStructure.ScoreConfigName ),
                                                    ResultListFormatDef = topLevelEvent.ResultListFormatDef,
                                                    ScoreConfigName = cofStructure.ScoreConfigName,
                                                    CourseOfFireId = courseOfFireId,
                                                    AttributeFilters = new List<AttributeFilter>() {
                                                        new AttributeFilterEquation() {
                                                            Boolean = ShowWhenBoolean.AND,
                                                            Arguments = new List<AttributeFilter>() {
                                                                await AttributeFilterAttributeValue.FactoryAsync(attrValue2),
                                                                await AttributeFilterAttributeValue.FactoryAsync(attrValue3)
                                                            }
                                                        }
                                                    }
                                                } );
                                            }

                                            if (!attribute1.SetName.IsDefault) {
                                                resultLists.Add( new ResultListAbbr() {
                                                    EventName = topLevelEvent.EventName,
                                                    ResultName = $"{team.Item1} - {value1.Value} - {value3.Value}",
                                                    Team = team.Item2,
                                                    RankingRuleDef = topLevelEvent.RankingRuleMapping.GetRankingRuleDef( cofStructure.ScoreConfigName ),
                                                    ResultListFormatDef = topLevelEvent.ResultListFormatDef,
                                                    ScoreConfigName = cofStructure.ScoreConfigName,
                                                    CourseOfFireId = courseOfFireId,
                                                    AttributeFilters = new List<AttributeFilter>() {
                                                        new AttributeFilterEquation() {
                                                            Boolean = ShowWhenBoolean.AND,
                                                            Arguments = new List<AttributeFilter>() {
                                                                await AttributeFilterAttributeValue.FactoryAsync(attrValue1),
                                                                await AttributeFilterAttributeValue.FactoryAsync(attrValue3)
                                                            }
                                                        }
                                                    }
                                                } );
                                            }


                                            if (!attribute1.SetName.IsDefault && !attribute2.SetName.IsDefault) {
                                                resultLists.Add( new ResultListAbbr() {
                                                    EventName = topLevelEvent.EventName,
                                                    ResultName = $"{team.Item1} - {value1.Value} - {value2.Value} - {value3.Value}",
                                                    Team = team.Item2,
                                                    RankingRuleDef = topLevelEvent.RankingRuleMapping.GetRankingRuleDef( cofStructure.ScoreConfigName ),
                                                    ResultListFormatDef = topLevelEvent.ResultListFormatDef,
                                                    ScoreConfigName = cofStructure.ScoreConfigName,
                                                    CourseOfFireId = courseOfFireId,
                                                    AttributeFilters = new List<AttributeFilter>() {
                                                        new AttributeFilterEquation() {
                                                            Boolean = ShowWhenBoolean.AND,
                                                            Arguments = new List<AttributeFilter>() {
                                                                await AttributeFilterAttributeValue.FactoryAsync(attrValue1),
                                                                await AttributeFilterAttributeValue.FactoryAsync(attrValue2),
                                                                await AttributeFilterAttributeValue.FactoryAsync(attrValue3)
                                                            }
                                                        }
                                                    }
                                                } );
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }

            return resultLists;
        }
    }
}
