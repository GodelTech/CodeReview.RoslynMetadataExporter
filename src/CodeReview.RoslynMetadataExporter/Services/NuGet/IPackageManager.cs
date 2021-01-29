namespace ReviewItEasy.RoslynMetadataExporter.Services.NuGet
{
    public interface IPackageManager
    {
        bool Install(string targetFolder, AnalysisRequest request);
        PackageMetadata[] Find(string targetFolder, AnalysisRequest request);
    }
}