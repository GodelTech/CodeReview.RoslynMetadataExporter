using Microsoft.CodeAnalysis.Diagnostics;

namespace GodelTech.CodeReview.RoslynMetadataExporter.Services.Diagnostics
{
    public interface IDiagnosticsFactory
    {
        /// <summary>
        /// Loads all of the given assemblies and instantiates Roslyn diagnostic objects - i.e. existing types deriving from
        /// <see cref="DiagnosticAnalyzer"/>. Non-assembly analyzerAssemblies will be ignored.
        /// </summary>
        /// <returns>Enumerable with instances of DiagnosticAnalyzer from discovered assemblies</returns>
        DiagnosticAnalyzer[] Create(string language, string packageFolder, string[] analyzerAssemblies);
    }
}