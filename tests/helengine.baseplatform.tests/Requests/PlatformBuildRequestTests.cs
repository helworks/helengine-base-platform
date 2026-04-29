using helengine.baseplatform.Requests;

namespace helengine.baseplatform.tests.Requests;

/// <summary>
/// Verifies build requests require explicit filesystem roots.
/// </summary>
public class PlatformBuildRequestTests {
    /// <summary>
    /// Ensures the request rejects a missing output root.
    /// </summary>
    [Fact]
    public void Constructor_WhenOutputRootIsMissing_Throws() {
        var manifest = TestManifestFactory.Create();

        Assert.Throws<ArgumentException>(() => new PlatformBuildRequest(manifest, string.Empty, "temp"));
    }
}
