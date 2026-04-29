using helengine.baseplatform.Builders;
using helengine.baseplatform.Reporting;

namespace helengine.baseplatform.tests.Builders;

/// <summary>
/// Records diagnostics emitted by a builder so tests can assert the streamed reporting contract behavior.
/// </summary>
public class RecordingDiagnosticReporter : IPlatformBuildDiagnosticReporter {
    /// <summary>
    /// Initializes a new reporter with an empty diagnostic list.
    /// </summary>
    public RecordingDiagnosticReporter() {
        Diagnostics = [];
    }

    /// <summary>
    /// Gets the diagnostics that were reported during the test build.
    /// </summary>
    public List<PlatformBuildDiagnostic> Diagnostics { get; }

    /// <summary>
    /// Records one streamed diagnostic.
    /// </summary>
    /// <param name="diagnostic">The diagnostic emitted by the builder.</param>
    public void Report(PlatformBuildDiagnostic diagnostic) {
        if (diagnostic == null) {
            throw new ArgumentNullException(nameof(diagnostic));
        }

        Diagnostics.Add(diagnostic);
    }
}
