using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {

    /// <summary>
    /// Generates a RESULT LIST FORMAT based on the inputted Squadding List containing most of the demographic fields
    /// (e.g. DisplayName, Hometown), Squadding, and all Attribute Values.
    /// </summary>
    public class DynamicSquadding : DynamicResultListFormat<SquaddingList> {

        /// <inheritdoc />
        public override async Task<ResultListFormat> GenerateAsync( SquaddingList squaddingList ) {

            ResultListFormat rlf = new ResultListFormat();
            rlf.SetName = "v1.0:dynamic:Squadding";
            rlf.ScoreFormatCollectionDef = "v1.0:orion:Standard Score Formats";
            rlf.HierarchicalName = "dynamic:Squadding";
            rlf.Version = "1.0";
            var scoreFormatCollection = await rlf.GetScoreFormatCollectionDefinitionAsync();

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Display Name",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{DisplayName}",
                        LinkTo = LinkToOption.PublicProfile,
                        ClassSet = new List<ClassSet>() {
                            new ClassSet() {
                                 Name = "rlf-col-participant"
                            }
                        }
                    }
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Competitor Number",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{CompetitorNumber}",
                        ClassSet = new List<ClassSet>() {
                            new ClassSet() {
                                 Name = "rlf-col-matchinfo"
                            }
                        }
                    }
                },
                ShowWhen = new ShowWhenVariable() {
                    Condition = ShowWhenCondition.DIMENSION_LARGE
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Club",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Club}",
                        ClassSet = new List<ClassSet>() {
                            new ClassSet() {
                                 Name = "rlf-col-matchinfo"
                            }
                        }
                    }
                },
                ShowWhen = new ShowWhenVariable() {
                    Condition = ShowWhenCondition.DIMENSION_EXTRA_LARGE
                }
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Hometown",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Hometown}",
                        ClassSet = new List<ClassSet>() {
                            new ClassSet() {
                                 Name = "rlf-col-matchinfo"
                            }
                        }
                    }
                },
                ShowWhen = new ShowWhenVariable() {
                    Condition = ShowWhenCondition.DIMENSION_EXTRA_LARGE
                }
            } );

            /*
             * Add columns for each of the Attribute Values.
             * This is a two part operation. Since AttributeValues are not standard, we will dynamically 
             * define ResultListFields for each of them. Then, add colummsn for each of them.
             * 
             * NOTE: All competitors in a Squadding List should have the same ATTRIBUTES. Will only use the first
             * competitor to learn about the list.
             */

            if (squaddingList is not null && squaddingList.Items.Count > 0) {
                var firstItem = squaddingList.Items[0];

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
                                Text = $"{{{attributeValue.AttributeDef}}}",
                                ClassSet = new List<ClassSet>() {
                                    new ClassSet() {
                                         Name = "rlf-col-matchinfo"
                                    }
                                }
                            }
                        },
                        ShowWhen = new ShowWhenVariable() {
                            Condition = ShowWhenCondition.DIMENSION_MEDIUM
                        }
                    } );
                }
            }

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Squadding",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Squadding}"
                    }
                }
            } );

            rlf.Format.Display = new ResultListDisplayPartitions() {
                Footer = new ResultListDisplayPartition() {
                    ClassSet = new List<ClassSet>() {
                        new ClassSet() {
                            Name = "rlf_row_footer"
                        }
                    }
                },
                Children = new ResultListDisplayPartition() {
                    ClassSet = new List<ClassSet>() {
                        new ClassSet() {
                            Name = "rlf_row_child"
                        }
                    }
                },
                Header = new ResultListDisplayPartition() {
                    ClassSet = new List<ClassSet>() {
                        new ClassSet() {
                            Name = "rlf_row_header"
                        }
                    }
                },
                Body = new ResultListDisplayPartition() {
                    ClassSet = new List<ClassSet>() {
                        new ClassSet() {
                            Name = "rlf_row_athlete"
                        }
                    }
                }
            };

            return rlf;
        }
    }
}
