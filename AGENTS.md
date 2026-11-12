# BabelFish repository instructions

## Purpose and organization

BabelFish owns shared .NET API clients, request/response wrappers, data models and converters, scoring/result actors, and report support. It is a library initialized by consumers, not an application entry point.

- `BabelFish.sln` contains Core, Reports, and MSTest projects. `src/BabelFish/BabelFish.csproj` and `src/BabelFish.Reports/BabelFish.Reports.csproj` target `netstandard2.1;netstandard2.0`; Reports references Core.
- Under `src/BabelFish/`: `APIClients/` owns client operations; `Requests/` owns HTTP methods, paths, parameters and authentication requirements; `Responses/` owns response wrappers; `DataModel/` and `Converters/` own wire representations.
- `DataActors/` contains result/scoring/formatting behavior. `Runtime/` includes initialization/authentication. `Helpers/SerializerOptions.cs` centralizes serializer options. Reporting code lives separately in `src/BabelFish.Reports/`.

## Contract and coding conventions

1. Trace an operation through its client, concrete request, response wrapper, model, and converters. `src/BabelFish/APIClients/APIClient.cs`, `APISubDomain.cs`, and `APIStage.cs` compose endpoint selection. Match the actual route, verb, parameters, and named response payload to RestAPI; a matching method name does not prove routing.
2. Preserve public signatures and wire contracts: property names, enum descriptions/values, polymorphic identifiers, dates/versions, missing/null/default behavior, and `Items`/`NextToken` pagination where used. Follow neighboring family-specific request/wrapper patterns rather than inventing a parallel client framework.
3. `src/BabelFish/Responses/Response.cs` deserializes successful responses through System.Text.Json; `Helpers/SerializerOptions.cs` also contains Newtonsoft settings used by consumers. Check the actual serialization direction and converter registrations. Default serializers are not interchangeable, and changes must account for both where relevant.
4. Trace public versus credential-bearing requests through `src/BabelFish/Requests/Request.cs` and `src/BabelFish/Runtime/Authentication/UserAuthentication.cs`. API-key headers, Cognito sessions, and signed IAM requests serve different purposes; preserve that distinction.
5. Result/device model changes can also affect Orion/EST MQTT exchanges, not only HTTP responses. Use representative offline payloads for changed converters, including relevant legacy shapes and boundary cases.

## Consumers, versions, and distribution

- Check the package version actually consumed by each affected application. Orion uses `packages.config`/assembly hint paths; Rezults uses `PackageReference` in its projects. Neither automatically builds against this sibling checkout.
- Historical source declarations are Core/Reports 1.12.6.3/1.12.4.4, matching Orion, while Rezults declares 2.1.10-beta.5/2.1.4-beta.4. Developer policy (2026-10-19): `release_2.0` is the latest release line and produced Core 2.1.10-beta.5; assume the locally checked-out BabelFish branch contains the code used by the checked-out Rezults and Orion branches. Record metadata discrepancies without blocking routine work on old-package support, and do not switch branches automatically. Exact artifact/commit hashes remain separate evidence.
- Keep package IDs, target frameworks, dependency references, and release notes in `CHANGELOG.md` consistent with any explicitly requested release change. `src/BabelFish/PublishToNuget.ps1` publishes packages and is separate from local validation. Source edits do not publish or upgrade consumers.
- For API changes, compare RestAPI's handler/layers and direct EST consumers when they share the endpoint. Use the developer-designated source baseline while checking actual endpoint coverage; the library need not implement every backend operation.

## Developer-confirmed contract policy (2026-10-19)

- Orion and Rezults are the only BabelFish consumers. Rezults is controlled by the team; Orion users must update frequently. API changes generally need not support older BabelFish packages.
- Updated RestAPI responses must not cause BabelFish errors. The usual workflow changes the response first and then aligns this library's structures. Preserve or intentionally change wire fields/converters against the designated current baseline; do not equate coordinated change with accepting deserialization failures.
- Older clients must still be able to update to the latest release. Retain release-discovery and migration compatibility that enables that transition; do not infer a blanket requirement to preserve every old runtime API.
- No consistent validation workflow exists yet. Review candidate tests and use focused offline checks; do not claim these suggestions are established acceptance gates.

## Build, tests, and safety

- Project files support `dotnet build BabelFish.sln` from this root with a suitable SDK/package environment. The test project `tests/BabelFish.Tests/BabelFish.Tests.csproj` targets .NET 8. These commands were not executed for this guidance; clean-machine restore/build prerequisites remain unverified.
- Inspect `tests/BabelFish.Tests/BaseTestClass.cs`, inherited initialization, and the specific test before running it. `tests/BabelFish.Tests/ScoreHistory/AuthenticatedScoreHistoryTests.cs` creates production clients and posts scores. Do not run the entire suite as an offline check.
- After selecting a reviewed offline test, use `dotnet test tests/BabelFish.Tests/BabelFish.Tests.csproj --filter 'FullyQualifiedName=<reviewed-test-name>'`, replacing the placeholder. A filter does not neutralize initialization or network effects; mock/isolate dependencies first where necessary.
- Inspect Git status and preserve unrelated changes. Avoid destructive commands. Do not commit, push, publish NuGet packages, call live APIs, connect to databases, use AWS, or deploy without authorization. Never expose API keys, credentials, signing material, or customer data. For documentation-only work, validate paths and diffs without building or running suites.

## Evidence and optional shared context

**Verified** means supported by source/project/test evidence, not deployed behavior. Use **Developer-provided** for unconfirmed owner context and **Unresolved** for missing evidence. The release line, checked-out-source convention, and older-client support policy are developer-confirmed above. Exact published package hashes/commit mapping and clean-machine validation remain unverified.

ScoposWorkspace maintains the shared knowledge base. If available, read [workspace instructions](../../AGENTS.md), [architecture](../../ARCHITECTURE.md), and [API architecture](../../docs/API_ARCHITECTURE.md) under `../../docs/`; use [edge architecture](../../docs/EDGE_ARCHITECTURE.md) for device model changes. These paths assume the standard workspace layout and are optional for standalone clones.
