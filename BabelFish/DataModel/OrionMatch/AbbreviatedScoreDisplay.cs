﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Used to hold formatted score data to be displayed to the athlete or spectators. 
    /// 
    /// Intentially similiar to the DataModel.Definition class AbbreviatedFormat, which is used to define the
    /// structure of he data. This class (in DataModel.OrionMatch) is used to hold the formatted
    /// score data.
    /// </summary>
    public class AbbreviatedScoreDisplay {

        public AbbreviatedScoreDisplay() {
            FormatName = "";
            EventName = "";
            Children = new List<AbbreviatedScoreDisplay>();
        }

        /// <summary>
        /// Generates a new AbbreviatedScoreDisplay instance, based on the defintion of an AbbreviatedFormat.
        /// </summary>
        /// <param name="abbreviatedFormat"></param>
        public AbbreviatedScoreDisplay( AbbreviatedFormat abbreviatedFormat, ResultEvent re) {
            this.FormatName = abbreviatedFormat.FormatName;
            this.EventName = abbreviatedFormat.EventName;
            this.EventDisplayName = abbreviatedFormat.EventDisplayName;
            
            this.Children = new List<AbbreviatedScoreDisplay>();
            foreach ( var abChild in abbreviatedFormat.GetCompiledAbbreviatedFormatChildren( re ) ) {
                this.Children.Add(new AbbreviatedScoreDisplay(abChild, re));
            }
        }

        public AbbreviatedScoreDisplay( AbbreviatedFormatChild abbreviatedFormat, ResultEvent re ) {
			this.EventName = abbreviatedFormat.EventName;
			this.EventDisplayName = abbreviatedFormat.EventDisplayName;
		}

        /// <summary>
        /// A unique name given to this AbbreviatedFormat.
        /// </summary>
        [JsonPropertyOrder ( 1 )]
        [DefaultValue( "" )]
        public string FormatName { get; set; }

        /// <summary>
        /// The name of the top level event.
        /// </summary>
        [JsonPropertyOrder ( 2 )]
        [DefaultValue( "" )]
        public string EventName { get; set; }

        /// <summary>
        /// The name of the event to display to the athlete
        /// </summary>
        [DefaultValue( "" )]
        [JsonPropertyOrder ( 3 )]
        public string EventDisplayName { get; set; }

        /// <summary>
        /// A list of child events who scores should be included in the resulting AbbreviatedResultCOF.
        /// Must be List<AbbreviatedFormat> or ...
        /// </summary>
        [JsonPropertyOrder ( 4 )]
        [DefaultValue( null )]
        public List<AbbreviatedScoreDisplay> Children { get; set; }

        public bool ShouldSerializeChildren() {
            return ( Children != null && Children.Count > 0 );
        }

        /// <summary>
        /// Human Readable score format string. defaults to decimal single value.
        /// </summary>
        [JsonPropertyOrder ( 5 )]
        [DefaultValue( "0 - 0" )]
        public string ScoreFormatted { get; set; }

        /// <summary>
        /// available shot attribute list.
        /// </summary>
        [JsonPropertyOrder ( 6 )]
        public List<string> AttributeList { get; set; } = new List<string>();

        public bool ShouldSerializeAttributeList() {
            return (AttributeList != null && AttributeList.Count > 0);
        }

        public override string ToString() {
            if (FormatName != "")
                return $"{FormatName} for {EventName}";

            else
                return $"AbbreviatedFormat for {EventName}";

        }
    }
}
