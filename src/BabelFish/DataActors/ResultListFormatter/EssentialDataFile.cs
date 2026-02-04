using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {
    public class EssentialDataFile : DynamicResultListFormat<ResultList> {

        public override async Task<ResultListFormat> GenerateAsync( ResultList resultList ) {

            ResultListFormat rlf = new ResultListFormat();

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
                Header = "Competitor Number",
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{CompetitorNumber}"
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

            if (resultList is not null && resultList.Items.Count > 0) {
                var firstItem = resultList.Items[0];

                foreach (var attributeValue in firstItem.Participant.AttributeValues) {

                    var field = new ResultListField();
                    field.FieldName = attributeValue.AttributeDef;
                    field.Method = ResultFieldMethod.ATTRIBUTE;
                    field.Source = new FieldSource() {
                        Name = attributeValue.AttributeDef
                    };
                    rlf.Fields.Add( field );

                    var attribute = await attributeValue.GetAttributeDefinitionAsync();

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

            return rlf;
        }
    }
}
