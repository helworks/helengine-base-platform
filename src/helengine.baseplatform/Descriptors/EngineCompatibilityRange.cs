namespace helengine.baseplatform.Descriptors;

/// <summary>
/// Defines the inclusive engine-version bounds a platform builder supports.
/// </summary>
public class EngineCompatibilityRange {
    /// <summary>
    /// Initializes a new compatibility range with inclusive minimum and maximum versions.
    /// </summary>
    /// <param name="minimumVersion">The inclusive minimum engine version supported by the builder.</param>
    /// <param name="maximumVersion">The inclusive maximum engine version supported by the builder.</param>
    /// <exception cref="ArgumentException">Thrown when either bound is missing.</exception>
    public EngineCompatibilityRange(string minimumVersion, string maximumVersion) {
        if (string.IsNullOrWhiteSpace(minimumVersion)) {
            throw new ArgumentException("Minimum engine version is required.", nameof(minimumVersion));
        } else if (string.IsNullOrWhiteSpace(maximumVersion)) {
            throw new ArgumentException("Maximum engine version is required.", nameof(maximumVersion));
        }

        MinimumVersion = minimumVersion;
        MaximumVersion = maximumVersion;
    }

    /// <summary>
    /// Gets the inclusive minimum engine version supported by the builder.
    /// </summary>
    public string MinimumVersion { get; }

    /// <summary>
    /// Gets the inclusive maximum engine version supported by the builder.
    /// </summary>
    public string MaximumVersion { get; }
}
