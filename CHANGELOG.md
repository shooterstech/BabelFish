# Changelog
All notable changes to BabelFish will be documented in this file.


## [1.11.4] - 2026-01-00
### Enhancements
#### AverageMethod
- Added configuration option to count only the top n number of scores when calculating a participant's average.

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
