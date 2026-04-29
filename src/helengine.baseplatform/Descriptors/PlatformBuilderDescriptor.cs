namespace helengine.baseplatform.Descriptors;

/// <summary>
/// Describes a platform asset builder identity and the compatibility ranges it supports.
/// </summary>
public class PlatformBuilderDescriptor {
    /// <summary>
    /// Initializes a new builder descriptor with explicit identity and compatibility metadata.
    /// </summary>
    /// <param name="builderId">The stable identifier for the builder package.</param>
    /// <param name="builderVersion">The version of the builder implementation.</param>
    /// <param name="targetPlatformId">The target platform identifier served by the builder.</param>
    /// <param name="supportedEngineVersions">The inclusive engine version range the builder supports.</param>
    /// <param name="supportedManifestVersions">The inclusive manifest version range the builder supports.</param>
    /// <exception cref="ArgumentException">Thrown when any required string value is missing.</exception>
    /// <exception cref="ArgumentNullException">Thrown when a required compatibility object is missing.</exception>
    public PlatformBuilderDescriptor(
        string builderId,
        string builderVersion,
        string targetPlatformId,
        EngineCompatibilityRange supportedEngineVersions,
        ManifestCompatibilityRange supportedManifestVersions) {
        if (string.IsNullOrWhiteSpace(builderId)) {
            throw new ArgumentException("Builder id is required.", nameof(builderId));
        } else if (string.IsNullOrWhiteSpace(builderVersion)) {
            throw new ArgumentException("Builder version is required.", nameof(builderVersion));
        } else if (string.IsNullOrWhiteSpace(targetPlatformId)) {
            throw new ArgumentException("Target platform id is required.", nameof(targetPlatformId));
        } else if (supportedEngineVersions == null) {
            throw new ArgumentNullException(nameof(supportedEngineVersions), "Supported engine versions are required.");
        } else if (supportedManifestVersions == null) {
            throw new ArgumentNullException(nameof(supportedManifestVersions), "Supported manifest versions are required.");
        }

        BuilderId = builderId;
        BuilderVersion = builderVersion;
        TargetPlatformId = targetPlatformId;
        SupportedEngineVersions = supportedEngineVersions;
        SupportedManifestVersions = supportedManifestVersions;
    }

    /// <summary>
    /// Gets the stable identifier for the builder package.
    /// </summary>
    public string BuilderId { get; }

    /// <summary>
    /// Gets the version of the builder implementation.
    /// </summary>
    public string BuilderVersion { get; }

    /// <summary>
    /// Gets the target platform identifier served by the builder.
    /// </summary>
    public string TargetPlatformId { get; }

    /// <summary>
    /// Gets the inclusive engine version range supported by the builder.
    /// </summary>
    public EngineCompatibilityRange SupportedEngineVersions { get; }

    /// <summary>
    /// Gets the inclusive manifest version range supported by the builder.
    /// </summary>
    public ManifestCompatibilityRange SupportedManifestVersions { get; }
}
