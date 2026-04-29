using helengine.baseplatform.Manifest;

namespace helengine.baseplatform.tests;

/// <summary>
/// Creates valid manifest snapshots shared by multiple contract tests.
/// </summary>
public static class TestManifestFactory {
    /// <summary>
    /// Creates a minimal valid manifest that exercises the contract surface without extra fixture noise.
    /// </summary>
    /// <returns>A valid minimal platform build manifest.</returns>
    public static PlatformBuildManifest Create() {
        var scene = new PlatformBuildScene(
            "scene-main",
            "MainScene",
            "content/scenes/main.scene",
            [new PlatformBuildPayloadReference("mesh-sponza", "content/models/sponza.obj")],
            []);

        return new PlatformBuildManifest(
            1,
            "city",
            "1.0.0",
            "1.4.0",
            "windows",
            [scene],
            []);
    }
}
