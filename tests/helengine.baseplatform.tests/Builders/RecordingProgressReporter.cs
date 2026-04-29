using helengine.baseplatform.Builders;
using helengine.baseplatform.Reporting;

namespace helengine.baseplatform.tests.Builders;

/// <summary>
/// Records progress updates emitted by a builder so tests can assert the streamed contract behavior.
/// </summary>
public class RecordingProgressReporter : IPlatformBuildProgressReporter {
    /// <summary>
    /// Initializes a new reporter with an empty update list.
    /// </summary>
    public RecordingProgressReporter() {
        Updates = [];
    }

    /// <summary>
    /// Gets the progress updates that were reported during the test build.
    /// </summary>
    public List<PlatformBuildProgressUpdate> Updates { get; }

    /// <summary>
    /// Records one streamed progress update.
    /// </summary>
    /// <param name="update">The progress update emitted by the builder.</param>
    public void Report(PlatformBuildProgressUpdate update) {
        if (update == null) {
            throw new ArgumentNullException(nameof(update));
        }

        Updates.Add(update);
    }
}
