using helengine.baseplatform.Requests;

namespace helengine.baseplatform.tests.Builders;

/// <summary>
/// Verifies the platform asset builder contract can stream progress and diagnostics.
/// </summary>
public class PlatformAssetBuilderContractTests {
    /// <summary>
    /// Ensures builders expose descriptor metadata and return a final report after reporting progress.
    /// </summary>
    [Fact]
    public async Task BuildAsync_WhenInvoked_StreamsProgressAndReturnsReport() {
        var builder = new FakePlatformAssetBuilder();
        var progressReporter = new RecordingProgressReporter();
        var diagnosticReporter = new RecordingDiagnosticReporter();

        var report = await builder.BuildAsync(
            new PlatformBuildRequest(TestManifestFactory.Create(), "out", "temp"),
            progressReporter,
            diagnosticReporter,
            CancellationToken.None);

        Assert.Equal("test-builder", builder.Descriptor.BuilderId);
        Assert.Single(progressReporter.Updates);
        Assert.True(report.Succeeded);
    }
}
