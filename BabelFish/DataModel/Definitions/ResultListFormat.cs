using System.ComponentModel;
using System.Runtime.Serialization;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.Specification.Definitions;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A ResultListFormat describes how to convert a ResultList into an intermediate format for
    /// displaying. The intermediat format is implementation independent, meaning it doesn't know if it
    /// will be dsiplayed on a computer monitor, mobile phone, or a drawing on the side of the wall. 
    /// 
    /// The ResultListFormat will describes in a 2D fashion the data from the ResultList to display.
    /// </summary>
    public class ResultListFormat : Definition, IGetScoreFormatCollectionDefinition {

        public ResultListFormat() : base() {
            Type = DefinitionType.RESULTLISTFORMAT;

            Fields = new List<ResultListField>();

            Format = new ResultListFormatDetail();
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod( StreamingContext context ) {
            base.OnDeserializedMethod( context );

            if (Fields == null)
                Fields = new List<ResultListField>();

            if (Format == null)
                Format = new ResultListFormatDetail();
        }

        /// <summary>
        /// String, formated as a set name. The set name of the ScoreFormatCollection to use when formatting score results. 
        /// 
        /// the default value is v1.0:orion:Standard Score Formats.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "v1.0:orion:Standard Score Formats" )]
        public string ScoreFormatCollectionDef { get; set; } = "v1.0:orion:Standard Score Formats";

        /// <summary>
        /// The name of the ScoreConfig to use if none other is specified.
        /// </summary>
        /// <remarks>The ScoreConfigName is usually specified by the Result List.</remarks>
		[G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "Integer" )]
        public string ScoreConfigDefault { get; set; } = "Integer";

        /// <summary>
        /// A list of ResultListFields that define field values that will be used in the text output of the 
        /// Result List Intermediate Fromat cell values. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 13 )]
        [G_NS.JsonProperty( Order = 13 )]
        public List<ResultListField> Fields { get; set; }

        /// <summary>
        /// Describes the intermediate format for cells of data within a Result List. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 14 )]
        [G_NS.JsonProperty( Order = 14 )]
        public ResultListFormatDetail Format { get; set; }

        /// <inheritdoc />
        public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsResultListFormatValid();

            var meetsSpecification = await validation.IsSatisfiedByAsync( this );
            SpecificationMessages = validation.Messages;

            return meetsSpecification;
        }

        /// <summary>
        /// Helper method, that returns a list of FieldNames that are 
        /// acceptable for string interpolation in Display.Columns.
        /// This list includes both the standard FieldNames (e.g. DisplayName)
        /// as well of the FieldNames defined by the user in .Fields
        /// </summary>
        /// <returns></returns>
        public List<string> GetFieldNames() {
            List<string> fields = new List<string>();
            fields.AddRange( ResultListIntermediateFormattedRow.StandardParticipantAttributeFields );

            foreach( var field in Fields )
                fields.Add( field.FieldName );

            return fields;
        }

        /// <inheritdoc />
        public override bool ConvertValues() {
            bool updateHappened = base.ConvertValues();

            //Convert from using ClassList to ClassSet
            //On Columns
            foreach( var col in Format.Columns ) {

                if (col.ClassSet is null || col.ClassSet.Count == 0) {
                    //First try and convert from the deprecated .ClassList property
                    if (col.ClassList != null) {
                        foreach (var cl in col.ClassList) {
                            var cs = new ClassSet();
                            cs.Name = cl;
                            cs.ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone();
                            col.ClassSet.Add( cs );
                            updateHappened |= true;
                        }
                    } else {
                        //Next try and infer the column and give the .ClassSet default values.
                        switch (col.Header) {
                            case "Rank":
                            case "Rk":
                                col.ClassSet.Add( new ClassSet() {
                                    Name = "rol-col-rank",
                                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                                } );
                                updateHappened |= true;
                                break;
                            case "Prone":
                            case "Standing":
                            case "Sitting":
                            case "Kneeling":
                                col.ClassSet.Add( new ClassSet() {
                                    Name = "rol-col-stage",
                                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                                } );
                                updateHappened |= true;
                                break;
                            case "Athlete":
                            case "Team":
                            case "Participant":
                                col.ClassSet.Add( new ClassSet() {
                                    Name = "rol-col-participant",
                                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                                } );
                                updateHappened |= true;
                                break;
                            case "Gap":
                            case "DFL":
                                col.ClassSet.Add( new ClassSet() {
                                    Name = "rol-col-gap",
                                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                                } );
                                updateHappened |= true;
                                break;
                            default:
                                col.ClassSet.Add( new ClassSet() {
                                    Name = "rol-col-event",
                                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                                } );
                                updateHappened |= true;
                                break;
                        }
                    }
                }

            }

            //On Rows
            updateHappened |= ConvertResultListDisplayPartition( Format.Display.Header);
            updateHappened |= ConvertResultListDisplayPartition( Format.Display.Body );
            updateHappened |= ConvertResultListDisplayPartition( Format.Display.Children );
            updateHappened |= ConvertResultListDisplayPartition( Format.Display.Footer );
            updateHappened |= ConvertResultListDisplayPartition( Format.DisplayForTeam.Header );
            updateHappened |= ConvertResultListDisplayPartition( Format.DisplayForTeam.Body );
            updateHappened |= ConvertResultListDisplayPartition( Format.DisplayForTeam.Children );
            updateHappened |= ConvertResultListDisplayPartition( Format.DisplayForTeam.Footer );

            /* EKA NOTE March 2025: These 'SetDefault' methods should be removed at some point. 
             * Adding them now as a helper convienance to convert really old Result List formats. */

            updateHappened |= SetDefaultResultListDisplayPartition( Format.Display.Header, "rlf-row-header" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.Display.Body, "rlf-row-athlete" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.Display.Children, "rlf-row-child" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.Display.Footer, "rlf-row-footer" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.DisplayForTeam.Header, "rlf-row-header" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.DisplayForTeam.Body, "rlf-row-team" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.DisplayForTeam.Children, "rlf-row-child" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.DisplayForTeam.Footer, "rlf-row-footer" );

            return updateHappened;
        }

        public override bool SetDefaultValues() {
            bool updateHappened = true;

            if (this.Fields == null)
                this.Fields = new List<ResultListField>();
            if (this.Format == null)
                this.Format = new ResultListFormatDetail();
            if (this.Format.Columns == null)
                this.Format.Columns = new List<ResultListDisplayColumn>();
            if (this.Format.Display == null)
                this.Format.Display = new ResultListDisplayPartitions();
            if (this.Format.DisplayForTeam == null)
                this.Format.DisplayForTeam = new ResultListDisplayPartitions();


            this.Fields.Add( new ResultListField() {
                FieldName = "Individual",
                Source = new FieldSource() {
                    ScoreFormat = "Events",
                    Name = "Qualification",
                    Comment = "Auto generated FieldSource. Source's Name may need to be updated."
                },
                Method = ResultFieldMethod.SCORE
            } );

            this.Fields.Add( new ResultListField() {
                FieldName = "Gap",
                Source = new FieldSource() {
                    ScoreFormat = "Gap",
                    Value = 1,
                    Name = "Qualification",
                    Comment = "Auto generated FieldSource. Source's Name may need to be updated."
                },
                Method = ResultFieldMethod.GAP
            } );

            this.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Rk",
                Body = "{Rank}",
                ClassSet = new List<ClassSet>() { new ClassSet() {
                    Name = "rlf-col-rank",
                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                }},
                ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
            } );

            this.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "",
                Body = "",
                BodyLinkTo = LinkToOption.PublicProfile,
                ClassSet = new List<ClassSet>() { new ClassSet() {
                    Name = "rlf-col-profile",
                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                }},
                ShowWhen = new ShowWhenVariable() {
                    Condition = ShowWhenCondition.ENGAGEABLE
                }
            } );

            this.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Participant",
                Body = "{DisplayName}",
                BodyLinkTo = LinkToOption.ResultCOF,
                ClassSet = new List<ClassSet>() { new ClassSet() {
                    Name = "rlf-col-participant",
                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                }},
                ShowWhen = new ShowWhenVariable() {
                    Condition = ShowWhenCondition.DIMENSION_LARGE
                }
            } );

            this.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Participant",
                Body = "{DisplayNameAbbreviated}",
                BodyLinkTo = LinkToOption.ResultCOF,
                ClassSet = new List<ClassSet>() { new ClassSet() {
                    Name = "rlf-col-participant",
                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                }},
                ShowWhen = new ShowWhenVariable() {
                    Condition = ShowWhenCondition.DIMENSION_LT_LARGE
                }
            } );

			this.Format.Columns.Add( new ResultListDisplayColumn() {
				Header = "LS",
				Body = "{LastShot}",
				ClassSet = new List<ClassSet>() { new ClassSet() {
					Name = "rlf-col-shot",
					ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
				}},
				ShowWhen = new ShowWhenEquation() {
					Boolean = ShowWhenBoolean.AND,
					Arguments = new List<ShowWhenBase>() {
						new ShowWhenVariable() {
							Condition = ShowWhenCondition.SUPPLEMENTAL
						},
						new ShowWhenVariable() {
							Condition = ShowWhenCondition.SHOT_ON_EST
						}
					}
				}
			} );

			this.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Location",
                Body = "{MatchLocation}",
                Child = "{Empty}",
                ClassSet = new List<ClassSet>() { new ClassSet() {
                    Name = "rlf-col-matchinfo",
                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                }},
                ShowWhen = new ShowWhenEquation() {
                    Boolean = ShowWhenBoolean.AND,
                    Arguments = new List<ShowWhenBase>() {
                        new ShowWhenVariable() {
                            Condition = ShowWhenCondition.MATCH_TYPE_VIRTUAL
                        },
                        new ShowWhenVariable() {
                            Condition = ShowWhenCondition.DIMENSION_LARGE
                        }
                    }
                }
            } );

            this.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Aggregate",
                Body = "{Individual}",
                ClassSet = new List<ClassSet>() { new ClassSet() {
                    Name = "rlf-col-event",
                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                }},
                ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
            } );

            this.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "DFL",
                Body = "{Gap}",
                ClassSet = new List<ClassSet>() { new ClassSet() {
                    Name = "rlf-col-gap",
                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                }},
                ShowWhen = new ShowWhenEquation() {
                    Boolean = ShowWhenBoolean.OR,
                    Arguments = new List<ShowWhenBase>() {
                        new ShowWhenVariable() {
                            Condition = ShowWhenCondition.RESULT_STATUS_INTERMEDIATE
                        },
                        new ShowWhenVariable() {
                            Condition = ShowWhenCondition.RESULT_STATUS_UNOFFICIAL
                        }
                    }
                }
            } );

			this.Format.Columns.Add( new ResultListDisplayColumn() {
				Header = "Remark",
				Body = "{Remark}",
				ClassSet = new List<ClassSet>() { new ClassSet() {
					Name = "rlf-col-matchinfo",
					ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
				}},
				ShowWhen = new ShowWhenEquation() {
					Boolean = ShowWhenBoolean.AND,
					Arguments = new List<ShowWhenBase>() {
						new ShowWhenVariable() {
							Condition = ShowWhenCondition.HAS_ANY_REMARK
						},
						new ShowWhenVariable() {
							Condition = ShowWhenCondition.DIMENSION_LARGE
						}
					}
				}
			} );

			updateHappened |= SetDefaultResultListDisplayPartition( Format.Display.Header, "rlf-row-header" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.Display.Body, "rlf-row-athlete" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.Display.Children, "rlf-row-child" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.Display.Footer, "rlf-row-footer" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.DisplayForTeam.Header, "rlf-row-header" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.DisplayForTeam.Body, "rlf-row-team" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.DisplayForTeam.Children, "rlf-row-child" );
            updateHappened |= SetDefaultResultListDisplayPartition( Format.DisplayForTeam.Footer, "rlf-row-footer" );

            return updateHappened;
        }

        private bool ConvertResultListDisplayPartition( ResultListDisplayPartition partition ) {

            bool updateHappened = false;

            if (partition.ClassSet == null)
                partition.ClassSet = new List<ClassSet>();

            if (partition.ClassSet.Count == 0) {
                if (partition.ClassList != null) {
                    foreach (var cl in partition.ClassList) {
                        partition.ClassSet.Add( new ClassSet() {
                            Name = cl,
                            ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                        } );
                        updateHappened |= true;
                    }
                }
            }

            return updateHappened;
        }

        private bool SetDefaultResultListDisplayPartition( ResultListDisplayPartition partition, string defaultCssName ) {
            bool updateHappened = false;

            if (partition.ClassSet == null)
                partition.ClassSet = new List<ClassSet>();

            if (partition.ClassSet.Count == 0) { 

                if (partition.ClassSet.Count == 0) {
                    partition.ClassSet.Add( new ClassSet() {
                        Name = defaultCssName,
                        ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                    } );
                    updateHappened |= true;
                }
            }

            return updateHappened;
        }

        /// <inheritdoc />
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync() {

            SetName scoreFormatCollectionSetName = Scopos.BabelFish.DataModel.Definitions.SetName.Parse( ScoreFormatCollectionDef );
            return await DefinitionCache.GetScoreFormatCollectionDefinitionAsync( scoreFormatCollectionSetName );

        }
    }
}
