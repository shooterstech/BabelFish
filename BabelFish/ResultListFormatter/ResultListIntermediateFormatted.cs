﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.ResultListFormatter.UserProfile;

namespace Scopos.BabelFish.ResultListFormatter {

    /// <summary>
    /// ResultListFormatted is an intermediate formatted result list. It's like assembly code, half way 
    /// between a high livel language and binary code running on the CPU. In this case, it reads in the 
    /// raw result list data, and the result list format definition, to produce the ResultListIntermediateFormatted
    /// class.
    /// </summary>
    public class ResultListIntermediateFormatted {

        private readonly List<ResultListIntermediateFormattedRow> rows = new();
        private bool initialized = false;

        private DefinitionAPIClient DefinitionApiClient;

        /// <summary>
        /// Converts a list of ResultEvents, that is returned by the GetResultList API, to
        /// a list of ResultEventIntermediates. 
        /// </summary>
        /// <param name="resultEventList"></param>
        /// <param name="resultListFormat"></param>
        /// <returns></returns>
        public ResultListIntermediateFormatted( 
            ResultList resultList, 
            ResultListFormat resultListFormat, 
            DefinitionAPIClient definitionApiClient, 
            IUserProfileLookup userProfileLookup ) {

            this.ResultList = resultList;
            this.ResultListFormat = resultListFormat;
            this.DefinitionApiClient = definitionApiClient;
            this.UserProfileLookup = userProfileLookup;
        }

        /// <summary>
        /// Completes the constructor process, makes necessary Async calls that can not be done within the Constructor.
        /// </summary>
        /// <param name="definitionApiClient"></param>
        /// <returns></returns>
        public async Task InitializeAsync( ) {

            //NOTE: Don't need to wait for the profile visibility initialization. 
            this.UserProfileLookup.RefreshUserProfileVisibilityAsync();

            var response = await DefinitionApiClient.GetScoreFormatCollectionDefinition( ScoreFormatCollectionSetName );
            ScoreFormatCollection = response.Definition;

            foreach (var sConfig in ScoreFormatCollection.ScoreConfigs) {
                if (sConfig.ScoreConfigName == ScoreConfigName) {
                    this.ScoreConfig = sConfig;
                    break;
                }
            }

            foreach (var re in ResultList.Items) {
                var rei = new ResultListIntermediateFormattedBodyRow( this, re );

                rows.Add( rei );

                if (re.TeamMembers != null) {
                    foreach (var child in re.TeamMembers) {
                        var reiChild = new ResultListIntermediateFormattedChildRow( this, child );

                        rows.Add( reiChild );
                    }
                }

            }

            initialized = true;
        }

        /// <summary>
        /// When a Result List is tokenized (returned from the rest API in chuncks of 50), its common that ResultListIntermediateFormatted
        /// is initialize with only the first chunck of results. This function allows the user to append additional tokenized ResultList
        /// to this instance. It assumes the user is passing in the same ResultList, just with a different tokenized chunk of result list items.
        /// </summary>
        /// <param name="tokenizedResultList"></param>
        public void AppendTokenizedResultList( ResultList tokenizedResultList ) {

            foreach (var re in tokenizedResultList.Items) {
                var rei = new ResultListIntermediateFormattedBodyRow( this, re );

                rows.Add( rei );

                if (re.TeamMembers != null) {
                    foreach (var child in re.TeamMembers) {
                        var reiChild = new ResultListIntermediateFormattedChildRow( this, child );

                        rows.Add( reiChild );
                    }
                }
            }
        }

        /// <summary>
        /// UserProfielDB instance to look up if a user id has a public profile.
        /// </summary>
        protected internal IUserProfileLookup UserProfileLookup { get; private set; }

        public async Task RefreshCacheAsync() {
            //There is no need to await this method
            UserProfileLookup.RefreshUserProfileVisibilityAsync();
        }

