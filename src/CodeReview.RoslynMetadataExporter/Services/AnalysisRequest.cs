namespace GodelTech.CodeReview.RoslynMetadataExporter.Services
{
    public class AnalysisRequest
    {
        public string PackageId { get; set; }
        public string Framework { get; set; }
        public string PackageVersion { get; set; }
        public string Language { get; set; }
        public bool ScanDependencyPackages { get; set; }
    }
}