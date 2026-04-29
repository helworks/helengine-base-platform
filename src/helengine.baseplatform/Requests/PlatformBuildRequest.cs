using helengine.baseplatform.Manifest;

namespace helengine.baseplatform.Requests;

/// <summary>
/// Describes a single asset-platform build invocation using a fully resolved manifest and filesystem roots.
/// </summary>
public class PlatformBuildRequest {
    /// <summary>
    /// Initializes a new build request for one target platform output.
    /// </summary>
    /// <param name="manifest">The fully resolved manifest the builder must transform.</param>
    /// <param name="outputRoot">The root directory where cooked outputs should be written.</param>
    /// <param name="workingRoot">The temporary working directory the builder may use during execution.</param>
    public PlatformBuildRequest(PlatformBuildManifest manifest, string outputRoot, string workingRoot) {
        if (manifest == null) {
            throw new ArgumentNullException(nameof(manifest));
        } else if (string.IsNullOrWhiteSpace(outputRoot)) {
            throw new ArgumentException("Output root is required.", nameof(outputRoot));
        } else if (string.IsNullOrWhiteSpace(workingRoot)) {
            throw new ArgumentException("Working root is required.", nameof(workingRoot));
        }

        Manifest = manifest;
        OutputRoot = outputRoot;
        WorkingRoot = workingRoot;
    }

    /// <summary>
    /// Gets the fully resolved platform build manifest that the builder must process.
    /// </summary>
    public PlatformBuildManifest Manifest { get; }

    /// <summary>
    /// Gets the root directory where final cooked outputs should be written.
    /// </summary>
    public string OutputRoot { get; }

    /// <summary>
    /// Gets the temporary working directory the builder may use during execution.
    /// </summary>
    public string WorkingRoot { get; }
}
