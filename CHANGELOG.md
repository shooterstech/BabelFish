# Changelog
All notable changes to BabelFish will be documented in this file.

## [2.1] - In Development Now, Expected Summer 2026
### Breaking Changes
** Contains many breaking changes **
- SetNames that were previously represented as strings are now represented as SetName objects.
- MatchIds that were previously represented as strings are now represented as MatchID objects.
- Datas and Times that were previously represented as strings are now represented as DateTime objects.
- The Tournament class is renamed to the ResultListMerger class.
### Enhancements
#### Match
- Matches may now have multiple Courses of Fire (1.x version allowed only one), each of which is defined in a new class CourseOFFireStructure.
- Multiple properties within the Match class are now deprecated, as they previously existed when only one Course of Fire was allowed. These deprecated properties will remain for backwards compatibility for at least 1 year after release. They will however point to the first (and default) CourseOfFireStructure.
- ResultListWizard data actor class is added to generated suggested ResultListAbbr (configuration data for a ResultList) based on a CourseOfFireStructure.
- AttributeFilterCalculator data actor class is added to filter Participants (both Individuals and Teams) based on their AttributeValues.
#### MatchProject
- Added the MatchProject class which is the Top level class that is a container for all data related to an Orion Match (data file). This includes the Match object itself, as well as the Participants, and raw score data. Also includes a Result Engine (data actors) to interpret definition files and help manage the Match data, such as the ShotMapper, ResultDocumentGenerator, ResultListSlidingWindow.
- MatchProject includes serialization and deserialization methods to save to and read from files.
- The new ShotMapper data actor stores shot data from ESTs (and other sources) and maps the shots to Events defined by the COURSE OF FIRE.
- The new ResultDocumentGenerator data actor compiles Participant and Shot data into both (individual) ResultCOF and ResultList instances.
- The new ResultListSlidingWindow data actor tracks recent versions of ResultList instances to use as comparisons in RankDelta calculations.
#### Club
- ClubDetail now has a list of ClubAddress and ClubContact instances. Independent VisibilityOptions may be set to each of these.
#### RULEBOOK Definition
- Added the top level RULEBOOK definition. RULEBOOKs contains lists of options for Match Structures, Courses of Fire, and Attributes that a user can use to construct their own Match Structure and Match. 
#### Match API Calls
- Deprecated MatchSearch API calls replaced them with ListMatches.
- Added option in ListMatches to include away matches -- matches that members of the club competed in but not hosted directly by the specified club.
- Added Range Reporter API calls, allowing AI generated press releases to be saved with each Result List.
#### Tournament API Calls
A Tournament is a group of Matches. Once a Tournament is created "Merged Result Lists" may be added combining scores from accross multiple Result Lists from the member Matches.
- Added API calls to create, read, and update Tournaments, this includes Merged Result Lists within a Tournament.
- Added Tournament permissions to allow "Invite Only" (only the creator of a Tournament may add members), "Request to Join" (any Match may ask to join but the creator of the Tournament must approve), and "Open" (anyone may join without approval) policies.

## [1.12.5] - 2026-05-14
### Enhancements
#### ICheckSum
- Added the ICheckSum interface to calculate a checksum value for high level documents. Intended to check difference in instances between local copy and a server's copy. Implemented in many DataModel/OrionMatch classes including Match, ResultList, ResultCOF, and SquaddingList.

## [1.12.4] - 2026-03-20
### Enhancements
#### GetClubList
- Added a list of ClubAuthorizationRoles the authenticated caller has for each returned Club.
#### ResultListIntermediateFormatted
- Added 'Team' as a standard field name, displaying the Team Name that the participant is competing for.
### Bug Fixes
#### ResultCOF
- Added 'Visibility' as a property to ResultCOF
#### Authentication
- Fixed issue with automatically refreshing authentication tokens after 24 hours.

## [1.12.3] - 2026-03-03
### Enhancements
#### ProjectScoresByAverageShotFired
- Updated projection algorithm to factor in relative difficulty of each stage, in a multi-stage event.
### Bug Fixes
- Fixed issue with TargetAnalysis that was calling an async method in a non-async function.

## [1.12.2] - 2026-02-20
### Enhancements
#### MatchSearchPublicRequest
- Added ability to search for matches based on the owner of the match (aka Orion Club).
#### ResultListIntermediateFormattedRow
- Updated the return value for an Attribute to be the Field's Name (previously was the Field's Value).

### Bug Fixes
#### MatchAbbr
- Corrected variable type for StartDate and EndDate to be a DateTime (sorry, this is technically a breaking change albeit a bug fix).
#### Attribute Specification
- Fixed bug that was allowing INTERNAL visibility to be higher than PROTECTED visibility.


## [1.12.1] - 2026-02-10
### Enhancements
#### StringFormatting
- Updated FormatScore() method to include DNF use case.
#### DynamicEssentialDataFile
- Added a DynamicEssentialDataFile class that dynamically generates a RESULT LIST FORMAT definition listing all demographic, squadding, and score data contained in a Result List.
#### DynamicSquadding
- Added a DynamicSquadding class that dynamically generates a RESULT LIST FORMAT definition listing most demographic, squadding, and attribute values contained in a Squadding List.
#### ResultListExcel
** Breaking Changes **
- Refactored the ResultListExcel class to have a FactoryAsync method to construct new instances.
- Abstracted the FactoryAsync method to work with either ResultLists or SquaddingList objects.
- The GenerateExcel method now returns a byte[].
- When instantiating a new instance, the default behavior is to create an Excel file with two worksheets. The first uses the standard RESULT LIST FORMAT. The second worksheet uses the new dynamically gnerated essential data format RESULT LIST FORMAT.

### Bug Fixes
#### SquaddingLists
- Fixed bug that was prevening Attribute Value included in a SquaddingList from being deserialized. 


## [1.11.4] - 2026-01-23
### Enhancements
#### AverageMethod
- Added configuration option to count only the top n number of scores when calculating a participant's average.
#### SumMethod
- Added configuration option to count only the top n number of scores when calculating a participant's summation aggregate.

### Bug Fixes
#### ResultListIntermediateFormatted
- Fix bug that was allowing the spanninng row to show, even when the ShowSpanningRow property evaluated to false.
- Fix bug that was allowing participants, who have not shot yet, be included in ShowRanks of <= 3. 


## [1.11.3] - 2026-01-15
### Enhancements
#### Multiline Rows in RESULT LIST FORMAT
- Added support for defining multiline rows in the RESULT LIST FORMAT. 
- ResultListIntermediateFormat class updated to support multiline rows.
#### User Defined Text in RESULT LIST FORMAT
- Added common fields OptionText1, OptionText2, and OptionText3. Each may be used in the Body of a ResultListDisplayColumn.
- The value of the optional text fields are set by each Result List. The ResultList and Match object updated to store these values.
#### MatchHtmlReport Class
- Added MatchHtmlReport class for listing (html) reports associated with a Match.
- Added the property .HtmlReport, which is a list of MatchHtmlReport, to the Match object. Will be returned by Scopos's GetMatchDetail REST API.
