# Changelog
All notable changes to BabelFish will be documented in this file.


## [2.1.0-alpha] - 2026-03-00
** Alpha build, not intended for outside of Scopos use **
** Contains breaking changes **
### Enhancements
#### SetName References
- All SetName references (e.g. TargetDef) have been changed from a string to SetName object.
- When a SetName in JSON is deserialized, if it can not be parsed or the value is null, the SetName 1.0:orion:Default is returned.
#### AttributeFilter
- Added a series of classes, derived from the abstract class AttributeFilter, that specify conditions in which a Participant passes or doesn't pass. Intended to be used to filter a list of Participants for inclusion on a Result List. For example, list all the Participants in a match that are shooting Sporter air rifle.
- Added the AttributeFilterCalculator that tests if a Participant meets the filter's specifications.
#### CourseOfFire
- Added property for RequiredAttributeDef and deprecated DefaultAttributeDef, which will specify which, if any, ATTRIBUTE is required when the COURSE OF FIRE is added to an Orion Match. 
- Added specification to check that RequiredAttributeDef is a simple attribute, of type string, and each field value specifies an Attribute Value Appelation.


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
