using helengine.baseplatform.Descriptors;

namespace helengine.baseplatform.tests.Builders;

/// <summary>
/// Verifies the contract library can be referenced by the test project.
/// </summary>
public class PlatformAssetBuilderContractTests {
    /// <summary>
    /// Proves the test project can construct a simple compatibility range once the contract exists.
    /// </summary>
    [Fact]
    public void ContractAssembly_WhenReferenced_ExposesCompatibilityTypes() {
        var range = new EngineCompatibilityRange("1.0.0", "2.0.0");

        Assert.Equal("1.0.0", range.MinimumVersion);
    }
}
