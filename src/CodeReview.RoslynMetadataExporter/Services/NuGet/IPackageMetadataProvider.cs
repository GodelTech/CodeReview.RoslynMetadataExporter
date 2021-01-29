namespace ReviewItEasy.RoslynMetadataExporter.Services.NuGet
{
    public interface IPackageMetadataProvider
    {
        PackageMetadata Get(string folderPath);
    }
}