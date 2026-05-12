# BabelFish AI Coding Instructions

BabelFish is a .NET library (`netstandard2.1`) providing a data model and REST API façade for Scopos' shooting sports platform. It powers three core domains: Orion Match data, score history/analytics, and result list formatting.

## Architecture Overview

**Three-Tier Structure:**
- **APIClients** (`src/BabelFish/APIClients/`): Generic base `APIClient<T>` pattern with concrete clients like `OrionMatchAPIClient`, `DefinitionAPIClient`, `ScoreHistoryAPIClient`. All require initialization via `Initializer.Initialize(xApiKey)` before use.
- **DataModel** (`src/BabelFish/DataModel/`): Organized by domain (OrionMatch, ScoreHistory, Definitions, Athena, SocialNetwork, Clubs). Many use abstract base classes (e.g., `MatchBase`, `ScoreHistoryBase`, `AttributeFilter`) with polymorphic JSON deserialization.
- **DataActors** (`src/BabelFish/DataActors/`): Business logic for result list formatting, result list merging, and tournament calculations. `ResultListIntermediateFormatted` is the primary formatter.

**Key Patterns:**
- **Serialization**: Dual support for both System.Text.Json and Newtonsoft.Json via converters in `Converters/` folder. GlobalUsing aliases: `G_STJ` (System.Text.Json), `G_NS` (Newtonsoft.Json), `G_BF_STJ_CONV`/`G_BF_NS_CONV` for BabelFish converters.
- **Attribute Ordering**: JSON property order is explicit (e.g., `[G_NS.JsonProperty(Order=1)]`). When adding properties, preserve existing order values and insert new ones in logical places.
- **Token/Pagination**: `ITokenRequest` interface for paginated API responses; `NextToken` in responses indicates more data available.

## Essential Workflows

**Initialization (Critical):**
```csharp
Initializer.Initialize(xApiKey, false);  // false skips definition preload in tests
Initializer.UpdateLocalStoreDirectory(@"C:\temp");
```

**Testing:** Inherit from `BaseTestClass` in `tests/BabelFish.Tests/BaseTestClass.cs`. It reads `ScoposXApiKey` environment variable and initializes BabelFish without preloading definitions.

**Building:** Use `BuildInternal.ps1` which auto-increments FileVersion. Set environment variables: `BF_REPO_PATH`, `NUGET_PATH`.

## Code Conventions

**Global Usings**: Use aliases from `GlobalUsing.cs` (never full namespaces):
- Newtonsoft.Json attributes → prefix with `G_NS.`
- System.Text.Json attributes → prefix with `G_STJ.`
- BabelFish converters → prefix with `G_BF_NS_CONV.` or `G_BF_STJ_CONV.`

**Attribute Order for JSON Properties:**
1. System.Text.Json attributes (`G_STJ_SER.JsonPropertyOrder`)
2. Newtonsoft.Json attributes (`G_NS.JsonProperty`)
3. System attributes (e.g., `[DefaultValue]`)

**Abstract Classes & Polymorphism:**
When deserializing abstract types, implement custom converters in both `Converters/System.Text.Json/` and `Converters/Newtonsoft.Json/`. Register in `SerializerOptions.InitSerializers()`.

**Caching:** `DefinitionCache` and `ResponseCache` provide caching for definitions and API responses. Set `LocalStoreDirectory` on APIClient subclasses to enable persistence.

## Domain-Specific Knowledge

- **MatchID**: Unique match identifier in format like "1.1.2025100109364878.1"
- **Result Lists**: Primary results identified by `Primary` flag in `ResultListAbbr`
- **Definitions**: Reconfigurable rulebook objects implementing `IReconfigurableRulebookObject`
- **SetName**: Recently changed from string to object; null deserialization defaults to "1.0:orion:Default"
- **Visibility**: `MatchBase.Visibility` uses `VisibilityOption` enum (e.g., PRIVATE, PUBLIC)

## File Organization Guidelines

- Converters for each JSON type go in both `Converters/System.Text.Json/` AND `Converters/Newtonsoft.Json/`
- Request/Response DTOs in `Requests/` and `Responses/` organized by API (e.g., `Requests/OrionMatch/`)
- New data model classes inherit from domain-specific base classes or `BaseClass`
- Add `[NamespaceGroupDoc]` attributes for Sandcastle documentation grouping