        /// <summary>
        /// Gets the list of rows to display in the formatted results.
        /// </summary>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<ResultListIntermediateFormattedRow> Rows {
            get {
                if (!initialized)
                    throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );
                return rows; }
        }

        /// <summary>
        /// Gets the Orion ResultList used in this translation.
        /// </summary>
        public ResultList ResultList { get; private set; }

        /// <summary>
        /// Gets the ScoreFormatCollection Definition used in this Result List
        /// </summary>
        public ScoreFormatCollection ScoreFormatCollection { get; private set; }

        private SetName ScoreFormatCollectionSetName {
            get {
                //The ScoreFormatCollection should be included in the ResultListFormat,
                //but in case it isn't, ,pass back the default value of Standard Score Formats.
                SetName setName;
                try {
                    setName = SetName.Parse( ResultListFormat.ScoreFormatCollectionDef );
                } catch {
                    setName = SetName.Parse( "v1.0:orion:Standard Score Formats" );
                }
                return setName;
            }
        }

        private string ScoreConfigName {
            get {
                if (! string.IsNullOrEmpty( ResultList.ScoreConfigName ) ) 
                    return ResultList.ScoreConfigName;
                return ResultListFormat.ScoreConfigDefault;
            }
        }

        /// <summary>
        /// Helper property, returns the ScoreConfig to use when formatting scores.
        /// </summary>
        private ScoreConfig ScoreConfig { get; set; }

        public string GetScoreFormat(string scoreFormatName) {
            string scoreFormat;
            if (ScoreConfig.ScoreFormats.TryGetValue( scoreFormatName, out scoreFormat ))
                return scoreFormat;
            else
                return "{d}";
        }

        /// <summary>
        /// Gets the ResultListFormat definition in use in this display
        /// </summary>
        public ResultListFormat ResultListFormat { get; private set; }

        /// <summary>
        /// Gets the list of FieldNames in use by this ResultListFormat
        /// </summary>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<string> FieldNames {
            get {
                if (!initialized)
                    throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

                List<string> fieldNames = new();

                //Add the standard fields that are always included
                foreach (var field in ResultListIntermediateFormattedRow.StandardParticipantAttributeFields) {
                    fieldNames.Add( field );
                }

                //Add the fields that are defined in the ResultListFormat definition file
                foreach (var field in ResultListFormat.Fields) {
                    if (!fieldNames.Contains( field.FieldName.ToString() )) {
                        fieldNames.Add( field.FieldName.ToString() );
                    }
                }

                return fieldNames;
            }
        }


        /// <summary>
        /// Returns the number of defined columns. 
        /// </summary>
        /// <returns></returns>
        public int GetColumnCount() {
            return ResultListFormat.Format.Columns.Count;
        }

        /// <summary>
        /// Returns the CellValue for one cell in the header row, specified by the columnIndex
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index value is outside the range of columns.</exception>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public CellValues GetColumnHeaderCell( int columnIndex ) {
            if (!initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            CellValues cellValues = new CellValues();

            var format = ResultListFormat.Format.Columns[columnIndex];

            cellValues.Text = format.Header.ToString();
            cellValues.ClassList = new List<string>();
            foreach (var c in format.HeaderClassList)
                cellValues.ClassList.Add( c.ToString() );

            return cellValues;
        }

        /// <summary>
        /// Returns the entire header row, as a List of CellValues.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<CellValues> GetHeaderRow() {
            if (!initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            List<CellValues> row = new();

            for (int i = 0; i < GetColumnCount(); i++)
                row.Add( GetColumnHeaderCell( i ) );

            return row;
        }

        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<string> GetHeaderRowClassList() {
            if (!initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            List<string> l = new();
            foreach (var c in DisplayPartitions.Header.RowClass)
                l.Add( c.ToString() );
            return l;
        }

        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        [Obsolete( "Use .GetClassList() from the ResultListIntermediateFormattedRow instance instead.")]
        public List<string> GetBodyRowClassList() {
            if (!initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            List<string> l = new();
            foreach (var c in DisplayPartitions.Body.RowClass)
                l.Add( c.ToString() );
            return l;
        }

        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<string> GetFooterRowClassList() {
            if (!initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            List<string> l = new();
            foreach (var c in DisplayPartitions.Footer.RowClass)
                l.Add( c.ToString() );
            return l;
        }

        /// <summary>
        /// Returns the CellValue for one cell in the header row, specified by the columnIndex
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index value is outside the range of columns.</exception>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public CellValues GetColumnFooterCell( int columnIndex ) {
            if (!initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            CellValues cellValues = new();

            var format = ResultListFormat.Format.Columns[columnIndex];

            try {
                cellValues.Text = format.Footer.ToString();
                cellValues.ClassList = new List<string>();
                foreach (var c in format.FooterClassList)
                    cellValues.ClassList.Add( c.ToString() );
            } catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex) {
                ;
            }

            return cellValues;
        }

        /// <summary>
        /// Returns the entire header row, as a List of CellValues.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<CellValues> GetFooterRow() {
            if (!initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            List<CellValues> row = new();

            for (int i = 0; i < GetColumnCount(); i++)
                row.Add( GetColumnFooterCell( i ) );

            return row;
        }

        /// <summary>
        /// This ResultListDisplayPartitions to use is dependent on if the Result List is a team event. This is a helper property
        /// to return the correct ResultListDisplayPartitions.
        /// </summary>
        public ResultListDisplayPartitions DisplayPartitions {
            get {
                if (ResultList.Team && ResultListFormat.Format.DisplayForTeam != null)
                    return ResultListFormat.Format.DisplayForTeam;
                else
                    return ResultListFormat.Format.Display;
            }
        }
    }
}
