# BabbelFish
Dot Net Library that provides a façade for Shooter's Tech REST API interface.

NOTE: Library is considered a WORK-IN-PROGRESS and is NOT intended for production environment at this time.

v1.0.7.0
Implement AWS Signer v4 functionality for Authenticated APIs
Add Allowed UserSettings to support AWS Signing

v1.0.6.0
Introduce User Settings: 
 Add App.config that contains defaults and reads User Settings file BabelFish_User_Settings.config
 Add incoming UserSettings Dictionary<string,string> as alternative to config files
Combined .NETStandard2.0, .NET6.0 compatibility into single compilation
Addition of GetMatchSearch()
Addition of GetSquaddingList()

v1.0.5.0
Update Definitions logic
Addition of Definitions

v1.0.4.0
Move Message Response out of returned object
Add Unit Tests Project
Add self-contained External API for zipcode information

v1.0.3.0
Addition of GetResultCourseOfFireDetail()
Moved ResponseTemplate.cs from /DataModel/OrionMatch to /DataModel

v1.0.2.0
Updated data structure
Addition of GetResultList
Modified return json string processing in to object

v1.0.1.0
Addition of Response.TimeToRun yielding time (minutes:seconds:milliseconds) to query API and process returned json in to object

v1.0.0.0
Initial development build implementing GetMatchDetailAsync()