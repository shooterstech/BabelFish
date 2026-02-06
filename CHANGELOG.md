# Changelog
All notable changes to BabelFish will be documented in this file.


## [1.12.1] - 2026-02-00
### Enhancements
#### StringFormatting
- Updated FormatScore() method to include DNF use case.
#### EssentialDataFile
- Added a EssentialDataFile class that dynamically generates a RESULT LIST FORMAT definition listing all demographic and score data contained in a Result List.
#### ResultListExcel
*** Breaking Changes ***
- Refactored the ResultListExcel class to have a FactoryAsync method to construct new instances.
- The GenerateExcel method new returns a byte[].
- When instantiating a new instance, the default behavior is to create an Excel file with two worksheets. The first uses the standard RESULT LIST FORMAT. The second worksheet uses the new dynamically gnerated essential data format RESULT LIST FORMAT.


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
