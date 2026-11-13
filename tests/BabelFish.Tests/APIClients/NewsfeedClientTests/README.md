# Newsfeed post client checks

`NewsfeedAPIClient` implements authenticated `POST /newsfeed/posts` and
`DELETE /newsfeed/posts/{post-id}` from RestAPI's
`Lambda/Functions/newsfeed/NewsfeedPostCRUD/lambda_function.py`.
These operations return `Post` and `DeletedPost`, respectively. IDs remain
decimal strings across the full unsigned BIGINT range. Creation sends a JSON
object containing `body-text`, plus an optional `license-number` query parameter.
No GET, update, or pagination operation is supplied by that handler.

```csharp
var client = new NewsfeedAPIClient(APIStage.BETA);
var response = await client.CreateNewsfeedPostAuthenticatedAsync("Announcement", credentials);
if (response.HasOkStatusCode && response.Post != null) {
    var deleted = await client.DeleteNewsfeedPostAuthenticatedAsync(response.Post.PostId, credentials);
}
```

The client also accepts request objects. Supply the optional `licenseNumber`
argument for club posts; the backend checks the caller's permissions. All
mutations bypass caches. A timed-out POST can have committed and must not be
automatically retried. DELETE retains the row/text and preserves its first
deletion audit fields across authorized retries.

From the BabelFish root:

```powershell
dotnet build src/BabelFish/BabelFish.csproj
dotnet build tests/BabelFish.Tests/BabelFish.Tests.csproj
dotnet test tests/BabelFish.Tests/BabelFish.Tests.csproj --no-build --filter 'FullyQualifiedName~Scopos.BabelFish.Tests.APIClients.NewsfeedClientTests.NewsfeedPostContractTests'
```

The contract class has no cloud or authentication setup. It checks wire requests,
validation bounds, response conversion through the shared `Response.Body` setter,
both JSON serializers, nullable fields, large IDs, UTC timestamps and errors.

The live test requires explicit opt-in and local credentials. The CSV is the
user-supplied account table with `Alias Accounts`, `User Name`, `Password`, and
`User ID` columns; introductory notes are allowed. Never commit that file or
credential values. `ScoposXApiKey` must already exist in the process environment.

```powershell
$env:SCOPOS_NEWSFEED_LIVE_BETA = '1'
$env:SCOPOS_TEST_USERS_CSV = 'C:\path\to\test_users.csv'
$env:SCOPOS_NEWSFEED_TEST_ALIAS = 'test_dev_7' # default approved personal-post test alias
dotnet test tests/BabelFish.Tests/BabelFish.Tests.csproj --no-build --filter 'FullyQualifiedName=Scopos.BabelFish.Tests.APIClients.NewsfeedClientTests.NewsfeedPostBetaTests.PersonalPostCreateDeleteAndRetryThroughBetaClient' --logger 'console;verbosity=detailed'
```

This test authenticates the selected test account using BabelFish, pins application
calls to `APIStage.BETA`, and creates one uniquely marked personal post. It checks
the complete result, deletes that exact ID, and repeats deletion to check audit
stability. Cleanup runs in `finally` when an ID is known. It never retries POST,
changes club roles, or physically removes SQL rows. Missing opt-in/inputs produce
an inconclusive result before authentication. It does not exercise club permissions.

**Verified — 2026-11-13:** 33 offline cases passed. One live Beta personal-post
round-trip test passed using `test_dev_7` through the actual client pipeline;
POST, DELETE, and repeated DELETE returned HTTP 200, with cleanup complete.
Restricted-network authentication initially failed at TLS; the authorized live
run succeeded outside that sandbox without disabling certificate validation.
This is HTTP evidence, not an independent SQL or deployment configuration audit.
