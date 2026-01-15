using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Responses.OrionMatchAPI;

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

        private Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly List<ResultListIntermediateFormattedRow> _rows = new();
        public ShowWhenCalculator ShowWhenCalculator;
        private bool _initialized = false;
        /// <summary>
        /// Key is Result COF ID. Value is the ResultListIntermediateFormattedRow.
        /// Dictionary is cleared when it is dirty (a row is added to _rows).
        /// </summary>
        private Dictionary<string, ResultListIntermediateFormattedRow> _rowLookup = new Dictionary<string, ResultListIntermediateFormattedRow>();

        /// <summary>
        /// Converts a list of ResultEvents, that is returned by the GetResultList API, to
        /// a list of ResultEventIntermediates. 
        /// </summary>
        /// <param name="resultEventList"></param>
        /// <param name="resultListFormat"></param>
        /// <returns></returns>
        public ResultListIntermediateFormatted(
            IRLIFList resultList,
            ResultListFormat resultListFormat,
            IUserProfileLookup userProfileLookup ) {

            this.RLIFList = resultList;
            this.ResultListFormat = resultListFormat;
            this.UserProfileLookup = userProfileLookup;
            this.ShowWhenCalculator = new ShowWhenCalculator( this );

            if (this.UserProfileLookup is null)
                this.UserProfileLookup = new BaseUserProfileLookup();

            // Read in the user defined text from the Result List.
            if (this.RLIFList.UserDefinedText != null) {
                if (this.RLIFList.UserDefinedText.TryGetValue( UserDefinedFieldNames.USER_DEFINED_FIELD_1, out string text1 )) {
                    this.UserDefinedText[UserDefinedFieldNames.USER_DEFINED_FIELD_1] = text1;
                }

                if (this.RLIFList.UserDefinedText.TryGetValue( UserDefinedFieldNames.USER_DEFINED_FIELD_2, out string text2 )) {
                    this.UserDefinedText[UserDefinedFieldNames.USER_DEFINED_FIELD_2] = text2;
                }

                if (this.RLIFList.UserDefinedText.TryGetValue( UserDefinedFieldNames.USER_DEFINED_FIELD_3, out string text3 )) {
                    this.UserDefinedText[UserDefinedFieldNames.USER_DEFINED_FIELD_3] = text3;
                }
            }
        }

        /// <summary>
        /// Completes the constructor process, makes necessary Async calls that can not be done within the Constructor.
        /// </summary>
        /// <param name="runRefreshUserProfileVisibilityAsync">If the caller selects no, they should run this method manually, later.</param>
        /// <returns></returns>
        public async Task InitializeAsync( bool runRefreshUserProfileVisibilityAsync = true ) {

            //NOTE: Don't need to wait for the profile visibility initialization. 
            if (runRefreshUserProfileVisibilityAsync)
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

            //Grow the Event Tree, from the CORUSE OF FIRE listed in the Result List.
            if (this.RLIFList is ResultList rl) {
                try {
                    EventTree = EventComposite.GrowEventTree( await rl.GetCourseOfFireDefinitionAsync() );
                } catch (Exception ex) {
                    EventTree = null;
                }
            } else {
                EventTree = null;
            }

            lock (_rows) { //rows needs to be thread safe
                foreach (var item in RLIFList.GetAsIRLItemsList()) {
                    var rei = new ResultListIntermediateFormattedBodyRow( this, item );

                    _rows.Add( rei );
                    _rowLookup.Clear();

                    if (item is ResultEvent re && re.TeamMembers != null) {
                        foreach (var child in re.TeamMembers) {
                            var reiChild = new ResultListIntermediateFormattedChildRow( this, child );

                            _rows.Add( reiChild );
                            rei.ChildRows.Add( reiChild );
                            reiChild.ParentRow = rei;
                        }
                    }

                }
            }

            _initialized = true;
        }

        /// <summary>
        /// When a Result List is tokenized (returned from the rest API in chuncks of 50), its common that ResultListIntermediateFormatted
        /// is initialize with only the first chunck of results. This function allows the user to append additional tokenized ResultList
        /// to this instance. It assumes the user is passing in the same ResultList, just with a different tokenized chunk of result list items.
        /// </summary>
        /// <remarks>The caller may wish to call .ReSort() on the Result List prior to calling .AppendTokenizedREsultList().
        /// This is an exstension method that checks to see if the status of the Result List is official, but remains sorted by
        /// projected ranking (happens when the orion user closes match too soon). </remarks>
        /// <param name="tokenizedResultList"></param>
        public void AppendTokenizedResultList( IRLIFList tokenizedResultList ) {

            lock (_rows) {

                foreach (var item in tokenizedResultList.GetAsIRLItemsList()) {
                    var rei = new ResultListIntermediateFormattedBodyRow( this, item );

                    _rows.Add( rei );
                    _rowLookup.Clear();

                    if (item is ResultEvent re && re.TeamMembers != null) {
                        foreach (var child in re.TeamMembers) {
                            var reiChild = new ResultListIntermediateFormattedChildRow( this, child );

                            _rows.Add( reiChild );
                            rei.ChildRows.Add( reiChild );
                            reiChild.ParentRow = rei;
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
            lock (_rows) {
                _rows.Clear();
            }
        }

        /// <summary>
        /// Updates the reference result list. Mostly used to refreshing the Result List's Meta Data.
        /// Does NOT effect the rows, ,use Clear() and AppendTokenizedResultList() to do this. 
        /// </summary>
        /// <param name="updatedResultList"></param>
        public void RefreshResultList( IRLIFList updatedResultList ) {
            this.RLIFList = updatedResultList;
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
        /// Refreshes the _rowLookup dictionary. _rowLookup is a cache that looks up a row, in this RLIF based on the result cof id.
        /// </summary>
        private void RefreshRowLookup() {
            lock (_rows) {
                //_rowLookup is cleared when _rows is updated with new rows.
                //We populate it only when needed
                if (_rowLookup.Count > 0)
                    return;

                foreach (var row in _rows) {
                    if (row.GetParticipant() is Individual individual && !string.IsNullOrEmpty( individual.ResultCOFID )) {
                        _rowLookup[individual.ResultCOFID] = row;
                    }
                }
            }
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
                if (!_initialized)
                    throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );


                lock (_rows) {
                    List<ResultListIntermediateFormattedRow> copyOfRows = new List<ResultListIntermediateFormattedRow>();
                    this.ResetNumberOfChildRowsLeftToShow();

                    foreach (var row in _rows) {
                        //Reset the count of child rows to show. Note this only effects Body/Parent rows
                        row.ResetNumberOfChildRowsLeftToShow();

                        if (row.ShowRowBasedOnShowRanks()) {
                            //Always show scores with a rank value up to and including ShowRanks
                            row.RowIsShown = true;
                            copyOfRows.Add( row );
                        } else {

                            if (row.ShowRowBasedOnShownStatus()
                                && row.ShowRowBasedOnShowRelay()
                                && row.ShowRowBasedOnShowNumberOfBodies()
                                && row.ShowRowBasedOnShowNumberOfChildren()
                                && row.ShowRowBasedZeroScores()) {
                                row.RowIsShown = true;
                                row.Reset();
                                copyOfRows.Add( row );
                            } else {
                                row.RowIsShown = false;
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
                if (!_initialized)
                    throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

                int childRowsRemaining = this.ShowNumberOfChildRows;
                lock (_rows) {
                    List<ResultListIntermediateFormattedRow> copyOfRows = new List<ResultListIntermediateFormattedRow>( _rows );
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
            lock (_rows) {
                foreach (var row in _rows) {
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
        public IRLIFList RLIFList { get; private set; }

        public ResultList? ResultList {
            get {
                if (RLIFList is ResultList)
                    return (ResultList)RLIFList;

                return null;
            }
        }

        public EventComposite EventTree { get; private set; }

        /// <summary>
        /// Gets the ScoreFormatCollection Definition used in this Result List
        /// </summary>
        public ScoreFormatCollection ScoreFormatCollection { get; private set; }

        /// <summary>
        /// The set name of the SCORE FORMAT COLLECTION to use to format scores. 
        /// <para>If one is not specified in the RESULT LIST FORMAT then the default v1.0:orion:Standard Score Formats
        /// is used.</para>
        /// </summary>
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

        /// <summary>
        /// The ScoreConfigName, within the SCORE FORMAT COLLECTION, to use to format scores.
        /// If a value is not found within the ResultLIst object, then the default value from 
        /// the RESULT LIST FORMAT definition is returned.
        /// </summary>
        private string ScoreConfigName {
            get {
                if (ResultList != null && !string.IsNullOrEmpty( ResultList.ScoreConfigName ))
                    return ResultList.ScoreConfigName;
                if (ResultListFormat != null)
                    return ResultListFormat.ScoreConfigDefault;
                return "Integer";
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
                if (!_initialized)
                    throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

                List<string> fieldNames = new();

                //Add the standard fields that are always included
                foreach (var @field in ResultListIntermediateFormattedRow.StandardParticipantAttributeFields) {
                    fieldNames.Add( @field );
                }

                //Add the fields that are defined in the ResultListFormat definition file
                foreach (var @field in ResultListFormat.Fields) {
                    if (!fieldNames.Contains( @field.FieldName.ToString() )) {
                        fieldNames.Add( @field.FieldName.ToString() );
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

        //Chached values for .GetShownColumnIndexes()
        List<int> _shownColumnIndexes = new List<int>();
        DateTime _shownColumnIndexesCachedTime = DateTime.MinValue;

        /// <summary>
        /// Returns a list of columnIndex values, Each columnIndex is shown (e.g. not .Hide), as it 
        /// will not contain a CSS Class that's listed in .HideColumnsWithTheseClasses.
        /// </summary>
        /// <returns></returns>
        public List<int> GetShownColumnIndexes() {

            //Becuase GetShownColumnIndexes() gets called a lot, we will cache the return value
            //And only recalculate it ever 2 seconds.
            if ((DateTime.UtcNow - _shownColumnIndexesCachedTime) < TimeSpan.FromSeconds( 2 ))
                return _shownColumnIndexes;

            _shownColumnIndexes.Clear();
            bool include = true;
            for (int i = 0; i < ResultListFormat.Format.Columns.Count; i++) {
                var column = ResultListFormat.Format.Columns[i];
                include = true;

                if (!ShowWhenCalculator.Show( column.ShowWhen )) {
                    include = false;
                }

                if (include)
                    _shownColumnIndexes.Add( i );
            }

            _shownColumnIndexesCachedTime = DateTime.UtcNow;
            return _shownColumnIndexes;
        }

        /// <summary>
        /// Returns the CellValue for one cell in the header row, specified by the columnIndex
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index value is outside the range of columns.</exception>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public CellValues GetColumnHeaderCell( int columnIndex ) {

            if (!_initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            CellValues cellValues = new CellValues( this );

            var format = ResultListFormat.Format.Columns[columnIndex];

            cellValues.Text = format.Header;
            cellValues.ClassList = new List<string>();

            foreach (var c in format.ClassSet) {
                if (ShowWhenCalculator.Show( c.ShowWhen ))
                    cellValues.ClassList.Add( c.Name );
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
            if (!_initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            var format = ResultListFormat.Format.Columns[columnIndex];

            return format.Header;
        }

        /// <summary>
        /// Returns the entire header row, as a List of CellValues.
        /// <para>To get a list of only the columns that are shown, use .GetShownHeaderRow() instead.</para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<CellValues> GetHeaderRow() {
            if (!_initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            List<CellValues> row = new();

            for (int i = 0; i < GetColumnCount(); i++)
                row.Add( GetColumnHeaderCell( i ) );

            return row;
        }

        /// <summary>
        /// Returns the shown header row, as a List of CellValues.
        /// <para>Similiar to .GetHeaderRow() but only includes CellValues for the columns that are shown.</para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<CellValues> GetShownHeaderRow() {
            if (!_initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            List<CellValues> row = new();

            foreach (int i in this.GetShownColumnIndexes())
                row.Add( GetColumnHeaderCell( i ) );

            return row;
        }

        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<string> GetHeaderRowClassList() {
            if (!_initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            List<string> l = new();

            foreach (var c in DisplayPartitions.Header.ClassSet) {
                if (ShowWhenCalculator.Show( c.ShowWhen ))
                    l.Add( c.Name );
            }

            return l;
        }

        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<string> GetFooterRowClassList() {
            if (!_initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            List<string> l = new();

            foreach (var c in DisplayPartitions.Footer.ClassSet) {
                if (ShowWhenCalculator.Show( c.ShowWhen ))
                    l.Add( c.Name );
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
            if (!_initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            CellValues cellValues = new CellValues( this );

            var format = ResultListFormat.Format.Columns[columnIndex];

            cellValues.Text = format.Footer.ToString();
            cellValues.ClassList = new List<string>();

            foreach (var c in format.ClassSet) {
                if (ShowWhenCalculator.Show( c.ShowWhen ))
                    cellValues.ClassList.Add( c.Name );
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
            if (!_initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );


            var format = ResultListFormat.Format.Columns[columnIndex];

            return format.Footer;
        }

        /// <summary>
        /// Returns the entire header row, as a List of CellValues.
        /// <para>To get a list of only the shown columns, use .GetShownFooterRow() instead.</para>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if the caller does not complete the initilization process by calling InitializeAsync()</exception>
        public List<CellValues> GetFooterRow() {
            if (!_initialized)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the ResultListIntermediateFormatted constructor. Can not proceed until after this call was successful." );

            List<CellValues> row = new();

            for (int i = 0; i < GetColumnCount(); i++)
                row.Add( GetColumnFooterCell( i ) );

            return row;
        }

        /// <summary>
        /// Returns a list of shown footer row CellValues. 
        /// <para>Similiar to .GetFooterRow(), but this method only return footer CellValues for the shown columns.</para>
        /// </summary>
        /// <returns></returns>
        public List<CellValues> GetShownFooterRow() {
            List<CellValues> l = new();
            foreach (int i in this.GetShownColumnIndexes()) {
                l.Add( this.GetColumnFooterCell( i ) );
            }
            return l;
        }

        /// <summary>
        /// This ResultListDisplayPartitions to use is dependent on if the Result List is a team event. This is a helper property
        /// to return the correct ResultListDisplayPartitions.
        /// </summary>
        public ResultListDisplayPartitions DisplayPartitions {
            get {
                if (ResultList != null && ResultList.Team && ResultListFormat.Format.DisplayForTeam != null)
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

        /// <summary>
        /// Returns a boolean, indicating if some columns are hidden because the ResolutionWidth is too small.
        /// </summary>
        public bool HasColumnsForWiderScreen {
            get {

                foreach (var column in ResultListFormat.Format.Columns) {
                    if (ShowWhenCalculator.GetLargestShowWhenResolution( column.ShowWhen ) > ResolutionWidth) {
                        return true;
                    }
                }

                return false;
            }
        }

        private int _showNumberOfBodies = int.MaxValue;
        private int _showNumberOfChildren = int.MaxValue;
        private HashSet<ResultStatus> _showStatuses = new HashSet<ResultStatus>() { ResultStatus.FUTURE, ResultStatus.INTERMEDIATE, ResultStatus.UNOFFICIAL, ResultStatus.OFFICIAL };
        private bool _showZeroScoresWithOFFICIAL = false;
        private int _showRanks = 3;
        private string _showRelay = string.Empty;

        /// <summary>
        /// Resets all "Show" values to their default. Specifially:
        /// <list type="bullet">
        /// <item>Sets .ShowNumberofChildRows to show all children</item>
        /// <item>Sets .ShowStatuses to show all rows (participants) regardless of status.</item>
        /// <item>Sets .ShowZeroScoresWithOFFICIAL to hide scores of zero if the ResultStatus is OFFICIAL</item>
        /// <item>Sets .ShowRanks to always show the top three ranked participants.</item>
        /// <item>Sets .ShowRelay to show all participants from all relays.</item>
        /// </list>
        /// </summary>
        public void SetShowValuesToDefault() {
            ShowNumberOfChildRows = int.MaxValue;
            ShowNumberOfBodyRows = int.MaxValue;
            ShowStatuses = new HashSet<ResultStatus>() { ResultStatus.FUTURE, ResultStatus.INTERMEDIATE, ResultStatus.UNOFFICIAL, ResultStatus.OFFICIAL };
            ShowZeroScoresWithOFFICIAL = true;
            ShowZeroScoresBeforeOFFICIAL = true;
            ShowRanks = 3;
            ShowRelay = string.Empty;
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
        /// Limits the number of body roes to show.
        /// <para>The default value is int.MaxValue, which means to show all body rows.</para>
        /// <para>This show parameter is over ruled by the ShowRanks parameter.</para>
        /// </summary>
        public int ShowNumberOfBodyRows {
            get {
                return _showNumberOfBodies;
            }
            set {
                if (value <= 0)
                    _showNumberOfBodies = 0;
                else
                    _showNumberOfBodies = value;
            }
        }


        private int _numberOfBodyRowsLeftToShow = 0;
        internal bool GetTokenToShowBodyRow( ResultListIntermediateFormattedBodyRow childRow ) {
            _numberOfBodyRowsLeftToShow--;
            return _numberOfBodyRowsLeftToShow > -1;
        }

        internal virtual void ResetNumberOfChildRowsLeftToShow() {
            _numberOfBodyRowsLeftToShow = ShowNumberOfBodyRows;
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
        /// Scores that have a remark of DNS, DSQ, or DNF are still shown regardless of value.
        /// <para>The defautl value is true.</para>
        /// </summary>
        public bool ShowZeroScoresWithOFFICIAL {
            get; set;
        }

        /// <summary>
        /// Gets or sets a boolean indicating if scores with a zero score shold be displayed when the Result List status is FUTURE, INTERMEDIATE, or UNOFFICIAL
        /// Scores that have a remark of DNS, DSQ, or DNF are still shown regardless of value.
        /// <para>The defautl value is true.</para>
        /// </summary>
        public bool ShowZeroScoresBeforeOFFICIAL {
            get; set;
        }


        /// <summary>
        /// Gets or sets the number of highest ranked athletes or teams to always show, 
        /// regardless of othre row limiting properties, when .ShownRows is called.
        /// For example, if ShowRanks is 3, then the athletes or teams ranked first, second, and third are always shown.
        /// <para>This property only effects parent rows, it does not effect child rows.</para>
        /// <para>The default value is 3.</para>
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
        /// Gets or sets the name of the relay to show. When set, only athletes on that relay will be shown. 
        /// Teams, who have one or more athltes on that relay will be shown, but only the team members on that
        /// relay will be shown. 
        /// <para>The default value is an empty string, which means to show all competitors from all relays.</para>
        /// </summary>
        public string ShowRelay {
            get {
                return _showRelay;
            }
            set {
                if (string.IsNullOrEmpty( value ))
                    _showRelay = string.Empty;
                else
                    _showRelay = value;
            }
        }


        /// <summary>
        /// Gets or sets the engagable variable. Which indicates if the Result List Format will be displayed
        /// on a screen that supports human interaction, such as a desktop browser or mobile phone. A leaderboard
        /// or MM100 would not support human interaction.
        /// <para>The default value is true.</para>
        /// </summary>
        public bool Engagable { get; set; } = true;

        /// <summary>
        /// Gets or sets the show supplemental information variable. What is or what is not supplemental information
        /// is up to the composers of the RESULT LIST FORMAT to decide. Generally though its extra information
        /// that is interesting, but not necessaryly required.
        /// <para>The default value is true.</para>
        /// </summary>
        public bool ShowSupplementalInformation { get; set; } = true;

        /// <summary>
        /// Boolean, indicating if the Spanning row should be included. 
        /// </summary>
        public bool ShowSpanningRows { get; set; } = true;

        /// <summary>
        /// On RESULT LIST FORMAT definitions that provided for the option, the user (usually the Match Director) may specify their own
        /// interpolated values for designated fields. These are known as UserDefinedText. There are at most three user defined fields in a
        /// RESULT LIST FORMAT (man definitions do not have any).
        /// <para>The most common example is a demographic spanning text field.</para>
        /// <para>Text values are interpolated with any common field or user defined field. The list is common fields is at
        /// <see href="https://support.scopos.tech/index.html?definition-resultlistfield.html">support.scopos.tech</see></para>
        /// <para>Example text values:</para>
        /// <list type="bullet">
        /// <item>"Competitor Number: {CompetitorNumber}, Hometown: {Hometown}</item>
        /// <item>"Club: {Organization}, Coach: {Coach}</item>
        /// </list>
        /// </summary>
        public Dictionary<UserDefinedFieldNames, string> UserDefinedText { get; set; } = new Dictionary<UserDefinedFieldNames, string>() {
            [UserDefinedFieldNames.USER_DEFINED_FIELD_1] = string.Empty,
            [UserDefinedFieldNames.USER_DEFINED_FIELD_2] = string.Empty,
            [UserDefinedFieldNames.USER_DEFINED_FIELD_3] = string.Empty,
        };

        #region Participant Attribute Delegates
        /// <summary>
        /// Recalculates the standard participant attribute fields (e.g. rank, display name, club, etc). Is useful
        /// to call if the user overrode one of the GetParticipantAttribute Pointers
        /// </summary>
        public void RefreshAllRowsParticipantAttributeFields() {
            lock (_rows) {
                foreach (var r in _rows) {
                    r.RefreshStandardParticipantAttributeFields();
                }
            }
        }

        /// <summary>
        /// When a ResultListIntermediateFormatted is instantiated with a ResultList, the ResultList object generally
        /// does not include Squadding (the values of .Squadding are null). This method uses GetSquaddingListPublic REST API call to read
        /// in squadding, setting the values of .Squadding. 
        /// <para>Participants are matched up, between the ResultLIst and SquaddingList using their ResultCofId. </para>
        /// </summary>
        /// <returns></returns>
		public async Task LoadSquaddingListAsync() {

            OrionMatchAPIClient orionMatchAPIClient = new OrionMatchAPIClient();

            List<Task<GetSquaddingListPublicResponse>> responses = new List<Task<GetSquaddingListPublicResponse>>();
            foreach (var metaData in this.ResultList.Metadata.Values) {
                //Make the request for all the squadding lists in parrallel
                try {
                    var matchId = new MatchID( metaData.MatchID );
                    var squaddingListName = metaData.SquaddingListName;
                    responses.Add( orionMatchAPIClient.GetSquaddingListPublicAsync( matchId, squaddingListName ) );
                } catch (Exception ex) {
                    _logger.Error( ex );
                }

                //Now wait for each one
                try {
                    foreach (var response in responses) {
                        var getSquaddingListResponse = await response;
                        if (getSquaddingListResponse.RestApiStatusCode == System.Net.HttpStatusCode.OK) {
                            LoadSquaddingList( getSquaddingListResponse.SquaddingList );
                        }
                    }
                } catch (Exception ex) {
                    _logger.Error( ex );
                }
            }

            RefreshAllRowsParticipantAttributeFields();

        }

        /// <summary>
        /// When a ResultListIntermediateFormatted is instantiated with a ResultList, the ResultList object generally
        /// does not include Squadding (the values of .Squadding are null). This method uses the passed in Squadding List to read
        /// in squadding, setting the values of .Squadding. 
        /// <para>Participants are matched up, between the ResultLIst and SquaddingList using their ResultCofId. </para>
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public void LoadSquaddingList( SquaddingList squaddingList ) {
            if (squaddingList == null)
                return;

            RefreshRowLookup();

            foreach (var item in squaddingList.Items) {
                if (item.SquaddingAssignment != null && item.Participant is Individual individual) {
                    if (_rowLookup.TryGetValue( individual.ResultCOFID, out ResultListIntermediateFormattedRow row )) {
                        row.SetSquaddingAssignment( item.SquaddingAssignment );
                    }
                }
            }
        }

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Rank field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeRankPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the RankOrder field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeRankOrderPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the RankDelta field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeRankDeltaPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the RankOrSquadding field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeRankOrSquaddingPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Empty field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeEmptyPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the DisplayName field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeDisplayNamePtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the DisplayNameShort field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeDisplayNameShortPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the FamilyName field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeFamilyNamePtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the GivenName field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeGivenNamePtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the MiddleName field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeMiddleNamePtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the HomeTown field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeHomeTownPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Country field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeCountryPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Club field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeClubPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the CompetitorNumber field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeCompetitorNumberPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Coach field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantCoachPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the MatchLocation field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeMatchLocationPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the MatchID field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeMatchIDPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the LocalDate field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeLocalDatePtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the ResultCOFID field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeResultCOFIDPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the UserID field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeUserIDPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Creator field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeCreatorPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Owner field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeOwnerPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the TargetCollectionName field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeTargetCollectionNamePtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Status field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeStatusPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the LastShot field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeLastShotPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Remark field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeRemarkPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Squadding field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeSquaddingPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Squadding field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeRelayPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Squadding field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeFiringPointPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Squadding field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeFiringOrderPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Squadding field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeSquadPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Squadding field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeBankPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Squadding field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeReentryPtr { get; set; } = null;

        /// <summary>
        /// Overrides the method the ResultListIntermediateFormatted uses to calculate the Squadding field
        /// in each row. 
        /// </summary>
        /// <remarks>
        /// After updating, be sure to call RefreshAllRowsParticipantAttributeFields to use the new 
        /// method in the field value.
        /// </remarks>
        public ParticipantAttributeOverload GetParticipantAttributeRangePtr { get; set; } = null;

        /// <summary>
        /// Overrides the method used to return a string to represent the completion percentage of an athletes stage.
        /// </summary>
        public CompletionPercentageOverload GetCompletionPercentageStringPtr { get; set; } = null;

        #endregion
    }
}
