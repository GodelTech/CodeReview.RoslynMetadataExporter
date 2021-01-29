namespace ReviewItEasy.RoslynMetadataExporter.Services
{
    public interface INuGetPackageProcessor
    {
        PackageDetails[] Process(AnalysisRequest args);
    }
}