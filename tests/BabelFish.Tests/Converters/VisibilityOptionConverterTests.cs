using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.Tests.Converters;

/// <summary>Offline wire-format checks only; no SDK initializer, credentials or API reads.</summary>
[TestClass]
public class VisibilityOptionConverterTests {
    private static JsonSerializerOptions Options() => new() { Converters = { new VisibilityOptionJsonConverter() } };

    [TestMethod]
    [DataRow("Public", VisibilityOption.PUBLIC)]
    [DataRow("PUBLIC", VisibilityOption.PUBLIC)]
    [DataRow("public", VisibilityOption.PUBLIC)]
    [DataRow("PuBlIc", VisibilityOption.PUBLIC)]
    [DataRow("Internal", VisibilityOption.INTERNAL)]
    [DataRow("INTERNAL", VisibilityOption.INTERNAL)]
    [DataRow("internal", VisibilityOption.INTERNAL)]
    [DataRow("Protected", VisibilityOption.PROTECTED)]
    [DataRow("PROTECTED", VisibilityOption.PROTECTED)]
    [DataRow("protected", VisibilityOption.PROTECTED)]
    [DataRow("Private", VisibilityOption.PRIVATE)]
    [DataRow("PRIVATE", VisibilityOption.PRIVATE)]
    [DataRow("private", VisibilityOption.PRIVATE)]
    [DataRow("", VisibilityOption.PRIVATE)]
    [DataRow("unknown", VisibilityOption.PRIVATE)]
    [DataRow(" Public ", VisibilityOption.PRIVATE)]
    public void ReadAcceptsVisibilityCasingAndKeepsUnknownValuesPrivate(string value, VisibilityOption expected) =>
        Assert.AreEqual(expected, JsonSerializer.Deserialize<VisibilityOption>(JsonSerializer.Serialize(value), Options()));

    [TestMethod]
    public void NullRemainsPrivate() =>
        Assert.AreEqual(VisibilityOption.PRIVATE, JsonSerializer.Deserialize<VisibilityOption>("null", Options()));

    [TestMethod]
    [DataRow(VisibilityOption.PUBLIC, "Public")]
    [DataRow(VisibilityOption.INTERNAL, "Internal")]
    [DataRow(VisibilityOption.PROTECTED, "Protected")]
    [DataRow(VisibilityOption.PRIVATE, "Private")]
    public void WritePreservesExistingTitleCaseWireValues(VisibilityOption value, string expected) =>
        Assert.AreEqual(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(value, Options()));
}
