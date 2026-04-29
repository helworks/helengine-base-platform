using helengine.baseplatform.Descriptors;

namespace helengine.baseplatform.tests.Descriptors;

/// <summary>
/// Covers engine-version compatibility rules for builders.
/// </summary>
public class EngineCompatibilityRangeTests {
    /// <summary>
    /// Ensures the range rejects a maximum version that sorts below the minimum version.
    /// </summary>
    [Fact]
    public void Constructor_WhenMaximumVersionIsLowerThanMinimum_Throws() {
        Assert.Throws<ArgumentException>(() => new EngineCompatibilityRange("2.0.0", "1.0.0"));
    }
}
