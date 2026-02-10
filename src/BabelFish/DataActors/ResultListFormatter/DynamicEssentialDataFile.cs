using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {

    /// <summary>
    /// Generates a RESULT LIST FORMAT based on the inputted ResultList containing all the demographic fields
    /// (e.g. FamilyName, GivenName, Hometown) and Attribute Values.
    /// </summary>
    public class DynamicEssentialDataFile : DynamicResultListFormat<IRLIFList> {

        /// <inheritdoc />
        public override async Task<ResultListFormat> GenerateAsync( IRLIFList irlitList ) {

            ResultListFormat rlf = new ResultListFormat();
            rlf.SetName = "v1.0:dynamic:Essential Data File";
            rlf.ScoreFormatCollectionDef = "v1.0:orion:Minimal Score Formats";
            rlf.HierarchicalName = "dynamic:Essential Data File";
            rlf.Version = "1.0";
            var scoreFormatCollection = await rlf.GetScoreFormatCollectionDefinitionAsync();

            var resultList = (irlitList is ResultList) ? (ResultList)irlitList : null;
            var includeIndividual = (irlitList is SquaddingList) || (resultList is not null && !resultList.Team);
            var includeTeam = (resultList is not null && resultList.Team);
            var includeScores = (resultList is not null);
            var includeIfNotOfficial = (resultList is not null && resultList.Status != ResultStatus.OFFICIAL);

            #region Demographic Columns
            /*
             * Add columns for each of the standard demographic fields
             */

            if (includeIndividual) {
                rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                    Header = "Family Name",
                    BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{FamilyName}"
                    }
                }
                } );

                rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                    Header = "Given Name",
                    BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{GivenName}"
                    }
                }
                } );

                rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                    Header = "Middle Name",
                    BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{MiddleName}"
                    }
                }
                } );

                rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                    Header = "Competitor Number",
                    BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{CompetitorNumber}"
                    }
                }
                } );
            }

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Display Name",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{DisplayName}"
                    }
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Display Name Short",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{DisplayNameShort}"
                    }
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Hometown",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Hometown}"
                    }
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Country",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Country}"
                    }
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Club",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Club}"
                    }
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Match Location",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{MatchLocation}"
                    }
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Result COF ID",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{ResultCOFID}"
                    }
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Status",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Status}"
                    }
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Remarks",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Remark}"
                    }
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Target Collection",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{TargetCollectionName}"
                    }
                }
            } );

            if (includeIndividual && includeIfNotOfficial) {
                rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                    Header = "Squadding",
                    BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Squadding}"
                    }
                }
                } );

                rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                    Header = "Relay",
                    BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Relay}"
                    }
                }
                } );

                rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                    Header = "Firing Point",
                    BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{FiringPoint}"
                    }
                }
                } );
            }

            #endregion

            /*
             * Add columns for each of the Attribute Values.
             * This is a two part operation. Since AttributeValues are not standard, we will dynamically 
             * define ResultListFields for each of them. Then, add colummsn for each of them.
             * 
             * NOTE: All competitors in a Result List should have the same ATTRIBUTES. Will only use the first
             * competitor to learn about the list.
             */

            if (resultList is not null && resultList.Items.Count > 0) {
                var firstItem = resultList.Items[0];

                foreach (var attributeValue in firstItem.Participant.AttributeValues) {

                    // Define the ResultListField with the ATTRIBUTE method.
                    var field = new ResultListField();
                    field.FieldName = attributeValue.AttributeDef;
                    field.Method = ResultFieldMethod.ATTRIBUTE;
                    field.Source = new FieldSource() {
                        Name = attributeValue.AttributeDef
                    };
                    rlf.Fields.Add( field );

                    //Learn about the ATTRIBUTE
                    var attribute = await attributeValue.GetAttributeDefinitionAsync();

                    //Add columns for each of the ATTRIBUTE Values.
                    rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                        Header = attribute.CommonName,
                        BodyValues = new List<ResultListCellValue>() {
                            new ResultListCellValue() {
                                Text = $"{{{attributeValue.AttributeDef}}}"
                            }
                        }
                    } );
                }
            }

            if (includeScores && irlitList is ResultList rl && SetName.TryParse( resultList.CourseOfFireDef, out SetName courseOfFireSetName )) {
                var courseOfFire = await DefinitionCache.GetCourseOfFireDefinitionAsync( courseOfFireSetName );
                var scoreformat = await courseOfFire.GetScoreFormatCollectionDefinitionAsync();
                var topLevelEvent = EventComposite.GrowEventTree( courseOfFire );
                var listOfEvents = topLevelEvent.GetEvents( true, true, true, true, true, false, true );
                var scoreFormat = scoreFormatCollection.ScoreFormats[0]; //There should be exactly one ScoreFormat defined in v1.0:orion:Minimal Score Formats

                foreach (var @event in listOfEvents) {
                    foreach (var scoreConfig in scoreFormatCollection.ScoreConfigs) {
                        var field = new ResultListField();
                        field.FieldName = $"{@event.EventName} - {scoreConfig.ScoreConfigName}";
                        field.Method = ResultFieldMethod.SCORE;
                        field.Source = new FieldSource() {
                            Name = @event.EventName,
                            ScoreFormat = scoreFormat,
                            ScoreConfigName = scoreConfig.ScoreConfigName
                        };
                        rlf.Fields.Add( field );

                        rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                            Header = field.FieldName,
                            BodyValues = new List<ResultListCellValue>() {
                            new ResultListCellValue() {
                                Text = $"{{{field.FieldName}}}"
                            }
                        }
                        } );
                    }
                }


            }

            return rlf;
        }
    }
}
