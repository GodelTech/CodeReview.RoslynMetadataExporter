namespace GodelTech.CodeReview.RoslynMetadataExporter.Services
{
    public interface INuGetPackageProcessor
    {
        PackageDetails[] Process(AnalysisRequest args);
    }
}