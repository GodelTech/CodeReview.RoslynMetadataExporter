using Microsoft.CodeAnalysis.Diagnostics;

namespace ReviewItEasy.RoslynMetadataExporter.Services
{
    public class PackageDetails
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public DiagnosticAnalyzer[] Diagnostics { get; set; }
    }
}