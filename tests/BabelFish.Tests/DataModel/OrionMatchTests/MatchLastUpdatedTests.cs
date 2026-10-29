using System;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Responses.OrionMatchAPI;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatchTests;

/// <summary>Pure serialization tests: no BaseTestClass, initialization, or API requests.</summary>
[TestClass]
public class MatchLastUpdatedTests {
    [TestInitialize]
    public void InitializeSerializers() => SerializerOptions.InitSerializers();

    [TestMethod]
    [DataRow("2026-04-14T17:33:55.123456Z")]
    [DataRow("2026-04-14T17:33:55.123456+0000")]
    [DataRow("2026-04-14 17:33:55.123456+00:00")]
    [DataRow("2026-04-14T13:33:55.123456-04:00")]
    public void PublicMatchEnvelopeAcceptsSupportedTimestampFormats(string timestamp) {
        var json = "{\"Match\":{\"MatchID\":\"1.1.20260414173355.1\",\"LastUpdated\":\"" + timestamp + "\"}}";
        var wrapper = JsonSerializer.Deserialize<MatchWrapper>(json, SerializerOptions.SystemTextJsonDeserializer)!;
        var expected = new DateTime(2026, 4, 14, 17, 33, 55, DateTimeKind.Utc).AddTicks(1234560);
        Assert.AreEqual(expected, wrapper.Match.LastUpdated);
        Assert.AreEqual(DateTimeKind.Utc, wrapper.Match.LastUpdated.Kind);
    }

    [TestMethod]
    public void MatchSerializationPreservesScoposTimestampAndRoundTripsThroughBothReaders() {
        var expected = new DateTime(2026, 4, 14, 17, 33, 55, DateTimeKind.Utc).AddTicks(1234560);
        var match = new Match { MatchID = new("1.1.20260414173355.1"), LastUpdated = expected };
        var json = match.SerializeToJson();
        using var document = JsonDocument.Parse(json);
        Assert.AreEqual("2026-04-14T17:33:55.123456Z", document.RootElement.GetProperty("LastUpdated").GetString());
        var restored = JsonSerializer.Deserialize<Match>(json, SerializerOptions.SystemTextJsonDeserializer)!;
        Assert.AreEqual(expected, restored.LastUpdated);
        var restoredNewtonsoft = Newtonsoft.Json.JsonConvert.DeserializeObject<Match>(json, SerializerOptions.NewtonsoftJsonSerializer)!;
        Assert.AreEqual(expected, restoredNewtonsoft.LastUpdated.ToUniversalTime());
    }
}
