using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {

    /// <summary>
    /// ResultListFormatted is an intermediate formatted result list. It's like assembly code, half way 
    /// between a high livel language and binary code running on the CPU. In this case, it reads in the 
    /// raw result list data, and the result list format definition, to produce the ResultListIntermediateFormatted
    /// class.
    /// 
    /// The compiled ResultListFormat has a header row, footer row, and a body considting of 0 to n rows. 
    /// Each header, footer, and body row has the same number of columns. The ResultListFormat has methods
    /// to retreive either the textual value of each cell or a CallValue object that is the text value plus
    /// css class list.
    /// </summary>
    public class ResultListIntermediateFormatted {

        private readonly List<ResultListIntermediateFormattedRow> rows = new();
        public ShowWhenCalculator ShowWhenCalculator;
        private bool initialized = false;

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
            IUserProfileLookup userProfileLookup ) {

            this.ResultList = resultList;
            this.ResultListFormat = resultListFormat;
            this.UserProfileLookup = userProfileLookup;
            this.ShowWhenCalculator = new ShowWhenCalculator( this );
        }

        /// <summary>
        /// Completes the constructor process, makes necessary Async calls that can not be done within the Constructor.
        /// </summary>
        /// <param name="definitionApiClient"></param>
        /// <returns></returns>
        public async Task InitializeAsync() {

            //NOTE: Don't need to wait for the profile visibility initialization. 
            this.UserProfileLookup.RefreshUserProfileVisibilityAsync();

            ScoreFormatCollection = await DefinitionCache.GetScoreFormatCollectionDefinitionAsync( ScoreFormatCollectionSetName );

            //Set to a default value first.
            this.ScoreConfig = ScoreFormatCollection.ScoreConfigs[0];
            //Now set to the value passed in on the Result List.
            foreach (var sConfig in ScoreFormatCollection.ScoreConfigs) {
                if (sConfig.ScoreConfigName == ScoreConfigName) {
                    this.ScoreConfig = sConfig;
                    break;
                }
            }

            lock (rows) { //rows needs to be thread safe
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

            lock (rows) {
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
        }

        /// <summary>
        /// Clears the existing Rows of this ResultListIntermediateFormatted. Likely becuase the Result List is
        /// going to be reloaded (using AppendTokenizedResultList).
        /// </summary>
        public void Clear() {
            lock (rows) {
                rows.Clear();
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
        /// Gets the list of rows to display in the formatted results. This lists excludes Rows hidden by the row limited properties (listed below).
        /// </summary>
        /// <remarks>
        /// Limiting properties are implemented in this order:
        /// <list type="bullet">
        /// <item>ShowNumberOfChildRows (implemented for child rows only)</item>
        /// <item>ShowRanks (implemented for parent rows only)</item>
        /// <item>ShowStatuses</item>
        /// <item>ShowZeroScoresWithOFFICIAL</item>
        /// </list>
        /// </remarks>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<ResultListIntermediateFormattedRow> ShownRows {
            get {
                if (!initialized)
                    throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

                int childRowsRemaining = this.ShowNumberOfChildRows;
                bool skipRemainingChildren = false;

                lock (rows) {
                    List<ResultListIntermediateFormattedRow> copyOfRows = new List<ResultListIntermediateFormattedRow>();
                    foreach (var row in rows) {
                        if (!row.IsChildRow) {
                            //This is a parent row

                            //Reset the count of child rows to show, and skip remaining children
                            childRowsRemaining = this.ShowNumberOfChildRows;
                            skipRemainingChildren = false;

                            if (ShowRanks >= row.GetRank()) {
                                //Always show scores with a rank value up to and including ShowRanks
                                copyOfRows.Add( row );
                            } else {

                                if ( ShowStatuses.Contains( row.GetStatus() ) ) {
                                    //This row's status is one of the status we were told to include

                                    if ( this.ResultList.Status == ResultStatus.OFFICIAL 
                                        && ShowZeroScoresWithOFFICIAL 
                                        && row.GetScore( this.ResultList.EventName, false ).IsZero ) {
                                        //The result list status is official, and we were told to exclude rows with scores of zero

                                        if (row.GetParticipant().RemarkList.HasRemark( ParticipantRemark.DNS )
                                            || row.GetParticipant().RemarkList.HasRemark( ParticipantRemark.DNF )
                                            || row.GetParticipant().RemarkList.HasRemark( ParticipantRemark.DSQ )) {
                                            //Do include these rows since the athlete / team did somethign bad and we want to shame them
                                            copyOfRows.Add( row );
                                        } else {
                                            //Don't incldue this row
                                            skipRemainingChildren = true;
                                        }

                                    } else {
                                        copyOfRows.Add( row );
                                    }
                                } else {
                                    //Don't show this row b/c the status is not in the lists of statuses we were told to show
                                    skipRemainingChildren = true;
                                }
                            }
                            
                        } else {
                            //This is a child row
                            if (skipRemainingChildren)
                                continue;

                            //Check if we can show any more child rows
                            if (childRowsRemaining > 0) {
                                childRowsRemaining--;

                                if (ShowStatuses.Contains( row.GetStatus() )) {
                                    //This row's status is one of the status we were told to include

                                    if (this.ResultList.Status == ResultStatus.OFFICIAL
                                        && ShowZeroScoresWithOFFICIAL
                                        && row.GetScore( this.ResultList.EventName, false ).IsZero) {
                                        //The result list status is official, and we were told to exclude rows with scores of zero

                                        if (row.GetParticipant().RemarkList.HasRemark( ParticipantRemark.DNS )
                                            || row.GetParticipant().RemarkList.HasRemark( ParticipantRemark.DNF )
                                            || row.GetParticipant().RemarkList.HasRemark( ParticipantRemark.DSQ )) {
                                            //Do include these rows since the athlete / team did somethign bad and we want to shame them
                                            copyOfRows.Add( row );
                                        } else {
                                            //Don't incldue this row
                                            ;
                                        }

                                    } else {
                                        copyOfRows.Add( row );
                                    }
                                } else {
                                    //Don't show this row b/c the status is not in the lists of statuses we were told to show
                                    ;
                                }
                            }
                        }

                    }
                    return copyOfRows;
                }
            }
        }

        /// <summary>
        /// Gets the list of all rows to display in the formatted results. This lists includes Rows hidden by row limiting properties.
        /// </summary>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<ResultListIntermediateFormattedRow> Rows {
            get {
                if (!initialized)
                    throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

                int childRowsRemaining = this.ShowNumberOfChildRows;
                lock (rows) {
                    List<ResultListIntermediateFormattedRow> copyOfRows = new List<ResultListIntermediateFormattedRow>( rows );
                    return copyOfRows;
                }
            }
        }

        /// <summary>
        /// Returns the ResultListIntermediateFormattedRow who has the rankOrder provided.
        /// If no row is found, null is returned.
        /// </summary>
        /// <param name="rankOrder"></param>
        /// <returns></returns>
        public ResultListIntermediateFormattedRow GetRowAtRankOrder( int rankOrder ) {
            //NOTE: This does a linear search, could be improved by using a dictionary lookup.
            lock (rows) {
                foreach (var row in rows) {
                    if (row.IsChildRow)
                        continue;

                    if (row.GetRankOrder() == rankOrder)
                        return row;
                }
            }

            return null;
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
                if (!string.IsNullOrEmpty( ResultList.ScoreConfigName ))
                    return ResultList.ScoreConfigName;
                return ResultListFormat.ScoreConfigDefault;
            }
        }

        /// <summary>
        /// Helper property, returns the ScoreConfig to use when formatting scores.
        /// </summary>
        private ScoreConfig ScoreConfig { get; set; }

        /// <summary>
        /// Returns the score format to use when displayg scores of type 'scoreFormatName'.
        /// Examples include Events => "{i} - {x}" or Shots => "{m}{d}{X}".
        /// If a score format could not be found, based on the passed in scoreFormatName, then 
        /// the default value of "{d}" is returned.
        /// </summary>
        /// <param name="scoreFormatName"></param>
        /// <returns></returns>
        public string GetScoreFormat( string scoreFormatName ) {
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
        /// Returns the total number of defined columns. Does not factor in hidden columns (from .HideColumnsWithTheseClasses).
        /// </summary>
        /// <returns></returns>
        public int GetColumnCount() {
            return ResultListFormat.Format.Columns.Count;
        }

        /// <summary>
        /// Returns a list of columnIndex values, Each columnIndex is shown (e.g. not .Hide), as it 
        /// will not contain a CSS Class that's listed in .HideColumnsWithTheseClasses.
        /// </summary>
        /// <returns></returns>
        public List<int> GetShownColumnIndexes() {
            List<int> shownColumnIndexes = new List<int>();
            bool include = true;
            for (int i = 0; i < ResultListFormat.Format.Columns.Count; i++) {
                var column = ResultListFormat.Format.Columns[i];
                include = true;

                if (!ShowWhenCalculator.Show( column.ShowWhen )) {
                    include = false;
                }

                if (include)
                    shownColumnIndexes.Add( i );
            }

            return shownColumnIndexes;
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

            CellValues cellValues = new CellValues( this );

            var format = ResultListFormat.Format.Columns[columnIndex];

            cellValues.Text = format.Header;
            cellValues.ClassList = new List<string>();

            foreach (var c in format.ClassSet)
            {
                if(ShowWhenCalculator.Show(c.ShowWhen))
                    cellValues.ClassList.Add(c.Name);
            }

            return cellValues;
        }

        /// <summary>
        /// Returns the text value of the header cell, specified by the columnIndex.
        /// Returns the same value as .GetColumnHeaderCell( columnIndex ).Text
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <exception cref="InitializeAsyncNotCompletedException"></exception>
        public string GetColumnHeaderValue( int columnIndex ) {
            if (!initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            var format = ResultListFormat.Format.Columns[columnIndex];

            return format.Header;
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

            foreach (var c in DisplayPartitions.Header.ClassSet)
            {
                if (ShowWhenCalculator.Show(c.ShowWhen))
                    l.Add(c.Name);
            }

            return l;
        }

        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<string> GetFooterRowClassList() {
            if (!initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            List<string> l = new();

            foreach (var c in DisplayPartitions.Footer.ClassSet)
            {
                if (ShowWhenCalculator.Show(c.ShowWhen))
                    l.Add(c.Name);
            }

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

            CellValues cellValues = new CellValues( this );

            var format = ResultListFormat.Format.Columns[columnIndex];

            cellValues.Text = format.Footer.ToString();
            cellValues.ClassList = new List<string>();

            foreach (var c in format.ClassSet)
            {
                if (ShowWhenCalculator.Show(c.ShowWhen))
                    cellValues.ClassList.Add(c.Name);
            }

            return cellValues;
        }

        /// <summary>
        /// Returns the text value of the footer cell, specified by the columnIndex.
        /// Returns the same value as .GetColumnFooterCell( columnIndex ).Text
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <exception cref="InitializeAsyncNotCompletedException"></exception>
        public string GetColumnFooterValue( int columnIndex ) {
            if (!initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );


            var format = ResultListFormat.Format.Columns[columnIndex];

            return format.Footer;
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

        /// <summary>
        /// The width of the screen that this Result List Formatted will be displayeed on. Depending on the width
        /// some columsn may, or may not, be displayed. The Result List Fromatted uses the same breakpoints as
        /// Bootstrap 5. 
        /// <para>The default value is int.MaxValue, which basically means a screen width of infinite width.</para>
        /// <para>Values of less than 0, are interpreted as being 0.</para>
        /// </summary>
        public int ResolutionWidth { get; set; } = int.MaxValue;


        private int _showNumberOfChildren = int.MaxValue;
        private HashSet<ResultStatus> _showStatuses = new HashSet<ResultStatus>() { ResultStatus.FUTURE, ResultStatus.INTERMEDIATE, ResultStatus.UNOFFICIAL, ResultStatus.OFFICIAL };
        private bool _showZeroScoresWithOFFICIAL = false;
        private int _showRanks = 0;

        public void SetShowValuesToDefault() {
            ShowNumberOfChildRows = int.MinValue;
            ShowStatuses = new HashSet<ResultStatus>() { ResultStatus.FUTURE, ResultStatus.INTERMEDIATE, ResultStatus.UNOFFICIAL, ResultStatus.OFFICIAL };
            ShowZeroScoresWithOFFICIAL = false;
            ShowRanks = 0;
        }
        /// <summary>
        /// Limits the number of child rows to show under a main body row.
        /// <para>The default value is int.MaxValue, which means to show all children.</para>
        /// <para>Values of less than 0, are interpreted as being 0.</para>
        /// </summary>
        public int ShowNumberOfChildRows {
            get {
                return _showNumberOfChildren;
            }
            set {
                if (value <= 0)
                    _showNumberOfChildren = 0;
                else
                    _showNumberOfChildren = value;
            }
        }        

        /// <summary>
        /// Gets or sets the set of ResultStatus (e.g. INTERMEDIATE, UNOFFICIAL) to show when .ShownRows is called. 
        /// <para>When setting, if the HashSet is empty, then all ResultStatus will be included.</para>
        /// </summary>
        public HashSet<ResultStatus> ShowStatuses {
            get {
                HashSet<ResultStatus> copy = new HashSet<ResultStatus>();
                foreach (ResultStatus status in _showStatuses)
                    copy.Add( status );

                return copy;
            }
            set {
                if (value == null || value.Count == 0) {
                    _showStatuses = new HashSet<ResultStatus>() { ResultStatus.FUTURE, ResultStatus.INTERMEDIATE, ResultStatus.UNOFFICIAL, ResultStatus.OFFICIAL };
                } else {
                    _showStatuses = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if scores with a zero score shold be displayed when the Result LIst's staus is OFFICIAL.
        /// Scores that have a remark of DNS, DSQ, or DNF are still shown.
        /// </summary>
        public bool ShowZeroScoresWithOFFICIAL {
            get; set;
        }
        

        /// <summary>
        /// Gets or sets the number of highest ranked athletes or teams to always show, regardless of othre row limiting properties, when .ShownRows is called.
        /// For example, if RanksToShow is 3, then the athletes or teams ranked first, second, and third are always shown.
        /// <para>A value of 0 indicates to show all athletes and teams.</para>
        /// <para>This property only effects parent rows, it does not effect child rows.</para>
        /// </summary>
        public int ShowRanks {
            get {
                return _showRanks;
            }
            set {
                if (value <= 0)
                    _showRanks = 0;
                else
                    _showRanks = value;
            }
        }


        /// <summary>
        /// Gets or sets the engagable variable. Which indicates if the Result List Format will be displayed
        /// on a screen that supports human interaction, such as a desktop browser or mobile phone. A leaderboard
        /// or MM100 would not support human interaction.
        /// <para>The default value is true.</para>
        /// </summary>
        public bool Engagable { get; set; } = true;
    }
}
