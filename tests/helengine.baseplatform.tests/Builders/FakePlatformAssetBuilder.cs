using helengine.baseplatform.Builders;
using helengine.baseplatform.Descriptors;
using helengine.baseplatform.Reporting;
using helengine.baseplatform.Requests;

namespace helengine.baseplatform.tests.Builders;

/// <summary>
/// Provides a minimal contract implementation that exercises the builder interfaces in tests.
/// </summary>
public class FakePlatformAssetBuilder : IPlatformAssetBuilder {
    /// <summary>
    /// Initializes a new fake builder with a deterministic descriptor.
    /// </summary>
    public FakePlatformAssetBuilder() {
        Descriptor = new PlatformBuilderDescriptor(
            "test-builder",
            "1.0.0",
            "windows",
            new EngineCompatibilityRange("1.0.0", "2.0.0"),
            new ManifestCompatibilityRange(1, 1));
    }

    /// <summary>
    /// Gets the deterministic descriptor exposed by the fake builder.
    /// </summary>
    public PlatformBuilderDescriptor Descriptor { get; }

    /// <summary>
    /// Emits one progress update and returns a successful final report.
    /// </summary>
    /// <param name="request">The build request being processed.</param>
    /// <param name="progressReporter">The reporter that receives streamed progress updates.</param>
    /// <param name="diagnosticReporter">The reporter that receives streamed diagnostics.</param>
    /// <param name="cancellationToken">The cancellation token used to stop the build cooperatively.</param>
    /// <returns>A successful final report with no diagnostics.</returns>
    public Task<PlatformBuildReport> BuildAsync(
        PlatformBuildRequest request,
        IPlatformBuildProgressReporter progressReporter,
        IPlatformBuildDiagnosticReporter diagnosticReporter,
        CancellationToken cancellationToken) {
        if (request == null) {
            throw new ArgumentNullException(nameof(request));
        } else if (progressReporter == null) {
            throw new ArgumentNullException(nameof(progressReporter));
        } else if (diagnosticReporter == null) {
            throw new ArgumentNullException(nameof(diagnosticReporter));
        }

        progressReporter.Report(new PlatformBuildProgressUpdate(
            "Cook Scenes",
            request.Manifest.Scenes[0].SceneId,
            1,
            1,
            "Cooked the test scene."));

        return Task.FromResult(new PlatformBuildReport(
            true,
            [],
            [new PlatformBuildItemOutcome(request.Manifest.Scenes[0].SceneId, PlatformBuildItemOutcomeKind.Succeeded)],
            []));
    }
}
