using helengine.baseplatform.Manifest;

namespace helengine.baseplatform.tests.Manifest;

/// <summary>
/// Verifies the build manifest keeps fully resolved scene and asset inputs.
/// </summary>
public class PlatformBuildManifestTests {
    /// <summary>
    /// Ensures scenes carry their own payload references without requiring cross-document lookup.
    /// </summary>
    [Fact]
    public void Constructor_WhenSceneContainsPayloads_PreservesThemInPlace() {
        var scene = new PlatformBuildScene(
            "scene-main",
            "MainScene",
            "content/scenes/main.scene",
            [new PlatformBuildPayloadReference("mesh-sponza", "content/models/sponza.obj")],
            []);

        var manifest = new PlatformBuildManifest(
            1,
            "city",
            "1.0.0",
            "1.4.0",
            "windows",
            [scene],
            []);

        Assert.Single(manifest.Scenes);
        Assert.Single(manifest.Scenes[0].PayloadReferences);
    }
}
