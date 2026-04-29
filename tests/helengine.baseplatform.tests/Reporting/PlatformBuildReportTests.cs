using helengine.baseplatform.Reporting;

namespace helengine.baseplatform.tests.Reporting;

/// <summary>
/// Verifies the final report preserves per-item failures and diagnostics.
/// </summary>
public class PlatformBuildReportTests {
    /// <summary>
    /// Ensures a failed scene outcome and its diagnostic are retained in the final report.
    /// </summary>
    [Fact]
    public void Constructor_WhenSceneFails_PreservesFailureDetails() {
        var diagnostic = new PlatformBuildDiagnostic(
            PlatformBuildDiagnosticSeverity.Error,
            "SCENE001",
            "Failed to cook scene.",
            "scene-main",
            string.Empty,
            "content/scenes/main.scene");

        var report = new PlatformBuildReport(
            false,
            [diagnostic],
            [new PlatformBuildItemOutcome("scene-main", PlatformBuildItemOutcomeKind.Failed)],
            []);

        Assert.False(report.Succeeded);
        Assert.Single(report.Diagnostics);
        Assert.Equal(PlatformBuildItemOutcomeKind.Failed, report.SceneOutcomes[0].OutcomeKind);
    }
}
