using helengine.baseplatform.Descriptors;

namespace helengine.baseplatform.tests.Descriptors;

/// <summary>
/// Verifies the descriptor exposes builder identity and compatibility metadata.
/// </summary>
public class PlatformBuilderDescriptorTests {
    /// <summary>
    /// Ensures the descriptor keeps explicit ids and compatibility ranges.
    /// </summary>
    [Fact]
    public void Constructor_WhenValuesAreValid_AssignsAllMetadata() {
        var descriptor = new PlatformBuilderDescriptor(
            "windows-desktop",
            "1.2.0",
            "windows",
            new EngineCompatibilityRange("1.0.0", "2.0.0"),
            new ManifestCompatibilityRange(1, 3));

        Assert.Equal("windows-desktop", descriptor.BuilderId);
        Assert.Equal("windows", descriptor.TargetPlatformId);
        Assert.Equal(1, descriptor.SupportedManifestVersions.MinimumVersion);
    }
}
